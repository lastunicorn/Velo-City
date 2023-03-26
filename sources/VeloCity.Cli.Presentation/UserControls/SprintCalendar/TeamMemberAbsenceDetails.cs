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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Cli.Presentation.UserControls.SprintCalendar
{
    public class TeamMemberAbsenceDetails
    {
        private readonly SprintMemberDay sprintMemberDay;

        public bool IsPartialVacation { get; }

        public bool IsMissingByContract { get; }

        public TeamMemberAbsenceDetails(SprintMemberDay sprintMemberDay)
        {
            this.sprintMemberDay = sprintMemberDay ?? throw new ArgumentNullException(nameof(sprintMemberDay));

            IsPartialVacation = sprintMemberDay.WorkHours > 0;
            IsMissingByContract = sprintMemberDay.AbsenceReason == AbsenceReason.Contract;
        }

        public override string ToString()
        {
            List<char> notes = new(2);
            if (IsMissingByContract)
                notes.Add('c');

            if (IsPartialVacation)
                notes.Add('*');

            PersonName name = sprintMemberDay.TeamMember.Name;
            string shortName = name.ShortName;

            if (notes.Count <= 0)
                return shortName;

            string notesAsString = string.Join(string.Empty, notes);
            return $"{shortName} ({notesAsString})";
        }
    }
}