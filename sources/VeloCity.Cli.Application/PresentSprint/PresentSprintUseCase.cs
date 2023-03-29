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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Cli.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SettingsAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Application.PresentSprint
{
    internal class PresentSprintUseCase : IRequestHandler<PresentSprintRequest, PresentSprintResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfig config;
        private readonly ISystemClock systemClock;
        private readonly IMediator mediator;

        public PresentSprintUseCase(IUnitOfWork unitOfWork, IConfig config, ISystemClock systemClock, IMediator mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PresentSprintResponse> Handle(PresentSprintRequest request, CancellationToken cancellationToken)
        {
            Sprint sprintToAnalyze = await RetrieveSprintToAnalyze(request);
            AnalyzeSprintResponse analyzeSprintResponse = await AnalyzeSprint(sprintToAnalyze, request);

            return CreateResponse(sprintToAnalyze, request, analyzeSprintResponse);
        }

        private async Task<Sprint> RetrieveSprintToAnalyze(PresentSprintRequest request)
        {
            Sprint sprint = request.SprintNumber == null
                ? await RetrieveDefaultSprintToAnalyze()
                : await RetrieveSpecificSprintToAnalyze(request.SprintNumber.Value);

            sprint.ExcludedTeamMembers = request.ExcludedTeamMembers;

            return sprint;
        }

        private async Task<Sprint> RetrieveDefaultSprintToAnalyze()
        {
            Sprint sprint = await unitOfWork.SprintRepository.GetLastInProgress();

            return sprint ?? throw new NoSprintInProgressException();
        }

        private async Task<Sprint> RetrieveSpecificSprintToAnalyze(int sprintNumber)
        {
            Sprint sprint = await unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            return sprint ?? throw new SprintDoesNotExistException(sprintNumber);
        }

        private async Task<AnalyzeSprintResponse> AnalyzeSprint(Sprint sprintToAnalyze, PresentSprintRequest presentSprintRequest)
        {
            AnalyzeSprintRequest request = new()
            {
                Sprint = sprintToAnalyze,
                ExcludedSprints = presentSprintRequest.ExcludedSprints,
                ExcludedTeamMembers = presentSprintRequest.ExcludedTeamMembers,
                AnalysisLookBack = presentSprintRequest.AnalysisLookBack ?? config.AnalysisLookBack
            };

            return await mediator.Send(request);
        }

        private PresentSprintResponse CreateResponse(Sprint sprint, PresentSprintRequest presentSprintRequest, AnalyzeSprintResponse analyzeSprintResponse)
        {
            return new PresentSprintResponse
            {
                SprintName = sprint.Title,
                SprintState = sprint.State,
                SprintDateInterval = sprint.DateInterval,
                SprintDays = sprint.EnumerateAllDays()?.ToList(),
                WorkDaysCount = sprint.CountWorkDays(),
                SprintMembers = sprint.SprintMembersOrderedByEmployment?.ToList(),
                TotalWorkHours = sprint.TotalWorkHours,
                EstimatedStoryPoints = analyzeSprintResponse.EstimatedStoryPoints,
                EstimatedStoryPointsWithVelocityPenalties = analyzeSprintResponse.EstimatedStoryPointsWithVelocityPenalties,
                EstimatedVelocity = analyzeSprintResponse.EstimatedVelocity,
                VelocityPenalties = analyzeSprintResponse.VelocityPenalties?
                    .Select(x => new VelocityPenaltyInfo(x))
                    .ToList(),
                CommitmentStoryPoints = sprint.CommitmentStoryPoints,
                ActualStoryPoints = sprint.ActualStoryPoints,
                ActualVelocity = sprint.Velocity,
                PreviouslyClosedSprints = analyzeSprintResponse.HistorySprints?
                    .Select(x => x.Number)
                    .ToList(),
                ExcludedSprints = presentSprintRequest.ExcludedSprints?.ToList(),
                CurrentDay = systemClock.Today
            };
        }
    }
}