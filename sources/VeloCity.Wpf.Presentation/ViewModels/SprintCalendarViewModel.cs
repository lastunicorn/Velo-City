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
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Presentation.ViewModels
{
    public class SprintCalendarViewModel
    {
        public List<CalendarItemViewModel> CalendarItems { get; }

        public List<NoteBase> Notes { get; }

        public SprintCalendarViewModel(List<SprintDay> sprintDays, IEnumerable<SprintMember> sprintMembers)
        {
            if (sprintDays == null) throw new ArgumentNullException(nameof(sprintDays));

            CalendarItems = CreateCalendarItems(sprintDays, sprintMembers);
            Notes = CreateNotes();

            Chart chart = CreateChart();
        }

        private Chart CreateChart()
        {
            Chart chart = new()
            {
                ActualSize = 100
            };

            IEnumerable<ChartBar> chartBars = CalendarItems
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

            return chart;
        }

        private static List<CalendarItemViewModel> CreateCalendarItems(IEnumerable<SprintDay> sprintDays, IEnumerable<SprintMember> sprintMembers)
        {
            return sprintDays
                .Select(x =>
                {
                    List<SprintMemberDay> sprintMemberDays = GetAllSprintMemberDays(x.Date, sprintMembers);
                    return new CalendarItemViewModel(x, sprintMemberDays);
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
    }
}