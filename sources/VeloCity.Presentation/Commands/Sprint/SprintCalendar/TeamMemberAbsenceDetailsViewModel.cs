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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.Sprint.SprintCalendar
{
    public class TeamMemberAbsenceDetailsViewModel
    {
        private readonly SprintMemberDay sprintMemberDay;

        public bool IsPartialVacation { get; }

        public TeamMemberAbsenceDetailsViewModel(SprintMemberDay sprintMemberDay)
        {
            this.sprintMemberDay = sprintMemberDay ?? throw new ArgumentNullException(nameof(sprintMemberDay));

            IsPartialVacation = sprintMemberDay.WorkHours > 0;
        }

        public override string ToString()
        {
            PersonName name = sprintMemberDay.TeamMember.Name;

            return IsPartialVacation
                ? name.ShortName + " (*)"
                : name.ShortName;
        }
    }
}