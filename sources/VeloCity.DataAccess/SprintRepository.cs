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
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;

namespace DustInTheWind.VeloCity.DataAccess
{
    internal class SprintRepository : ISprintRepository
    {
        private readonly VeloCityDbContext dbContext;

        public SprintRepository(VeloCityDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<Sprint> GetAll()
        {
            return dbContext.Sprints;
        }

        public Sprint Get(int id)
        {
            return dbContext.Sprints.FirstOrDefault(x => x.Id == id);
        }

        public Sprint GetByNumber(int number)
        {
            return dbContext.Sprints
                .FirstOrDefault(x => x.Number == number);
        }

        public DateInterval? GetDateIntervalFor(int sprintNumber)
        {
            return dbContext.Sprints
                .FirstOrDefault(x => x.Number == sprintNumber)?
                .DateInterval;
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
            return dbContext.Sprints
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();
        }

        public IEnumerable<Sprint> GetLast(int count)
        {
            return dbContext.Sprints
                .Where(x => x.State is SprintState.InProgress or SprintState.Closed)
                .OrderByDescending(x => x.StartDate)
                .Take(count);
        }

        public Sprint GetLastInProgress()
        {
            return dbContext.Sprints
                .Where(x => x.State == SprintState.InProgress)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();
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
    }
}