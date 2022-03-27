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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Application.AnalyzeSprint
{
    public class AnalyzeSprintResponse
    {
        public string SprintName { get; set; }

        public SprintState SprintState { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<SprintDay> SprintDays { get; set; }

        public List<SprintMember> SprintMembers { get; set; }

        public int TotalWorkHours { get; set; }

        public StoryPoints EstimatedStoryPoints { get; set; }

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; set; }

        public Velocity EstimatedVelocity { get; set; }

        public List<VelocityPenaltyInfo> VelocityPenalties { get; set; }

        public StoryPoints CommitmentStoryPoints { get; set; }

        public StoryPoints ActualStoryPoints { get; set; }

        public Velocity ActualVelocity { get; set; }

        public int LookBackSprintCount { get; set; }

        public List<int> PreviousSprints { get; set; }

        public List<int> ExcludedSprints { get; set; }

        public bool ShowTeam { get; set; }
    }
}