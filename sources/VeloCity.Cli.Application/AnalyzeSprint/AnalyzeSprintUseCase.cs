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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.Configuring;
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Application.AnalyzeSprint
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
            Sprint currentSprint = RetrieveSprintToAnalyze(request);

            SprintAnalysis sprintAnalysis = new(unitOfWork)
            {
                ExcludedSprints = request.ExcludedSprints,
                ExcludedTeamMembers = request.ExcludedTeamMembers,
                AnalysisLookBack = request.AnalysisLookBack ?? config.AnalysisLookBack
            };
            sprintAnalysis.Analyze(currentSprint);

            AnalyzeSprintResponse response = CreateResponse(sprintAnalysis);

            return Task.FromResult(response);
        }

        private Sprint RetrieveSprintToAnalyze(AnalyzeSprintRequest request)
        {
            Sprint sprint = request.SprintNumber == null
                ? RetrieveDefaultSprintToAnalyze()
                : RetrieveSpecificSprintToAnalyze(request.SprintNumber.Value);

            sprint.ExcludedTeamMembers = request.ExcludedTeamMembers;

            return sprint;
        }

        private Sprint RetrieveDefaultSprintToAnalyze()
        {
            Sprint sprint = unitOfWork.SprintRepository.GetLastInProgress();

            if (sprint == null)
                throw new NoSprintInProgressException();

            return sprint;
        }

        private Sprint RetrieveSpecificSprintToAnalyze(int sprintNumber)
        {
            Sprint sprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            if (sprint == null)
                throw new SprintDoesNotExistException(sprintNumber);

            return sprint;
        }

        private AnalyzeSprintResponse CreateResponse(SprintAnalysis sprintAnalysis)
        {
            return new AnalyzeSprintResponse
            {
                SprintName = sprintAnalysis.Sprint.Name,
                SprintState = sprintAnalysis.Sprint.State,
                SprintDateInterval = sprintAnalysis.Sprint.DateInterval,
                SprintDays = sprintAnalysis.Sprint.EnumerateAllDays()?.ToList(),
                WorkDaysCount = sprintAnalysis.Sprint.CountWorkDays(),
                SprintMembers = sprintAnalysis.Sprint.SprintMembersOrderedByEmployment?.ToList(),
                TotalWorkHours = sprintAnalysis.Sprint.TotalWorkHours,
                EstimatedStoryPoints = sprintAnalysis.EstimatedStoryPoints,
                EstimatedStoryPointsWithVelocityPenalties = sprintAnalysis.EstimatedStoryPointsWithVelocityPenalties,
                EstimatedVelocity = sprintAnalysis.EstimatedVelocity,
                VelocityPenalties = sprintAnalysis.VelocityPenalties?
                    .Select(x => new VelocityPenaltyInfo(x))
                    .ToList(),
                CommitmentStoryPoints = sprintAnalysis.Sprint.CommitmentStoryPoints,
                ActualStoryPoints = sprintAnalysis.Sprint.ActualStoryPoints,
                ActualVelocity = sprintAnalysis.Sprint.Velocity,
                PreviousSprints = sprintAnalysis.HistorySprints?
                    .Select(x => x.Number)
                    .ToList(),
                ExcludedSprints = sprintAnalysis.ExcludedSprints?.ToList(),
                CurrentDay = systemClock.Today
            };
        }
    }
}