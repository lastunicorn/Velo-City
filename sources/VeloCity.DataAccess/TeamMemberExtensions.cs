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
using VeloCity.DataAccess.Jsonfiles;

namespace DustInTheWind.VeloCity.DataAccess
{
    internal static class TeamMemberExtensions
    {
        public static IEnumerable<JTeamMember> ToJEntities(this IEnumerable<TeamMember> teamMembers)
        {
            return teamMembers
                .Select(x => x.ToJEntity());
        }

        public static JTeamMember ToJEntity(this TeamMember teamMember)
        {
            return new JTeamMember
            {
                Id = teamMember.Id,
                Name = teamMember.Name,
                HoursPerDay = teamMember.HoursPerDay,
                VacationDays = teamMember.VacationDays
                    .ToJEntities()
                    .ToList()
            };
        }

        public static IEnumerable<TeamMember> ToEntities(this IEnumerable<JTeamMember> teamMembers)
        {
            return teamMembers
                .Select(x => x.ToEntity());
        }

        public static TeamMember ToEntity(this JTeamMember teamMember)
        {
            return new TeamMember
            {
                Id = teamMember.Id,
                Name = teamMember.Name,
                HoursPerDay = teamMember.HoursPerDay,
                VacationDays = teamMember.VacationDays
                    .ToEntities()
                    .ToList()
            };
        }
    }
}