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

using System.Collections.ObjectModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentTeamMembers;

internal class TeamMemberList : Collection<TeamMember>
{
    public TeamMemberList(IEnumerable<TeamMember> teamMembers)
        : base(teamMembers?.ToList() ?? new List<TeamMember>())
    {
    }

    public void OrderEmployedFirst()
    {
        List<TeamMember> employedTeamMembers = new();
        List<TeamMember> unemployedTeamMembers = new();

        foreach (TeamMember teamMember in Items)
        {
            if (teamMember.HasActiveEmployment)
                employedTeamMembers.Add(teamMember);
            else
                unemployedTeamMembers.Add(teamMember);
        }

        Items.Clear();

        IEnumerable<TeamMember> orderedEmployedTeamMembers = employedTeamMembers
            .OrderByEmployment();

        IEnumerable<TeamMember> orderedUnemployedTeamMembers = unemployedTeamMembers
            .OrderByDescending(x => x.Employments?.GetLastEmployment().EndDate ?? DateTime.MinValue);

        IEnumerable<TeamMember> allTeamMembers = orderedEmployedTeamMembers
            .Concat(orderedUnemployedTeamMembers);

        foreach (TeamMember teamMember in allTeamMembers)
            Items.Add(teamMember);
    }
}