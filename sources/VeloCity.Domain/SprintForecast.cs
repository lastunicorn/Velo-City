// VeloCity
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
    public class SprintForecast
    {
        public string SprintName { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public List<SprintDay> Days { get; }

        public int WorkDaysCount { get; }

        public HoursValue TotalWorkHours { get; }

        public StoryPoints EstimatedStoryPoints { get; }

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; }

        public SprintForecast(Sprint sprint, Velocity estimatedVelocity)
        {
            StoryPoints estimatedStoryPoints = estimatedVelocity.IsEmpty
                ? StoryPoints.Empty
                : sprint.TotalWorkHours * estimatedVelocity;

            bool velocityPenaltiesExists = sprint.GetVelocityPenalties().Any();
            HoursValue totalWorkHoursWithVelocityPenalties = sprint.TotalWorkHoursWithVelocityPenalties;

            StoryPoints estimatedStoryPointsWithVelocityPenalties = estimatedVelocity.IsEmpty || !velocityPenaltiesExists
                ? StoryPoints.Empty
                : totalWorkHoursWithVelocityPenalties * estimatedVelocity;

            SprintName = sprint.Title;
            StartDate = sprint.StartDate;
            EndDate = sprint.EndDate;
            Days = sprint.EnumerateAllDays().ToList();
            WorkDaysCount = sprint.CountWorkDays();
            TotalWorkHours = sprint.TotalWorkHours;
            EstimatedStoryPoints = estimatedStoryPoints;
            EstimatedStoryPointsWithVelocityPenalties = estimatedStoryPointsWithVelocityPenalties;
        }
    }
}