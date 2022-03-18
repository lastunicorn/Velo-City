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
        private readonly Database database;

        public SprintRepository(Database database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Sprint Get(int id)
        {
            return database.Sprints.FirstOrDefault(x => x.Id == id);
        }

        public Sprint GetByNumber(int number)
        {
            return database.Sprints.FirstOrDefault(x => x.Number == number);
        }

        public IEnumerable<Sprint> GetClosedSprintsBefore(int sprintNumber, int count)
        {
            return database.Sprints
                .OrderByDescending(x => x.StartDate)
                .SkipWhile(x => x.Number != sprintNumber)
                .Skip(1)
                .Where(x => x.State == SprintState.Closed)
                .Take(count);
        }

        public IEnumerable<Sprint> GetClosedSprintsBefore(int sprintNumber, int count, IEnumerable<int> excludedSprints)
        {
            List<int> excludedSprintsList = excludedSprints.ToList();

            return database.Sprints
                .Where(x => !excludedSprintsList.Contains(x.Number))
                .OrderByDescending(x => x.StartDate)
                .SkipWhile(x => x.Number != sprintNumber)
                .Skip(1)
                .Where(x => x.State == SprintState.Closed)
                .Take(count);
        }

        public IEnumerable<Sprint> GetPage(int pageIndex, int count)
        {
            return database.Sprints
                .OrderByDescending(x => x.StartDate)
                .Skip(pageIndex * count)
                .Take(count);
        }

        public Sprint GetLast()
        {
            return database.Sprints
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();
        }

        public Sprint GetLastInProgress()
        {
            return database.Sprints
                .Where(x => x.State == SprintState.InProgress)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();
        }
    }
}