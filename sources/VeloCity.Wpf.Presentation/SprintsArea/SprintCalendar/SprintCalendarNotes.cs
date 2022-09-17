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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintCalendar
{
    public class SprintCalendarNotes : IEnumerable<NoteBase>
    {
        private readonly List<NoteBase> notes = new();

        private List<SprintCalendarItemViewModel> calendarItems;

        public List<SprintCalendarItemViewModel> CalendarItems
        {
            get => calendarItems;
            set
            {
                calendarItems = value;
                UpdateNotes();
            }
        }

        private void UpdateNotes()
        {
            notes.Clear();

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
        }

        public IEnumerator<NoteBase> GetEnumerator()
        {
            IEnumerable<NoteBase> list = notes ?? Enumerable.Empty<NoteBase>();
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}