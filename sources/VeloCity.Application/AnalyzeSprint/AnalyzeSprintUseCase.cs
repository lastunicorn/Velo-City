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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.Configuring;
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Application.AnalyzeSprint
{
    internal class AnalyzeSprintUseCase : IRequestHandler<AnalyzeSprintRequest, AnalyzeSprintResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfig config;
        private readonly ISystemClock systemClock;

        public AnalyzeSprintUseCase(IUnitOfWork unitOfWork, IConfig config, ISystemClock systemClock)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        }

        public Task<AnalyzeSprintResponse> Handle(AnalyzeSprintRequest request, CancellationToken cancellationToken)
        {
            Sprint currentSprint = RetrieveCurrentSprint(request);

            uint analysisLookBack = request.AnalysisLookBack ?? config.AnalysisLookBack;
            SprintList historySprints = RetrievePreviousSprints(currentSprint.Number, analysisLookBack, request.ExcludedSprints, request.ExcludedTeamMembers);

            SprintAnalysis sprintAnalysis = new()
            {
                Sprint = currentSprint,
                HistorySprints = historySprints
            };
            sprintAnalysis.Calculate();

            AnalyzeSprintResponse response = new()
            {
                SprintName = currentSprint.Name,
                SprintState = currentSprint.State,
                StartDate = currentSprint.StartDate,
                EndDate = currentSprint.EndDate,
                SprintDays = currentSprint.EnumerateAllDays().ToList(),
                WorkDaysCount = currentSprint.CountWorkDays(),
                SprintMembers = currentSprint.SprintMembersOrderedByEmployment.ToList(),
                TotalWorkHours = currentSprint.TotalWorkHours,
                EstimatedStoryPoints = sprintAnalysis.EstimatedStoryPoints,
                EstimatedStoryPointsWithVelocityPenalties = sprintAnalysis.EstimatedStoryPointsWithVelocityPenalties,
                EstimatedVelocity = sprintAnalysis.EstimatedVelocity,
                VelocityPenalties = sprintAnalysis.VelocityPenalties
                    .Select(x => new VelocityPenaltyInfo(x))
                    .ToList(),
                CommitmentStoryPoints = currentSprint.CommitmentStoryPoints,
                ActualStoryPoints = currentSprint.ActualStoryPoints,
                ActualVelocity = currentSprint.Velocity,
                LookBackSprintCount = analysisLookBack,
                PreviousSprints = historySprints
                    .Select(x => x.Number)
                    .ToList(),
                ExcludedSprints = request.ExcludedSprints?.ToList(),
                ShowTeam = request.ShowTeam,
                CurrentDay = systemClock.Today
            };

            return Task.FromResult(response);
        }

        private Sprint RetrieveCurrentSprint(AnalyzeSprintRequest request)
        {
            return request.SprintNumber == null
                ? RetrieveDefaultSprintToAnalyze(request.ExcludedTeamMembers)
                : RetrieveSpecificSprintToAnalyze(request.SprintNumber.Value, request.ExcludedTeamMembers);
        }

        private Sprint RetrieveDefaultSprintToAnalyze(IReadOnlyCollection<string> excludedTeamMembers)
        {
            Sprint sprint = unitOfWork.SprintRepository.GetLastInProgress();

            if (sprint == null)
                sprint = unitOfWork.SprintRepository.GetLast();

            if (sprint == null)
                throw new NoSprintException();

            return sprint;
        }

        private Sprint RetrieveSpecificSprintToAnalyze(int sprintNumber, IReadOnlyCollection<string> excludedTeamMembers)
        {
            Sprint sprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            if (sprint == null)
                throw new SprintDoesNotExistException(sprintNumber);

            sprint.ExcludedTeamMembers = excludedTeamMembers;

            return sprint;
        }

        private SprintList RetrievePreviousSprints(int sprintNumber, uint count, List<int> excludedSprints, IReadOnlyCollection<string> excludedTeamMembers)
        {
            bool excludedSprintsExists = excludedSprints is { Count: > 0 };

            List<Sprint> sprints = excludedSprintsExists
                ? unitOfWork.SprintRepository.GetClosedSprintsBefore(sprintNumber, count, excludedSprints).ToList()
                : unitOfWork.SprintRepository.GetClosedSprintsBefore(sprintNumber, count).ToList();

            foreach (Sprint sprint in sprints)
                sprint.ExcludedTeamMembers = excludedTeamMembers;

            return sprints.ToSprintList();
        }
    }
}