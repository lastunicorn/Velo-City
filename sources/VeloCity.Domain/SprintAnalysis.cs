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

namespace DustInTheWind.VeloCity.Domain
{
    public class SprintAnalysis
    {
        public Sprint Sprint { get; set; }

        public SprintList HistorySprints { get; set; }

        public Velocity EstimatedVelocity { get; private set; }

        public StoryPoints EstimatedStoryPoints { get; private set; }

        public List<VelocityPenaltyInstance> VelocityPenalties { get; private set; }

        public int? TotalWorkHoursWithVelocityPenalties { get; private set; }

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; private set; }

        public void Calculate()
        {
            Velocity estimatedVelocity = HistorySprints.CalculateAverageVelocity();

            EstimatedVelocity = estimatedVelocity;

            EstimatedStoryPoints = estimatedVelocity.IsNull
                ? StoryPoints.Null
                : Sprint.TotalWorkHours * estimatedVelocity;

            VelocityPenalties = Sprint.GetVelocityPenalties();
            TotalWorkHoursWithVelocityPenalties = Sprint.TotalWorkHoursWithVelocityPenalties;

            EstimatedStoryPointsWithVelocityPenalties = estimatedVelocity.IsNull || !VelocityPenalties.Any()
                ? StoryPoints.Null
                : TotalWorkHoursWithVelocityPenalties * estimatedVelocity;
        }
    }
}