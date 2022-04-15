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
using System.Collections.ObjectModel;
using System.Linq;

namespace DustInTheWind.VeloCity.Domain
{
    public class SprintList : Collection<Sprint>
    {
        public Sprint Last => Items
            .OrderByDescending(x => x.StartDate)
            .FirstOrDefault();

        public SprintList(IEnumerable<Sprint> sprints)
        {
            foreach (Sprint sprint in sprints)
                Items.Add(sprint);
        }

        public Velocity CalculateAverageVelocity()
        {
            IEnumerable<Velocity> previousVelocities = Items
                .Select(x => x.CalculateVelocity());

            if (!previousVelocities.Any())
                return Velocity.Null;

            return previousVelocities
                .Select(x => x.Value)
                .Average();
        }
    }
}