// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public static class TeamMembersExtensions
{
    public static IEnumerable<TeamMember> OrderByEmploymentForDate(this IEnumerable<TeamMember> teamMembers, DateTime? date = null)
    {
        return teamMembers
            .OrderBy(x =>
            {
                Employment employment = date == null
                    ? x.Employments.GetFirstEmployment()
                    : x.Employments.GetEmploymentBatchFor(date.Value).LastOrDefault();

                return employment?.TimeInterval.StartDate;
            })
            .ThenBy(x => x.Name);
    }

    public static IEnumerable<TeamMember> OrderByEmploymentForDate(this IEnumerable<TeamMember> teamMembers, DateTime date)
    {
        return teamMembers
            .OrderBy(x =>
            {
                Employment employment = x.Employments.GetEmploymentBatchFor(date).LastOrDefault();
                return employment?.TimeInterval.StartDate;
            })
            .ThenBy(x => x.Name);
    }

    public static IEnumerable<TeamMember> OrderByEmployment(this IEnumerable<TeamMember> teamMembers)
    {
        return teamMembers
            .OrderBy(x => x.Employments?.GetLastEmploymentBatch()?.StartDate)
            .ThenBy(x => x.Name);
    }
}