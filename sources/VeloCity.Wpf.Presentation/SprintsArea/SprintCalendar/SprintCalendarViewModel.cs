// VeloCity
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
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintCalendar
{
    public class SprintCalendarViewModel : ViewModelBase
    {
        private readonly IRequestBus requestBus;
        private List<SprintCalendarDayViewModel> sprintCalendarDays;

        public List<SprintCalendarDayViewModel> SprintCalendarDays
        {
            get => sprintCalendarDays;
            private set
            {
                sprintCalendarDays = value;
                OnPropertyChanged();
            }
        }

        public SprintCalendarViewModel(IRequestBus requestBus, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
            eventBus.Subscribe<TeamMemberVacationChangedEvent>(HandleTeamMemberVacationChangedEvent);
            eventBus.Subscribe<SprintsListChangedEvent>(HandleSprintsListChangedEvent);
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintCalendar();
        }

        private async Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintCalendar();
        }

        private async Task HandleTeamMemberVacationChangedEvent(TeamMemberVacationChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintCalendar();
        }

        private async Task HandleSprintsListChangedEvent(SprintsListChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintCalendar();
        }

        private async Task RetrieveSprintCalendar()
        {
            PresentSprintCalendarRequest request = new();
            PresentSprintCalendarResponse response = await requestBus.Send<PresentSprintCalendarRequest, PresentSprintCalendarResponse>(request);

            DisplayResponse(response);
        }

        private void DisplayResponse(PresentSprintCalendarResponse response)
        {
            List<SprintCalendarDayViewModel> sprintCalendarDays = CreateCalendarItems(response.SprintCalendarDays);
            CreateChartBars(sprintCalendarDays);

            SprintCalendarDays = sprintCalendarDays;
        }

        private static List<SprintCalendarDayViewModel> CreateCalendarItems(IEnumerable<SprintCalendarDay> sprintCalendarDays)
        {
            return sprintCalendarDays
                .Select(x => new SprintCalendarDayViewModel(x))
                .ToList();
        }

        private static void CreateChartBars(IEnumerable<SprintCalendarDayViewModel> calendarItems)
        {
            SprintWorkChart chart = new(calendarItems);

            foreach (ChartBarValue<SprintCalendarDayViewModel> chartBarValue in chart)
            {
                if (chartBarValue.Item?.IsWorkDay == true)
                    chartBarValue.Item.ChartBarValue = chartBarValue;
            }
        }
    }
}