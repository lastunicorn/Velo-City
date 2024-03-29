﻿// VeloCity
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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.JsonFiles.JsonFileModel;

namespace DustInTheWind.VeloCity.DataAccess;

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
            FirstName = teamMember.Name.FirstName,
            MiddleName = teamMember.Name.MiddleName,
            LastName = teamMember.Name.LastName,
            Nickname = teamMember.Name.Nickname,
            Employments = teamMember.Employments
                .ToJEntities()
                .ToList(),
            Comments = teamMember.Comments,
            VacationDays = teamMember.Vacations?
                .ToJEntities()
                .ToList(),
            VelocityPenalties = teamMember.VelocityPenalties?
                .ToJEntities()
                .ToList()
        };
    }

    public static IEnumerable<TeamMember> ToEntities(this IEnumerable<JTeamMember> teamMembers, VeloCityDbContext dbContext)
    {
        if(teamMembers == null)
            return Enumerable.Empty<TeamMember>();

        return teamMembers
            .Select(x => x.ToEntity(dbContext));
    }

    public static TeamMember ToEntity(this JTeamMember teamMember, VeloCityDbContext dbContext)
    {
        TeamMember entity = new()
        {
            Id = teamMember.Id,
            Name = GetPersonName(teamMember),
            Comments = teamMember.Comments,
            Vacations = new VacationCollection(teamMember.VacationDays?.ToEntities()),
            VelocityPenalties = teamMember.VelocityPenalties?
                .ToEntities(dbContext)
                .ToList()
        };

        entity.Employments.AddRange(teamMember.Employments?.ToEntities());

        return entity;
    }

    private static PersonName GetPersonName(JTeamMember teamMember)
    {
        bool hasDetailedName = teamMember.FirstName != null ||
                               teamMember.MiddleName != null ||
                               teamMember.LastName != null ||
                               teamMember.Nickname != null;

        if (!hasDetailedName)
            return PersonName.Parse(teamMember.Name);

        return new PersonName
        {
            FirstName = teamMember.FirstName,
            MiddleName = teamMember.MiddleName,
            LastName = teamMember.LastName,
            Nickname = teamMember.Nickname
        };
    }

    public static TeamMemberCollection ToTeamMemberCollection(this IEnumerable<TeamMember> teamMembers)
    {
        return new TeamMemberCollection(teamMembers);
    }
}