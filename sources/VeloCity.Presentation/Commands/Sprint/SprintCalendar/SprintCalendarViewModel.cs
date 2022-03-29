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
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.Sprint.SprintCalendar
{
    public class SprintCalendarViewModel
    {
        private readonly AnalyzeSprintResponse response;

        public bool IsVisible => response.SprintDays is { Count: > 0 };

        public List<CalendarItemViewModel> CalendarItems { get; }

        public List<NoteBase> Notes { get; }

        public SprintCalendarViewModel(AnalyzeSprintResponse response)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));

            CalendarItems = CreateCalendarItems();
            Notes = CreateNotes();
        }

        private List<CalendarItemViewModel> CreateCalendarItems()
        {
            List<CalendarItemViewModel> calendarItemViewModels = response.SprintDays
                .Select(CreateCalendarItem)
                .ToList();

            foreach (CalendarItemViewModel calendarItemViewModel in calendarItemViewModels)
                calendarItemViewModel.IsToday = calendarItemViewModel.Date == response.Today;

            return calendarItemViewModels;
        }

        private CalendarItemViewModel CreateCalendarItem(SprintDay sprintDay)
        {
            List<SprintMemberDay> sprintMemberDays = GetAllSprintMemberDays(sprintDay.Date);
            return new CalendarItemViewModel(sprintMemberDays, sprintDay);
        }

        private List<SprintMemberDay> GetAllSprintMemberDays(DateTime date)
        {
            if (response.SprintMembers == null)
                return new List<SprintMemberDay>();

            return response.SprintMembers
                .Select(x => x.Days[date])
                .Where(x => x != null)
                .ToList();
        }

        private List<NoteBase> CreateNotes()
        {
            List<NoteBase> notes = new();

            bool isPartialVacationNoteVisible = CalendarItems
                .SelectMany(x => x.AbsenceDetails.TeamMemberVacationDetails ?? Enumerable.Empty<TeamMemberAbsenceDetailsViewModel>())
                .Any(x => x.IsPartialVacation);

            if (isPartialVacationNoteVisible)
                notes.Add(new PartialDayVacationNote());

            return notes;
        }
    }
}