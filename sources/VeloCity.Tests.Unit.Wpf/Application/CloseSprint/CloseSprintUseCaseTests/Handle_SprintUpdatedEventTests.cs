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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintCloseConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CloseSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.CloseSprint.CloseSprintUseCaseTests;

public class Handle_SprintUpdatedEventTests
{
    private readonly SprintCloseConfirmationResponse confirmationResponse;
    private readonly EventBus eventBus;
    private readonly Sprint sprintFromRepository;
    private readonly CloseSprintUseCase useCase;

    public Handle_SprintUpdatedEventTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();
        ApplicationState applicationState = new();
        eventBus = new EventBus();
        Mock<IUserInterface> userInterface = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        applicationState.SelectedSprintId = 247;
        sprintFromRepository = new Sprint
        {
            State = SprintState.InProgress
        };

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        confirmationResponse = new SprintCloseConfirmationResponse
        {
            IsAccepted = true
        };

        userInterface
            .Setup(x => x.ConfirmCloseSprint(It.IsAny<SprintCloseConfirmationRequest>()))
            .Returns(confirmationResponse);

        useCase = new CloseSprintUseCase(unitOfWork.Object, applicationState, eventBus, userInterface.Object);
    }

    [Fact]
    public async Task HavingValidSprintInRepositoryAndUserAcceptsClosingIt_WhenUseCaseIsExecuted_ThenEventContainsSprintId()
    {
        sprintFromRepository.Id = 26094;

        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.SprintId.Should().Be(26094);
    }

    [Fact]
    public async Task HavingValidSprintInRepositoryAndUserAcceptsClosingIt_WhenUseCaseIsExecuted_ThenEventContainsSprintNumber()
    {
        sprintFromRepository.Number = 364;

        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.SprintNumber.Should().Be(364);
    }

    [Fact]
    public async Task HavingValidSprintInRepositoryAndUserAcceptsClosingIt_WhenUseCaseIsExecuted_ThenEventContainsSprintTitle()
    {
        sprintFromRepository.Title = "title bla bla";

        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.SprintTitle.Should().Be("title bla bla");
    }

    [Fact]
    public async Task HavingValidSprintInRepositoryAndUserAcceptsClosingIt_WhenUseCaseIsExecuted_ThenEventContainsSprintState()
    {
        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.SprintState.Should().Be(SprintState.Closed);
    }

    [Fact]
    public async Task HavingValidSprintInRepositoryAndUserAcceptsClosingIt_WhenUseCaseIsExecuted_ThenEventContainsCommitmentStoryPoints()
    {
        sprintFromRepository.CommitmentStoryPoints = 37;

        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.CommitmentStoryPoints.Should().Be((StoryPoints)37);
    }

    [Fact]
    public async Task HavingValidSprintInRepositoryAndUserAcceptsClosingIt_WhenUseCaseIsExecuted_ThenEventContainsSprintGoal()
    {
        sprintFromRepository.Goal = "some goal here";

        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.SprintGoal.Should().Be("some goal here");
    }

    [Fact]
    public async Task HavingValidSprintInRepositoryAndUserAcceptsClosingIt_WhenUseCaseIsExecuted_ThenEventContainsActualStoryPoints()
    {
        sprintFromRepository.ActualStoryPoints = 20;
        confirmationResponse.ActualStoryPoints = 30;

        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.ActualStoryPoints.Should().Be((StoryPoints)30);
    }

    [Fact]
    public async Task HavingValidSprintInRepositoryAndUserAcceptsClosingIt_WhenUseCaseIsExecuted_ThenEventContainsActualVelocity()
    {
        confirmationResponse.ActualStoryPoints = 50;
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 13), new DateTime(2023, 03, 19));

        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2000, 01, 13),
                    HoursPerDay = 5,
                    EmploymentWeek = EmploymentWeek.NewDefault
                }
            }
        };
        sprintFromRepository.AddSprintMember(teamMember);

        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.ActualVelocity.Should().Be((Velocity)2);
    }

    [Fact]
    public async Task HavingValidSprintInRepositoryAndUserAcceptsClosingIt_WhenUseCaseIsExecuted_ThenEventContainsSprintComments()
    {
        sprintFromRepository.Comments = "comments 1";
        confirmationResponse.Comments = "comments 2";

        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.Comments.Should().Be("comments 2");
    }
}