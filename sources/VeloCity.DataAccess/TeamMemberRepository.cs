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
    internal class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly VeloCityDbContext dbContext;

        public TeamMemberRepository(VeloCityDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public TeamMember Get(int id)
        {
            return dbContext.TeamMembers
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TeamMember> GetAll()
        {
            return dbContext.TeamMembers;
        }

        public IEnumerable<TeamMember> GetByDate(DateTime date)
        {
            return dbContext.TeamMembers
                .Where(x => x.Employments?.Any(e => e.ContainsDate(date)) ?? false);
        }

        public IEnumerable<TeamMember> GetByDateInterval(DateInterval dateInterval, IReadOnlyCollection<string> excludedNames = null)
        {
            IEnumerable<TeamMember> teamMembers = dbContext.TeamMembers
                .Where(x => x.Employments?.Any(e => e.TimeInterval.IsIntersecting(dateInterval)) ?? false);

            if (excludedNames is { Count: > 0 })
                teamMembers = teamMembers.Where(x => !excludedNames.Any(z => x.Name.Contains(z)));

            return teamMembers;
        }

        public IEnumerable<TeamMember> Find(string text)
        {
            return dbContext.TeamMembers
                .Where(x => x.Name.Contains(text));
        }
    }
}