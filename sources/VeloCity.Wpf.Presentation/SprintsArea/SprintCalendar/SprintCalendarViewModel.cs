// Velo City
// Copyright (C) 2022 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintCalendar
{
    public class SprintCalendarViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private List<SprintCalendarItemViewModel> calendarItems;
        private List<NoteBase> notes;

        public List<SprintCalendarItemViewModel> CalendarItems
        {
            get => calendarItems;
            private set
            {
                calendarItems = value;
                OnPropertyChanged();
            }
        }

        public List<NoteBase> Notes
        {
            get => notes;
            private set
            {
                notes = value;
                OnPropertyChanged();
            }
        }

        public SprintCalendarViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintCalendar();
        }

        private async Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintCalendar();
        }

        private async Task RetrieveSprintCalendar()
        {
            PresentSprintCalendarRequest request = new();

            PresentSprintCalendarResponse response = await mediator.Send(request);

            DisplayResponse(response);
        }

        private void DisplayResponse(PresentSprintCalendarResponse response)
        {
            List<SprintCalendarItemViewModel> calendarItems = CreateCalendarItems(response.SprintDays, response.SprintMembers);
            CreateChartBars(calendarItems);

            CalendarItems = calendarItems;
            Notes = CreateNotes();
        }

        private static List<SprintCalendarItemViewModel> CreateCalendarItems(IEnumerable<SprintDay> sprintDays, IEnumerable<SprintMember> sprintMembers)
        {
            return sprintDays
                .Select(x =>
                {
                    List<SprintMemberDay> sprintMemberDays = GetAllSprintMemberDays(x.Date, sprintMembers);
                    return new SprintCalendarItemViewModel(x, sprintMemberDays);
                })
                .ToList();
        }

        private static List<SprintMemberDay> GetAllSprintMemberDays(DateTime date, IEnumerable<SprintMember> sprintMembers)
        {
            if (sprintMembers == null)
                return new List<SprintMemberDay>();

            return sprintMembers
                .Select(x => x.Days[date])
                .Where(x => x != null)
                .ToList();
        }

        private List<NoteBase> CreateNotes()
        {
            List<NoteBase> notes = new();

            IEnumerable<TeamMemberAbsenceDetails> memberAbsenceDetailsViewModels = CalendarItems
                .SelectMany(x => x.AbsenceDetails.TeamMemberVacationDetails ?? Enumerable.Empty<TeamMemberAbsenceDetails>());

            bool isPartialVacationNoteVisible = false;
            bool isMissingByContractNoteVisible = false;

            foreach (TeamMemberAbsenceDetails teamMemberAbsenceDetailsViewModel in memberAbsenceDetailsViewModels)
            {
                if (teamMemberAbsenceDetailsViewModel.IsPartialVacation)
                    isPartialVacationNoteVisible = true;

                if (teamMemberAbsenceDetailsViewModel.IsMissingByContract)
                    isMissingByContractNoteVisible = true;
            }

            if (isPartialVacationNoteVisible)
                notes.Add(new PartialDayVacationNote());

            if (isMissingByContractNoteVisible)
                notes.Add(new AbsentByContractNote());

            return notes;
        }

        private static void CreateChartBars(IEnumerable<SprintCalendarItemViewModel> calendarItems)
        {
            Chart chart = new()
            {
                ActualSize = 100
            };

            IEnumerable<ChartBar> chartBars = calendarItems
                .Select(x =>
                {
                    int workHours = x.WorkHours?.Value ?? 0;
                    int absenceHours = x.AbsenceHours?.Value ?? 0;

                    ChartBar chartBar = new()
                    {
                        MaxValue = workHours + absenceHours,
                        FillValue = workHours
                    };

                    if (x.IsWorkDay)
                        x.ChartBar = chartBar;

                    return chartBar;
                });

            chart.AddRange(chartBars);
            chart.Calculate();
        }
    }
}