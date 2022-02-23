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

using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.JsonFiles;

namespace DustInTheWind.VeloCity.DataAccess
{
    internal static class SprintExtensions
    {
        public static IEnumerable<JSprint> ToJEntities(this IEnumerable<Sprint> sprints)
        {
            return sprints
                .Select(x => x.ToJEntity())
                .ToList();
        }

        public static JSprint ToJEntity(this Sprint sprint)
        {
            return new JSprint
            {
                Id = sprint.Id,
                Name = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                CommitmentStoryPoints = sprint.CommitmentStoryPoints,
                ActualStoryPoints = sprint.ActualStoryPoints,
            };
        }

        public static IEnumerable<Sprint> ToEntities(this IEnumerable<JSprint> sprints)
        {
            return sprints
                .Select(x => x.ToEntity());
        }

        public static Sprint ToEntity(this JSprint sprint)
        {
            return new Sprint
            {
                Id = sprint.Id,
                Name = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                CommitmentStoryPoints = sprint.CommitmentStoryPoints,
                ActualStoryPoints = sprint.ActualStoryPoints,
            };
        }
    }
}