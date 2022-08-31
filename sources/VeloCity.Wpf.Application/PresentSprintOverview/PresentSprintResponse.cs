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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintOverview
{
    public class PresentSprintOverviewResponse
    {
        public SprintState SprintState { get; set; }

        public DateInterval SprintDateInterval { get; set; }
        
        public string SprintDescription { get; set; }

        public int WorkDaysCount { get; set; }

        public HoursValue TotalWorkHours { get; set; }

        public StoryPoints EstimatedStoryPoints { get; set; }

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; set; }

        public Velocity EstimatedVelocity { get; set; }

        public List<VelocityPenaltyInfo> VelocityPenalties { get; set; }

        public StoryPoints CommitmentStoryPoints { get; set; }

        public StoryPoints ActualStoryPoints { get; set; }

        public Velocity ActualVelocity { get; set; }

        public List<int> PreviouslyClosedSprints { get; set; }

        public List<int> ExcludedSprints { get; set; }

        public string SprintComments { get; set; }
    }
}