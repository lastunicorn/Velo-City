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
using DustInTheWind.VeloCity.Ports.UserAccess.SprintNewConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.CreateNewSprint.CreateNewSprintUseCaseTests;

public class Handle_NoPreviousSprintTests
{
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly Mock<IUserInterface> userInterface;
    private readonly EventBus eventBus;
    private readonly ApplicationState applicationState;
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly CreateNewSprintUseCase useCase;
    private readonly SprintNewConfirmationResponse confirmationResponse;

    public Handle_NoPreviousSprintTests()
    {
        unitOfWork = new Mock<IUnitOfWork>();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        sprintRepository
            .Setup(x => x.GetLast())
            .ReturnsAsync(null as Sprint);

        userInterface = new Mock<IUserInterface>();

        confirmationResponse = new SprintNewConfirmationResponse();

        userInterface
            .Setup(x => x.ConfirmNewSprint(It.IsAny<SprintNewConfirmationRequest>()))
            .Returns(confirmationResponse);

        eventBus = new EventBus();
        applicationState = new ApplicationState();

        useCase = new CreateNewSprintUseCase(unitOfWork.Object, userInterface.Object, eventBus, applicationState);
    }

    [Fact]
    public async Task HavingUseCaseInstance_WhenUseCaseIsExecuted_ThenAttemptsToRetrieveLastSprintFromRepository()
    {
        // confirmationResponse.IsAccepted = true;
        // confirmationResponse.SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14));

        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintRepository.Verify(x => x.GetLast(), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintInRepository_WhenUseCaseIsExecuted_ThenAskTheUserForPermission()
    {
        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        userInterface.Verify(x => x.ConfirmNewSprint(It.IsNotNull<SprintNewConfirmationRequest>()), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintInRepositoryAndUserAcceptsNewSprint_WhenUseCaseIsExecuted_ThenNewSprintIsAddedInTheRepository()
    {
        confirmationResponse.IsAccepted = true;
        confirmationResponse.SprintTitle = "new sprint title";
        confirmationResponse.SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14));

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

        AssertSprintEquals(expectedSprint, actualSprint);
    }

    [Fact]
    public async Task HavingNoSprintInRepositoryAndUserAcceptsNewSprint_WhenUseCaseIsExecuted_ThenUnitOfWorkIsSaved()
    {
        confirmationResponse.IsAccepted = true;
        confirmationResponse.SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14));

        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        unitOfWork.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintInRepositoryAndUserAcceptsNewSprint_WhenUseCaseIsExecuted_ThenSprintIdIsSetInApplicationState()
    {
        confirmationResponse.IsAccepted = true;
        confirmationResponse.SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14));

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
        confirmationResponse.IsAccepted = true;
        confirmationResponse.SprintTimeInterval = new DateInterval(new DateTime(2021, 10, 01), new DateTime(2021, 10, 14));

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

    private static void AssertSprintEquals(Sprint expectedSprint, Sprint actualSprint)
    {
        actualSprint.Id.Should().Be(expectedSprint.Id);
        actualSprint.Number.Should().Be(expectedSprint.Number);
        actualSprint.Title.Should().Be(expectedSprint.Title);
        actualSprint.StartDate.Should().Be(expectedSprint.StartDate);
        actualSprint.EndDate.Should().Be(expectedSprint.EndDate);
        actualSprint.State.Should().Be(expectedSprint.State);
    }
}