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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.AnalyzeSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintOverview
{
    internal class PresentSprintOverviewUseCase : IRequestHandler<PresentSprintOverviewRequest, PresentSprintOverviewResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;
        private readonly IRequestBus requestBus;

        public PresentSprintOverviewUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState, IRequestBus requestBus)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
            this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
        }

        public async Task<PresentSprintOverviewResponse> Handle(PresentSprintOverviewRequest request, CancellationToken cancellationToken)
        {
            Sprint currentSprint = RetrieveSprintToAnalyze();
            AnalyzeSprintResponse analyzeSprintResponse = await AnalyzeSprint(currentSprint);

            PresentSprintOverviewResponse response = CreateResponse(currentSprint, analyzeSprintResponse);

            return response;
        }

        private Sprint RetrieveSprintToAnalyze()
        {
            return applicationState.SelectedSprintNumber == null
                ? RetrieveDefaultSprintToAnalyze()
                : RetrieveSpecificSprintToAnalyze(applicationState.SelectedSprintNumber.Value);
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

        private async Task<AnalyzeSprintResponse> AnalyzeSprint(Sprint sprint)
        {
            AnalyzeSprintRequest request = new()
            {
                Sprint = sprint
            };

            return await requestBus.Send<AnalyzeSprintRequest, AnalyzeSprintResponse>(request);
        }

        private static PresentSprintOverviewResponse CreateResponse(Sprint sprint, AnalyzeSprintResponse analyzeSprintResponse)
        {
            return new PresentSprintOverviewResponse
            {
                SprintState = sprint.State,
                SprintDateInterval = sprint.DateInterval,
                SprintGoal = sprint.Goal,
                WorkDaysCount = sprint.CountWorkDays(),
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
                ExcludedSprints = null,
                SprintComments = sprint.Comments,
            };
        }
    }
}