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

using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintCloseConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CloseSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.CloseSprint.CloseSprintUseCaseTests;

public class Handle_UserAcceptTests
{
    private readonly SprintCloseConfirmationResponse confirmationResponse;
    private readonly EventBus eventBus;
    private readonly Sprint sprintFromRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly CloseSprintUseCase useCase;

    public Handle_UserAcceptTests()
    {
        unitOfWork = new Mock<IUnitOfWork>();
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
            .Returns(sprintFromRepository);

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
    public async Task HavingUserAcceptSprintClosing_WhenUseCaseIsExecuted_ThenSprintStateIsClosed()
    {
        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintFromRepository.State.Should().Be(SprintState.Closed);
    }

    [Fact]
    public async Task HavingUserProvideActualStoryPoints_WhenUseCaseIsExecuted_ThenSprintActualStoryPointsAreUpdated()
    {
        confirmationResponse.ActualStoryPoints = 36;

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintFromRepository.ActualStoryPoints.Should().Be((StoryPoints)36);
    }

    [Fact]
    public async Task HavingUserProvideComment_WhenUseCaseIsExecuted_ThenSprintCommentIsUpdated()
    {
        confirmationResponse.Comments = "this is a comment";

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintFromRepository.Comments.Should().Be("this is a comment");
    }

    [Fact]
    public async Task HavingUserProvideEmptyComment_WhenUseCaseIsExecuted_ThenSprintCommentIsNull()
    {
        confirmationResponse.Comments = string.Empty;

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintFromRepository.Comments.Should().BeNull();
    }

    [Fact]
    public async Task HavingUserAcceptSprintClosing_WhenUseCaseIsExecuted_ThenUnitOfWorkIsSaved()
    {
        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        unitOfWork.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task HavingUserAcceptSprintClosing_WhenUseCaseIsExecuted_ThenSprintUpdatedEventIsPublished()
    {
        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        CloseSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.EventWasTriggered.Should().BeTrue();
    }
}