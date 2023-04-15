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

using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMembers;

namespace DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.Team;

public class TeamMemberViewModel
{
    public int TeamMemberId { get; }

    private TeamMemberInfo TeamMemberInfo { get; }

    public bool IsEmployed => TeamMemberInfo.IsEmployed;

    public TeamMemberViewModel(TeamMemberInfo teamMemberInfo)
    {
        TeamMemberInfo = teamMemberInfo ?? throw new ArgumentNullException(nameof(teamMemberInfo));

        TeamMemberId = teamMemberInfo.Id;
    }

    public override string ToString()
    {
        return TeamMemberInfo.Name;
    }
}