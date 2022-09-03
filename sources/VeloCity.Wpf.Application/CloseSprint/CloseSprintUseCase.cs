﻿// Velo City
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
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.CloseSprint
{
    internal class CloseSprintUseCase : IRequestHandler<CloseSprintRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;
        private readonly EventBus eventBus;
        private readonly ISprintCloseDataProvider sprintCloseDataProvider;
        private readonly IConfig config;

        public CloseSprintUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState, EventBus eventBus,
            ISprintCloseDataProvider sprintCloseDataProvider, IConfig config)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.sprintCloseDataProvider = sprintCloseDataProvider ?? throw new ArgumentNullException(nameof(sprintCloseDataProvider));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<Unit> Handle(CloseSprintRequest request, CancellationToken cancellationToken)
        {
            Sprint selectedSprint = RetrieveSelectedSprint();

            ValidateSprintState(selectedSprint);

            CloseSprintConfirmationResponse closeSprintConfirmationResponse = RequestUserConfirmation(selectedSprint);

            if (closeSprintConfirmationResponse.IsAccepted)
            {
                UpdateSprint(selectedSprint, closeSprintConfirmationResponse);
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
                    throw new Exception($"The sprint {sprint.Number} is not in progress.");

                case SprintState.InProgress:
                    break;

                case SprintState.Closed:
                    throw new Exception($"The sprint {sprint.Number} is already closed.");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private CloseSprintConfirmationResponse RequestUserConfirmation(Sprint selectedSprint)
        {
            CloseSprintConfirmationRequest startSprintConfirmationRequest = new()
            {
                SprintName = selectedSprint.Name,
                SprintNumber = selectedSprint.Number
            };

            return sprintCloseDataProvider.ConfirmCloseSprint(startSprintConfirmationRequest);
        }

        private static void UpdateSprint(Sprint selectedSprint, CloseSprintConfirmationResponse closeSprintConfirmationResponse)
        {
            selectedSprint.State = SprintState.Closed;
            selectedSprint.ActualStoryPoints = closeSprintConfirmationResponse.ActualStoryPoints;
            selectedSprint.Comments = string.IsNullOrWhiteSpace(closeSprintConfirmationResponse.Comments)
                ? null
                : closeSprintConfirmationResponse.Comments;
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