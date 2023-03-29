﻿// VeloCity
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
using System.Linq;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.DataAccess;

internal class SprintRepository : ISprintRepository
{
    private readonly VeloCityDbContext dbContext;

    public SprintRepository(VeloCityDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IEnumerable<Sprint> GetAll()
    {
        return dbContext.Sprints;
    }

    public Task<Sprint> Get(int id)
    {
        Sprint sprint = dbContext.Sprints.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(sprint);
    }

    public Task<Sprint> GetByNumber(int number)
    {
        Sprint sprint = dbContext.Sprints
            .FirstOrDefault(x => x.Number == number);

        return Task.FromResult(sprint);
    }

    public DateInterval? GetDateIntervalFor(int sprintNumber)
    {
        return dbContext.Sprints
            .FirstOrDefault(x => x.Number == sprintNumber)?
            .DateInterval;
    }

    public bool IsAnyInProgress()
    {
        return dbContext.Sprints.Any(x => x.State == SprintState.InProgress);
    }

    public bool IsFirstNewSprint(int sprintId)
    {
        int id = dbContext.Sprints
            .Where(x => x.State == SprintState.New)
            .OrderBy(x => x.StartDate)
            .Select(x => x.Id)
            .FirstOrDefault();

        return id == sprintId;
    }

    public IEnumerable<Sprint> GetClosedSprintsBefore(int sprintNumber, uint count)
    {
        return dbContext.Sprints
            .OrderByDescending(x => x.StartDate)
            .SkipWhile(x => x.Number != sprintNumber)
            .Skip(1)
            .Where(x => x.State == SprintState.Closed)
            .Take((int)count);
    }

    public IEnumerable<Sprint> GetClosedSprintsBefore(int sprintNumber, uint count, IEnumerable<int> excludedSprints)
    {
        List<int> excludedSprintsList = excludedSprints.ToList();

        return dbContext.Sprints
            .Where(x => !excludedSprintsList.Contains(x.Number))
            .OrderByDescending(x => x.StartDate)
            .SkipWhile(x => x.Number != sprintNumber)
            .Skip(1)
            .Where(x => x.State == SprintState.Closed)
            .Take((int)count);
    }

    public Sprint GetLast()
    {
        return dbContext.Sprints.MaxBy(x => x.StartDate);
    }

    public IEnumerable<Sprint> GetLast(int count)
    {
        return dbContext.Sprints
            .Where(x => x.State is SprintState.InProgress or SprintState.Closed)
            .OrderByDescending(x => x.StartDate)
            .Take(count);
    }

    public Task<Sprint> GetLastInProgress()
    {
        Sprint sprint = dbContext.Sprints
            .Where(x => x.State == SprintState.InProgress)
            .MaxBy(x => x.StartDate);

        return Task.FromResult(sprint);
    }

    public IEnumerable<Sprint> GetLastClosed(uint count, IEnumerable<int> excludedSprints)
    {
        List<int> excludedSprintsList = excludedSprints.ToList();

        return dbContext.Sprints
            .Where(x => !excludedSprintsList.Contains(x.Number))
            .OrderByDescending(x => x.StartDate)
            .Where(x => x.State == SprintState.Closed)
            .Take((int)count);
    }

    public IEnumerable<Sprint> GetLastClosed(uint count)
    {
        return dbContext.Sprints
            .OrderByDescending(x => x.StartDate)
            .Where(x => x.State == SprintState.Closed)
            .Take((int)count);
    }

    public Sprint GetLastClosed()
    {
        return dbContext.Sprints
            .OrderByDescending(x => x.StartDate)
            .FirstOrDefault(x => x.State == SprintState.Closed);
    }

    public IEnumerable<Sprint> Get(DateTime startDate, DateTime endDate)
    {
        return dbContext.Sprints
            .OrderByDescending(x => x.StartDate)
            .Where(x => x.EndDate >= startDate && x.StartDate <= endDate);
    }

    public void Add(Sprint sprint)
    {
        if (sprint == null) throw new ArgumentNullException(nameof(sprint));

        if (sprint.Id == 0)
            sprint.Id = CreateNewId();

        dbContext.Sprints.Add(sprint);
    }

    private int CreateNewId()
    {
        Sprint sprintWithBiggestId = dbContext.Sprints.MaxBy(x => x.Id);

        return sprintWithBiggestId == null
            ? 1
            : sprintWithBiggestId.Id + 1;
    }
}