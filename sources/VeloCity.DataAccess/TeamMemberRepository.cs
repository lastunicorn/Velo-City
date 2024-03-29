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
using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.DataAccess;

internal class TeamMemberRepository : ITeamMemberRepository
{
    private readonly VeloCityDbContext dbContext;

    public TeamMemberRepository(VeloCityDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<TeamMember> Get(int id)
    {
        TeamMember teamMember = dbContext.TeamMembers
            .FirstOrDefault(x => x.Id == id);

        return Task.FromResult(teamMember);
    }

    public Task<IEnumerable<TeamMember>> GetAll()
    {
        IEnumerable<TeamMember> teamMembers = dbContext.TeamMembers;

        return Task.FromResult(teamMembers);
    }

    public Task<IEnumerable<TeamMember>> GetByDate(DateTime date)
    {
        IEnumerable<TeamMember> teamMembers = dbContext.TeamMembers
            .Where(x => x.Employments.Any(e => e.ContainsDate(date)));

        return Task.FromResult(teamMembers);
    }

    public Task<IEnumerable<TeamMember>> GetByDateInterval(DateInterval dateInterval, IReadOnlyCollection<string> excludedNames = null)
    {
        IEnumerable<TeamMember> teamMembers = dbContext.TeamMembers
            .Where(x => x.Employments.Any(e => e.TimeInterval.IsIntersecting(dateInterval)));

        if (excludedNames is { Count: > 0 })
            teamMembers = teamMembers.Where(x => !excludedNames.Any(z => x.Name.Contains(z)));

        return Task.FromResult(teamMembers);
    }

    public Task<IEnumerable<TeamMember>> Find(string text)
    {
        IEnumerable<TeamMember> teamMembers = dbContext.TeamMembers
            .Where(x => x.Name.Contains(text));

        return Task.FromResult(teamMembers);
    }

    public Task Add(TeamMember teamMember)
    {
        if (teamMember == null) throw new ArgumentNullException(nameof(teamMember));

        if (teamMember.Id == 0)
            teamMember.Id = CreateNewId();

        dbContext.TeamMembers.Add(teamMember);

        return Task.CompletedTask;
    }

    private int CreateNewId()
    {
        TeamMember teamMemberWithBiggestId = dbContext.TeamMembers.MaxBy(x => x.Id);

        return teamMemberWithBiggestId == null
            ? 1
            : teamMemberWithBiggestId.Id + 1;
    }
}