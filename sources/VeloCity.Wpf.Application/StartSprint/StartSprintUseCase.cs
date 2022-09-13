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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.Configuring;
using DustInTheWind.VeloCity.Domain.DataAccess;
using DustInTheWind.VeloCity.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.StartSprint
{
    internal class StartSprintUseCase : IRequestHandler<StartSprintRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;
        private readonly EventBus eventBus;
        private readonly ISprintStartDataProvider sprintStartDataProvider;
        private readonly IConfig config;

        public StartSprintUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState, EventBus eventBus,
            ISprintStartDataProvider sprintStartDataProvider, IConfig config)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.sprintStartDataProvider = sprintStartDataProvider ?? throw new ArgumentNullException(nameof(sprintStartDataProvider));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<Unit> Handle(StartSprintRequest request, CancellationToken cancellationToken)
        {
            Sprint selectedSprint = RetrieveSelectedSprint();

            ValidateSprintState(selectedSprint);
            ValidateNoSprintIsInProgress(selectedSprint);
            ValidateSprintIsNextInLine(selectedSprint);

            StartSprintConfirmationResponse sprintStartConfirmationResponse = RequestUserConfirmation(selectedSprint);

            if (sprintStartConfirmationResponse.IsAccepted)
            {
                UpdateSprint(selectedSprint, sprintStartConfirmationResponse);
                unitOfWork.SaveChanges();

                await RaiseSprintUpdatedEvent(selectedSprint, cancellationToken);
            }

            return Unit.Value;
        }

        private Sprint RetrieveSelectedSprint()
        {
            int? selectedSprintId = applicationState.SelectedSprintId;

            if (selectedSprintId == null)
                throw new Exception("No sprint is selected.");

            return unitOfWork.SprintRepository.Get(selectedSprintId.Value);
        }

        private static void ValidateSprintState(Sprint sprint)
        {
            switch (sprint.State)
            {
                case SprintState.Unknown:
                    throw new Exception($"The sprint {sprint.Number} is in a invalid state.");

                case SprintState.New:
                    break;

                case SprintState.InProgress:
                    throw new Exception($"The sprint {sprint.Number} is already in progress.");

                case SprintState.Closed:
                    throw new Exception($"The sprint {sprint.Number} is closed. A closed sprint cannot be started again.");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ValidateSprintIsNextInLine(Sprint sprint)
        {
            bool isNextInLine = unitOfWork.SprintRepository.IsFirstNewSprint(sprint.Id);

            if (!isNextInLine)
                throw new Exception($"The sprint {sprint.Number} cannot be started because it is not the next sprint.");
        }

        private void ValidateNoSprintIsInProgress(Sprint sprint)
        {
            Sprint sprintInProgress = unitOfWork.SprintRepository.GetLastInProgress();

            if (sprintInProgress != null)
                throw new Exception($"The sprint {sprint.Number} cannot be started because sprint {sprintInProgress.Number} is already in progress.");
        }

        private StartSprintConfirmationResponse RequestUserConfirmation(Sprint selectedSprint)
        {
            SprintAnalysis sprintAnalysis = new(unitOfWork)
            {
                AnalysisLookBack = config.AnalysisLookBack
            };
            sprintAnalysis.Analyze(selectedSprint);

            StartSprintConfirmationRequest startSprintConfirmationRequest = new()
            {
                SprintName = selectedSprint.Name,
                SprintNumber = selectedSprint.Number,
                EstimatedStoryPoints = sprintAnalysis.EstimatedStoryPoints,
                CommitmentStoryPoints = StoryPoints.Empty,
                SprintGoal = selectedSprint.Goal
            };

            return sprintStartDataProvider.ConfirmStartSprint(startSprintConfirmationRequest);
        }

        private static void UpdateSprint(Sprint selectedSprint, StartSprintConfirmationResponse sprintStartConfirmationResponse)
        {
            selectedSprint.State = SprintState.InProgress;
            selectedSprint.CommitmentStoryPoints = sprintStartConfirmationResponse.CommitmentStoryPoints;
            selectedSprint.Goal = string.IsNullOrWhiteSpace(sprintStartConfirmationResponse.SprintGoal)
                ? null
                : sprintStartConfirmationResponse.SprintGoal;
        }

        private async Task RaiseSprintUpdatedEvent(Sprint sprint, CancellationToken cancellationToken)
        {
            SprintUpdatedEvent sprintUpdatedEvent = new()
            {
                SprintId = sprint.Id,
                SprintState = sprint.State,
                CommitmentStoryPoints = sprint.CommitmentStoryPoints,
                SprintGoal = sprint.Goal,
                ActualStoryPoints = sprint.ActualStoryPoints,
                ActualVelocity = sprint.Velocity,
                Comments = sprint.Comments
            };

            await eventBus.Publish(sprintUpdatedEvent, cancellationToken);
        }
    }
}