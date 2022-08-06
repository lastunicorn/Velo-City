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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprint;

namespace DustInTheWind.VeloCity.Wpf.Presentation.ViewModels
{
    public class SprintOverviewViewModel
    {
        public DateInterval TimeInterval { get; }

        public SprintState State { get; }

        public int? WorkDaysCount { get; }

        public HoursValue TotalWorkHours { get; }

        public StoryPoints EstimatedStoryPoints { get; }

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; }

        public Velocity EstimatedVelocity { get; }

        public StoryPoints CommitmentStoryPoints { get; }

        public StoryPoints ActualStoryPoints { get; }

        public Velocity ActualVelocity { get; }

        public SprintOverviewViewModel(PresentSprintResponse response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            
            DateTime startDate = response.SprintDateInterval.StartDate!.Value;
            DateTime endDate = response.SprintDateInterval.EndDate!.Value;
            TimeInterval = new DateInterval(startDate, endDate);

            State = response.SprintState;
            WorkDaysCount = response.WorkDaysCount;
            TotalWorkHours = response.TotalWorkHours;
            EstimatedStoryPoints = response.EstimatedStoryPoints;
            EstimatedStoryPointsWithVelocityPenalties = response.EstimatedStoryPointsWithVelocityPenalties;
            EstimatedVelocity = response.EstimatedVelocity;
            CommitmentStoryPoints = response.CommitmentStoryPoints;
            ActualStoryPoints = response.ActualStoryPoints;
            ActualVelocity = response.ActualVelocity;
        }
    }
}