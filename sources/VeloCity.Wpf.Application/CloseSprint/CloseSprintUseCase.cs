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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintCloseConfirmation;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.CloseSprint;

internal class CloseSprintUseCase : IRequestHandler<CloseSprintRequest>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ApplicationState applicationState;
    private readonly EventBus eventBus;
    private readonly IUserInterface userInterface;

    public CloseSprintUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState, EventBus eventBus, IUserInterface userInterface)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
    }

    public async Task<Unit> Handle(CloseSprintRequest request, CancellationToken cancellationToken)
    {
        Sprint selectedSprint = await RetrieveSelectedSprint();
        ValidateSprintState(selectedSprint);

        SprintCloseConfirmationResponse sprintCloseConfirmationResponse = RequestUserConfirmation(selectedSprint);

        if (sprintCloseConfirmationResponse.IsAccepted)
        {
            CloseSprint(selectedSprint, sprintCloseConfirmationResponse);
            await unitOfWork.SaveChanges();

            await RaiseSprintUpdatedEvent(selectedSprint, cancellationToken);
        }

        return Unit.Value;
    }

    private async Task<Sprint> RetrieveSelectedSprint()
    {
        int? selectedSprintId = applicationState.SelectedSprintId;

        if (selectedSprintId == null)
            throw new NoSprintSelectedException();

        Sprint sprint = await unitOfWork.SprintRepository.Get(selectedSprintId.Value);

        return sprint ?? throw new SprintDoesNotExistException(selectedSprintId.Value);
    }

    private static void ValidateSprintState(Sprint sprint)
    {
        if (sprint.State != SprintState.InProgress)
            throw new InvalidSprintStateException(sprint.Number, sprint.State);
    }

    private SprintCloseConfirmationResponse RequestUserConfirmation(Sprint selectedSprint)
    {
        SprintCloseConfirmationRequest request = new()
        {
            SprintName = selectedSprint.Title,
            SprintNumber = selectedSprint.Number,
            Comments = selectedSprint.Comments
        };

        SprintCloseConfirmationResponse response = userInterface.ConfirmCloseSprint(request);

        return response ?? throw new InternalException("User confirmation response is null.");
    }

    private static void CloseSprint(Sprint selectedSprint, SprintCloseConfirmationResponse sprintCloseConfirmationResponse)
    {
        selectedSprint.State = SprintState.Closed;
        selectedSprint.ActualStoryPoints = sprintCloseConfirmationResponse.ActualStoryPoints;
        selectedSprint.Comments = string.IsNullOrWhiteSpace(sprintCloseConfirmationResponse.Comments)
            ? null
            : sprintCloseConfirmationResponse.Comments;
    }

    private async Task RaiseSprintUpdatedEvent(Sprint sprint, CancellationToken cancellationToken)
    {
        SprintUpdatedEvent sprintUpdatedEvent = new()
        {
            SprintId = sprint.Id,
            SprintNumber = sprint.Number,
            SprintTitle = sprint.Title,
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