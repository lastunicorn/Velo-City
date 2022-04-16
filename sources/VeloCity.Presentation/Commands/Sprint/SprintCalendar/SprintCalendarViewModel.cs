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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.Sprint.SprintCalendar
{
    public class SprintCalendarViewModel
    {
        private DateTime? today;

        public string Title { get; set; } = "Sprint Calendar";

        public bool IsVisible => CalendarItems is { Count: > 0 };

        public List<CalendarItemViewModel> CalendarItems { get; }

        public List<NoteBase> Notes { get; }

        public bool ContainsHighlightedItems => CalendarItems.Any(x => x.IsHighlighted);

        public DateTime? Today
        {
            get => today;
            set
            {
                today = value;

                if (today == null)
                {
                    foreach (CalendarItemViewModel calendarItemViewModel in CalendarItems)
                        calendarItemViewModel.IsHighlighted = false;
                }
                else
                {
                    foreach (CalendarItemViewModel calendarItemViewModel in CalendarItems)
                        calendarItemViewModel.IsHighlighted = calendarItemViewModel.Date == today.Value;
                }
            }
        }

        public SprintCalendarViewModel(List<SprintDay> sprintDays, IReadOnlyCollection<SprintMember> sprintMembers)
        {
            if (sprintDays == null) throw new ArgumentNullException(nameof(sprintDays));

            CalendarItems = CreateCalendarItems(sprintDays, sprintMembers);
            Notes = CreateNotes();
        }

        private static List<CalendarItemViewModel> CreateCalendarItems(IEnumerable<SprintDay> sprintDays, IReadOnlyCollection<SprintMember> sprintMembers)
        {
            return sprintDays
                .Select(x =>
                {
                    List<SprintMemberDay> sprintMemberDays = GetAllSprintMemberDays(x.Date, sprintMembers);
                    return new CalendarItemViewModel(x, sprintMemberDays);
                })
                .ToList();
        }

        private static List<SprintMemberDay> GetAllSprintMemberDays(DateTime date, IReadOnlyCollection<SprintMember> sprintMembers)
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

            IEnumerable<TeamMemberAbsenceDetailsViewModel> memberAbsenceDetailsViewModels = CalendarItems
                .SelectMany(x => x.AbsenceDetails.TeamMemberVacationDetails ?? Enumerable.Empty<TeamMemberAbsenceDetailsViewModel>());

            bool isPartialVacationNoteVisible = false;
            bool isMissingByContractNoteVisible = false;

            foreach (TeamMemberAbsenceDetailsViewModel teamMemberAbsenceDetailsViewModel in memberAbsenceDetailsViewModels)
            {
                if (teamMemberAbsenceDetailsViewModel.IsPartialVacation)
                    isPartialVacationNoteVisible = true;

                if (teamMemberAbsenceDetailsViewModel.IsMissingByContract)
                    isMissingByContractNoteVisible = true;
            }

            if (isPartialVacationNoteVisible)
                notes.Add(new PartialDayVacationNote());

            if (isMissingByContractNoteVisible)
                notes.Add(new MissingMyContractNote());

            return notes;
        }
    }
}