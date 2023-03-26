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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintStartConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.StartSprint.StartSprintUseCaseTests;

public class Handle_UserNotApproveTests
{
    private readonly StartSprintUseCase useCase;
    private readonly Sprint sprintFromRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly EventBus eventBus;

    public Handle_UserNotApproveTests()
    {
        unitOfWork = new Mock<IUnitOfWork>();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        ApplicationState applicationState = new()
        {
            SelectedSprintId = 247
        };

        sprintFromRepository = new Sprint
        {
            State = SprintState.New
        };

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .Returns(sprintFromRepository);

        sprintRepository
            .Setup(x => x.IsFirstNewSprint(It.IsAny<int>()))
            .Returns(true);

        Mock<IRequestBus> requestBus = new();
        requestBus
            .Setup(x => x.Send<AnalyzeSprintRequest, AnalyzeSprintResponse>(It.IsAny<AnalyzeSprintRequest>(), CancellationToken.None))
            .Returns(Task.FromResult(new AnalyzeSprintResponse()));

        SprintStartConfirmationResponse userConfirmationResponse = new()
        {
            IsAccepted = false
        };

        Mock<IUserInterface> userInterface = new();
        userInterface
            .Setup(x => x.ConfirmStartSprint(It.IsAny<SprintStartConfirmationRequest>()))
            .Returns(userConfirmationResponse);

        eventBus = new EventBus();

        useCase = new StartSprintUseCase(unitOfWork.Object, applicationState, eventBus, userInterface.Object, requestBus.Object);
    }

    [Fact]
    public async Task HavingUserConfirmationFalse_WhenUseCaseIsExecuted_ThenSprintStateIsNotUpdated()
    {
        StartSprintRequest request = new();
        _ = await useCase.Handle(request, CancellationToken.None);

        sprintFromRepository.State.Should().Be(SprintState.New);
    }

    [Fact]
    public async Task HavingUserConfirmationFalse_WhenUseCaseIsExecuted_ThenCommitmentStoryPointsAreNotUpdated()
    {
        StartSprintRequest request = new();
        _ = await useCase.Handle(request, CancellationToken.None);

        sprintFromRepository.CommitmentStoryPoints.Should().Be((StoryPoints)0);
    }

    [Fact]
    public async Task HavingUserConfirmationFalse_WhenUseCaseIsExecuted_ThenTitleIsNotUpdated()
    {
        StartSprintRequest request = new();
        _ = await useCase.Handle(request, CancellationToken.None);

        sprintFromRepository.Title.Should().BeNull();
    }

    [Fact]
    public async Task HavingUserConfirmationFalse_WhenUseCaseIsExecuted_ThenGoalIsNotUpdated()
    {
        StartSprintRequest request = new();
        _ = await useCase.Handle(request, CancellationToken.None);

        sprintFromRepository.Goal.Should().BeNull();
    }

    [Fact]
    public async Task HavingUserConfirmationFalse_WhenUseCaseIsExecuted_ThenUnitOfWorkIsNotSaved()
    {
        StartSprintRequest request = new();
        _ = await useCase.Handle(request, CancellationToken.None);

        unitOfWork.Verify(x => x.SaveChanges(), Times.Never);
    }

    [Fact]
    public async Task HavingUserConfirmationFalse_WhenUseCaseIsExecuted_ThenSprintUpdatedEventIsNotPublished()
    {
        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        StartSprintRequest request = new();
        _ = await useCase.Handle(request, CancellationToken.None);

        eventBusClient.EventWasTriggered.Should().BeFalse();
    }
}