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

namespace DustInTheWind.VeloCity.Domain
{
    public class Forecast
    {
        public List<Sprint> HistorySprints { get; set; }

        public List<Sprint> FutureSprints { get; set; }

        public int TotalWorkHours { get; private set; }

        public Velocity EstimatedVelocity { get; private set; }

        public StoryPoints EstimatedStoryPoints { get; private set; }

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; private set; }

        public List<SprintForecast> ForecastSprints { get; private set; }

        public void Calculate()
        {
            if (HistorySprints == null || HistorySprints.Count == 0)
                throw new Exception("History sprints were not provided. They are needed to calculate the estimated velocity.");

            SprintList historySprints = HistorySprints.ToSprintList();
            EstimatedVelocity = historySprints.CalculateAverageVelocity();

            if (EstimatedVelocity.IsNull)
                throw new Exception("Error calculating the estimated velocity.");

            ForecastSprints = FutureSprints
                .Select(x => ToSprintForecast(x, EstimatedVelocity))
                .ToList();

            TotalWorkHours = FutureSprints.Sum(x => x.CalculateTotalWorkHours());

            EstimatedStoryPoints = TotalWorkHours * EstimatedVelocity;

            bool velocityPenaltiesExists = FutureSprints
                .SelectMany(x => x.GetVelocityPenalties())
                .Any();

            int? totalWorkHoursWithVelocityPenalties = FutureSprints
                .Sum(x => x.CalculateTotalWorkHoursWithVelocityPenalties());

            EstimatedStoryPointsWithVelocityPenalties = velocityPenaltiesExists
                ? totalWorkHoursWithVelocityPenalties * EstimatedVelocity
                : StoryPoints.Null;
        }

        private static SprintForecast ToSprintForecast(Sprint sprint, Velocity estimatedVelocity)
        {
            HoursValue totalWorkHours = sprint.CalculateTotalWorkHours();

            StoryPoints estimatedStoryPoints = estimatedVelocity.IsNull
                ? StoryPoints.Null
                : totalWorkHours * estimatedVelocity;

            bool velocityPenaltiesExists = sprint.GetVelocityPenalties().Any();
            int? totalWorkHoursWithVelocityPenalties = sprint.CalculateTotalWorkHoursWithVelocityPenalties();

            StoryPoints estimatedStoryPointsWithVelocityPenalties = estimatedVelocity.IsNull || !velocityPenaltiesExists
                ? StoryPoints.Null
                : totalWorkHoursWithVelocityPenalties * estimatedVelocity;

            return new SprintForecast
            {
                Number = sprint.Number,
                IsRealSprint = sprint.Number > 0,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                TotalWorkHours = totalWorkHours,
                EstimatedStoryPoints = estimatedStoryPoints,
                EstimatedStoryPointsWithVelocityPenalties = estimatedStoryPointsWithVelocityPenalties
            };
        }
    }
}