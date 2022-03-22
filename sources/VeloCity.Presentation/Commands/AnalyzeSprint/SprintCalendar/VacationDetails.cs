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

using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint.SprintCalendar
{
    public class VacationDetails
    {
        public List<TeamMemberVacationDetails> TeamMembers { get; set; }

        public AbsenceReason AllTeamAbsenceReason { get; set; }

        public override string ToString()
        {
            if (TeamMembers is { Count: > 0 })
            {
                List<string> absentTeamMemberNames = TeamMembers
                    .Select(x => x.IsPartialVacation
                        ? x.Name + "(*)"
                        : x.Name)
                    .ToList();

                return string.Join(", ", absentTeamMemberNames);
            }

            if (AllTeamAbsenceReason == AbsenceReason.OfficialHoliday)
                return "Official Holiday";

            return null;
        }
    }
}