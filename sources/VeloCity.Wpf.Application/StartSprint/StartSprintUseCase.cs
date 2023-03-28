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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintStartConfirmation;
using DustInTheWind.VeloCity.Wpf.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Wpf.Application.CloseSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.StartSprint;

internal class StartSprintUseCase : IRequestHandler<StartSprintRequest>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ApplicationState applicationState;
    private readonly EventBus eventBus;
    private readonly IUserInterface userInterface;
    private readonly IRequestBus requestBus;

    public StartSprintUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState, EventBus eventBus,
        IUserInterface userInterface, IRequestBus requestBus)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
    }

    public async Task<Unit> Handle(StartSprintRequest request, CancellationToken cancellationToken)
    {
        Sprint selectedSprint = await RetrieveSelectedSprint();

        ValidateSprintState(selectedSprint);
        await ValidateNoSprintIsInProgress();
        ValidateSprintIsNextInLine(selectedSprint);

        SprintStartConfirmationResponse sprintStartConfirmationResponse = await RequestUserConfirmation(selectedSprint);

        if (sprintStartConfirmationResponse.IsAccepted)
        {
            UpdateSprint(selectedSprint, sprintStartConfirmationResponse);
            unitOfWork.SaveChanges();

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

        if (sprint == null)
            throw new SprintDoesNotExistException(selectedSprintId.Value);

        return sprint;
    }

    private static void ValidateSprintState(Sprint sprint)
    {
        if (sprint.State != SprintState.New)
            throw new InvalidSprintStateException(sprint.Number, sprint.State);
    }

    private async Task ValidateNoSprintIsInProgress()
    {
        Sprint sprintInProgress = await unitOfWork.SprintRepository.GetLastInProgress();

        if (sprintInProgress != null)
            throw new OtherSprintAlreadyInProgressException(sprintInProgress.Number);
    }

    private void ValidateSprintIsNextInLine(Sprint sprint)
    {
        bool isNextInLine = unitOfWork.SprintRepository.IsFirstNewSprint(sprint.Id);

        if (!isNextInLine)
            throw new SprintIsNotNextException(sprint.Number);
    }

    private async Task<SprintStartConfirmationResponse> RequestUserConfirmation(Sprint selectedSprint)
    {
        AnalyzeSprintRequest request = new()
        {
            Sprint = selectedSprint
        };
        AnalyzeSprintResponse analysisResponse = await requestBus.Send<AnalyzeSprintRequest, AnalyzeSprintResponse>(request);

        SprintStartConfirmationRequest sprintStartConfirmationRequest = new()
        {
            SprintTitle = selectedSprint.Title,
            SprintNumber = selectedSprint.Number,
            EstimatedStoryPoints = analysisResponse.EstimatedStoryPoints,
            CommitmentStoryPoints = StoryPoints.Empty,
            SprintGoal = selectedSprint.Goal
        };

        SprintStartConfirmationResponse userResponse = userInterface.ConfirmStartSprint(sprintStartConfirmationRequest);

        if (userResponse == null)
            throw new InternalException("User confirmation response is null.");

        return userResponse;
    }

    private static void UpdateSprint(Sprint selectedSprint, SprintStartConfirmationResponse sprintStartConfirmationResponse)
    {
        selectedSprint.State = SprintState.InProgress;
        selectedSprint.CommitmentStoryPoints = sprintStartConfirmationResponse.CommitmentStoryPoints;
        selectedSprint.Title = string.IsNullOrWhiteSpace(sprintStartConfirmationResponse.SprintTitle)
            ? null
            : sprintStartConfirmationResponse.SprintTitle;
        selectedSprint.Goal = string.IsNullOrWhiteSpace(sprintStartConfirmationResponse.SprintGoal)
            ? null
            : sprintStartConfirmationResponse.SprintGoal;
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