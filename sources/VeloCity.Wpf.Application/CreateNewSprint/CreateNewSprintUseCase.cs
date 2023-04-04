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

using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintNewConfirmation;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint;

public class CreateNewSprintUseCase : IRequestHandler<CreateNewSprintRequest>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserInterface userInterface;
    private readonly EventBus eventBus;
    private readonly ApplicationState applicationState;

    public CreateNewSprintUseCase(IUnitOfWork unitOfWork, IUserInterface userInterface, EventBus eventBus, ApplicationState applicationState)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public async Task<Unit> Handle(CreateNewSprintRequest request, CancellationToken cancellationToken)
    {
        Sprint lastSprint = await RetrieveLastSprintFromStorage();
        SprintNewConfirmationResponse sprintNewConfirmationResponse = RequestUserConfirmationToCreateNewSprint(lastSprint);

        if (sprintNewConfirmationResponse?.IsAccepted == true)
        {
            Sprint newSprint = CreateNewSprint(sprintNewConfirmationResponse, lastSprint);
            await unitOfWork.SaveChanges();

            SetTheNewSprintAsCurrent(newSprint);
            await RaiseSprintListChangedEvent(newSprint, cancellationToken);
        }

        return Unit.Value;
    }

    private async Task<Sprint> RetrieveLastSprintFromStorage()
    {
        return await unitOfWork.SprintRepository.GetLast();
    }

    private SprintNewConfirmationResponse RequestUserConfirmationToCreateNewSprint(Sprint lastSprint)
    {
        SprintNewConfirmationRequest sprintNewConfirmationRequest = new()
        {
            SprintNumber = lastSprint?.Number + 1 ?? 1,
            SprintStartDate = lastSprint?.EndDate.AddDays(1) ?? DateTime.Today,
            SprintLength = 14
        };
        return userInterface.ConfirmNewSprint(sprintNewConfirmationRequest);
    }

    private Sprint CreateNewSprint(SprintNewConfirmationResponse sprintNewConfirmationResponse, Sprint lastSprint)
    {
        Sprint newSprint = new()
        {
            Id = 0,
            Number = lastSprint?.Number + 1 ?? 1,
            Title = sprintNewConfirmationResponse.SprintTitle,
            DateInterval = sprintNewConfirmationResponse.SprintTimeInterval,
            State = SprintState.New
        };

        unitOfWork.SprintRepository.Add(newSprint);

        return newSprint;
    }

    private void SetTheNewSprintAsCurrent(Sprint newSprint)
    {
        applicationState.SelectedSprintId = newSprint.Id;
    }

    private async Task RaiseSprintListChangedEvent(Sprint newSprint, CancellationToken cancellationToken)
    {
        SprintsListChangedEvent sprintChangedEvent = new()
        {
            NewSprintId = newSprint.Id
        };

        await eventBus.Publish(sprintChangedEvent, cancellationToken);
    }
}