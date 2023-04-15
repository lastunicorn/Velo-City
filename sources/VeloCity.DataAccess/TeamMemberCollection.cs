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

using System.Data;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.DataAccess;

public class TeamMemberCollection : EntityCollection<TeamMember, int>
{
    public TeamMemberCollection()
    {
    }

    public TeamMemberCollection(IEnumerable<TeamMember> teamMembers)
        : base(teamMembers.ToList())
    {
    }

    protected override int GetId(TeamMember teamMembers)
    {
        return teamMembers.Id;
    }

    protected override bool HasId(TeamMember teamMembers)
    {
        return teamMembers.Id != 0;
    }

    protected override void SetId(TeamMember teamMembers, int id)
    {
        teamMembers.Id = id;
    }

    protected override int GenerateNewId()
    {
        IEnumerable<TeamMember> teamMembersOrderedById = Items.OrderBy(x => x.Id);
        IEnumerable<int> possibleIds = Enumerable.Range(1, int.MaxValue);

        using IEnumerator<TeamMember> teamMembersEnumerator = teamMembersOrderedById.GetEnumerator();
        using IEnumerator<int> possibleIdEnumerator = possibleIds.GetEnumerator();

        while (possibleIdEnumerator.MoveNext())
        {
            bool sprintExists = teamMembersEnumerator.MoveNext();

            if (!sprintExists)
                return possibleIdEnumerator.Current;

            if (possibleIdEnumerator.Current != teamMembersEnumerator.Current.Id)
                return possibleIdEnumerator.Current;
        }

        throw new DataException("Cannot generate new id. Collection is full.");
    }
}