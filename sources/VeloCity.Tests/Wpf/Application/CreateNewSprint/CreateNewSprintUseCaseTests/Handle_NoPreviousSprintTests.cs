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
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintNewConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.CreateNewSprint.CreateNewSprintUseCaseTests;

public class Handle_NoPreviousSprintTests
{
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly Mock<IUserInterface> userInterface;
    private readonly EventBus eventBus;
    private readonly ApplicationState applicationState;
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly CreateNewSprintUseCase useCase;

    // call to retrieve last sprint (choices: sprint exists, does not exist)
    // call to ask user (response choices: yes, no)
    // save new sprint
    // set new sprint as current
    // raise event

    public Handle_NoPreviousSprintTests()
    {
        unitOfWork = new Mock<IUnitOfWork>();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        sprintRepository
            .Setup(x => x.GetLast())
            .Returns(null as Sprint);

        userInterface = new Mock<IUserInterface>();
        eventBus = new EventBus();
        applicationState = new ApplicationState();

        useCase = new(unitOfWork.Object, userInterface.Object, eventBus, applicationState);
    }

    [Fact]
    public async Task HavingUseCaseInstance_WhenUseCaseIsExecuted_ThenAttemptsToRetrieveLastSprintFromRepository()
    {
        SprintNewConfirmationResponse confirmationResponse = new()
        {
            IsAccepted = true,
            SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14))
        };

        userInterface
            .Setup(x => x.ConfirmNewSprint(It.IsAny<SprintNewConfirmationRequest>()))
            .Returns(confirmationResponse);

        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintRepository.Verify(x => x.GetLast(), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintInRepository_WhenUseCaseIsExecuted_ThenAskTheUserForPermission()
    {
        userInterface
            .Setup(x => x.ConfirmNewSprint(It.IsAny<SprintNewConfirmationRequest>()))
            .Returns(new SprintNewConfirmationResponse());

        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        userInterface.Verify(x => x.ConfirmNewSprint(It.IsNotNull<SprintNewConfirmationRequest>()), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintInRepositoryAndUserAcceptsNewSprint_WhenUseCaseIsExecuted_ThenNewSprintIsAddedInTheRepository()
    {
        SprintNewConfirmationResponse confirmationResponse = new()
        {
            IsAccepted = true,
            SprintTitle = "new sprint title",
            SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14))
        };

        userInterface
            .Setup(x => x.ConfirmNewSprint(It.IsAny<SprintNewConfirmationRequest>()))
            .Returns(confirmationResponse);

        Sprint actualSprint = null;

        sprintRepository
            .Setup(x => x.Add(It.IsAny<Sprint>()))
            .Callback<Sprint>(sprint => actualSprint = sprint);

        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        Sprint expectedSprint = new()
        {
            Id = 0,
            Number = 1,
            Title = "new sprint title",
            DateInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14)),
            State = SprintState.New
        };

        AssertSprintEquals(actualSprint, expectedSprint);
    }

    [Fact]
    public async Task HavingNoSprintInRepositoryAndUserAcceptsNewSprint_WhenUseCaseIsExecuted_ThenUnitOfWorkIsSaved()
    {
        SprintNewConfirmationResponse confirmationResponse = new()
        {
            IsAccepted = true,
            SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14))
        };

        userInterface
            .Setup(x => x.ConfirmNewSprint(It.IsAny<SprintNewConfirmationRequest>()))
            .Returns(confirmationResponse);

        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        unitOfWork.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintInRepositoryAndUserAcceptsNewSprint_WhenUseCaseIsExecuted_ThenSprintIdIsSetInApplicationState()
    {
        SprintNewConfirmationResponse confirmationResponse = new()
        {
            IsAccepted = true,
            SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14))
        };

        userInterface
            .Setup(x => x.ConfirmNewSprint(It.IsAny<SprintNewConfirmationRequest>()))
            .Returns(confirmationResponse);

        Sprint actualSprint = null;

        sprintRepository
            .Setup(x => x.Add(It.IsAny<Sprint>()))
            .Callback<Sprint>(sprint => actualSprint = sprint);

        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        applicationState.SelectedSprintId.Should().Be(actualSprint.Id);
    }

    [Fact]
    public async Task HavingNoSprintInRepositoryAndUserAcceptsNewSprint_WhenUseCaseIsExecuted_ThenSprintListChangedEventIsRaised()
    {
        SprintNewConfirmationResponse confirmationResponse = new()
        {
            IsAccepted = true,
            SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14))
        };

        userInterface
            .Setup(x => x.ConfirmNewSprint(It.IsAny<SprintNewConfirmationRequest>()))
            .Returns(confirmationResponse);

        Sprint actualSprint = null;

        sprintRepository
            .Setup(x => x.Add(It.IsAny<Sprint>()))
            .Callback<Sprint>(sprint => actualSprint = sprint);

        EventBusClient<SprintsListChangedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintsListChangedEvent>();
        
        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.VerifyEventWasTriggered();
        eventBusClient.Event.NewSprintId.Should().Be(actualSprint.Id);
    }

    private static void AssertSprintEquals(Sprint sprint, Sprint actualSprint)
    {
        actualSprint.Id.Should().Be(0);
        actualSprint.Number.Should().Be(1);
        actualSprint.Title.Should().Be("new sprint title");
        actualSprint.StartDate.Should().Be(new DateTime(2021, 10, 01));
        actualSprint.EndDate.Should().Be(new DateTime(2021, 10, 14));
        actualSprint.State.Should().Be(SprintState.New);
    }
}