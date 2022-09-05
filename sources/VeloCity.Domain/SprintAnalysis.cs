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
using DustInTheWind.VeloCity.Domain.DataAccess;

namespace DustInTheWind.VeloCity.Domain
{
    public class SprintAnalysis
    {
        private readonly IUnitOfWork unitOfWork;

        public uint AnalysisLookBack { get; set; }

        public List<int> ExcludedSprints { get; set; }

        public List<string> ExcludedTeamMembers { get; set; }

        public Sprint Sprint { get; private set; }

        public SprintList HistorySprints { get; private set; }

        public Velocity EstimatedVelocity { get; private set; }

        public StoryPoints EstimatedStoryPoints { get; private set; }

        public List<VelocityPenaltyInstance> VelocityPenalties { get; private set; }

        public int? TotalWorkHoursWithVelocityPenalties { get; private set; }

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; private set; }

        public SprintAnalysis(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Analyze(Sprint sprint)
        {
            Sprint = sprint ?? throw new ArgumentNullException(nameof(sprint));

            Sprint.ExcludedTeamMembers = ExcludedTeamMembers;

            HistorySprints = RetrievePreviousSprints();

            Velocity estimatedVelocity = HistorySprints.CalculateAverageVelocity();

            EstimatedVelocity = estimatedVelocity;

            EstimatedStoryPoints = estimatedVelocity.IsEmpty
                ? StoryPoints.Empty
                : Sprint.TotalWorkHours * estimatedVelocity;

            VelocityPenalties = Sprint.GetVelocityPenalties();
            TotalWorkHoursWithVelocityPenalties = Sprint.TotalWorkHoursWithVelocityPenalties;

            EstimatedStoryPointsWithVelocityPenalties = estimatedVelocity.IsEmpty || !VelocityPenalties.Any()
                ? StoryPoints.Empty
                : TotalWorkHoursWithVelocityPenalties * estimatedVelocity;
        }

        private SprintList RetrievePreviousSprints()
        {
            bool excludedSprintsExists = ExcludedSprints is { Count: > 0 };

            List<Sprint> sprints = excludedSprintsExists
                ? unitOfWork.SprintRepository.GetClosedSprintsBefore(Sprint.Number, AnalysisLookBack, ExcludedSprints).ToList()
                : unitOfWork.SprintRepository.GetClosedSprintsBefore(Sprint.Number, AnalysisLookBack).ToList();

            foreach (Sprint sprint in sprints)
                sprint.ExcludedTeamMembers = ExcludedTeamMembers;

            return sprints.ToSprintList();
        }
    }
}