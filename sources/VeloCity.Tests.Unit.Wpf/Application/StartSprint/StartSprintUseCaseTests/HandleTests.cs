// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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
using DustInTheWind.VeloCity.Ports.UserAccess.SprintStartConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Wpf.Application.CloseSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.StartSprint.StartSprintUseCaseTests;

public class HandleTests
{
    private readonly ApplicationState applicationState;
    private readonly Mock<IRequestBus> requestBus;
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly StartSprintUseCase useCase;
    private readonly Mock<IUserTerminal> userInterface;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        sprintRepository = new Mock<ISprintRepository>();
        applicationState = new ApplicationState();
        EventBus eventBus = new();
        userInterface = new Mock<IUserTerminal>();
        requestBus = new Mock<IRequestBus>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        useCase = new StartSprintUseCase(unitOfWork.Object, applicationState, eventBus, userInterface.Object, requestBus.Object);
    }

    [Fact]
    public async Task HavingNoSprintSelected_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = null;
        StartSprintRequest request = new();

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<NoSprintSelectedException>();
    }

    [Fact]
    public async Task HavingNoSprintInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = 247;

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(null as Sprint);

        StartSprintRequest request = new();
        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<SprintDoesNotExistException>();
    }

    [Fact]
    public async Task HavingSprintSelected_WhenUseCaseIsExecuted_ThenThatSprintIsRetrievedFromRepository()
    {
        applicationState.SelectedSprintId = 247;

        StartSprintRequest request = new();

        try
        {
            await useCase.Handle(request, CancellationToken.None);
        }
        catch { }

        sprintRepository.Verify(x => x.Get(247), Times.Once);
    }

    [Theory]
    [InlineData(SprintState.Unknown)]
    [InlineData(SprintState.InProgress)]
    [InlineData(SprintState.Closed)]
    [InlineData((SprintState)93450)]
    public async Task HavingSprintInRepositoryWithInvalidState_WhenUseCaseIsExecuted_ThenThrows(SprintState sprintState)
    {
        applicationState.SelectedSprintId = 247;
        Sprint sprintFromRepository = new()
        {
            State = sprintState
        };
        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        StartSprintRequest request = new();
        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<InvalidSprintStateException>();
    }

    [Fact]
    public async Task HavingAnotherSprintInProgress_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = 247;
        Sprint sprintFromRepository = new()
        {
            State = SprintState.New
        };
        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);
        sprintRepository
            .Setup(x => x.GetLastInProgress())
            .ReturnsAsync(new Sprint());

        StartSprintRequest request = new();
        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<OtherSprintAlreadyInProgressException>();
    }

    [Fact]
    public async Task HavingSprintNotBeingTheNextInLine_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = 247;
        Sprint sprintFromRepository = new()
        {
            State = SprintState.New
        };
        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);
        sprintRepository
            .Setup(x => x.GetLastInProgress())
            .ReturnsAsync(null as Sprint);
        sprintRepository
            .Setup(x => x.IsFirstNewSprint(247))
            .ReturnsAsync(false);

        StartSprintRequest request = new();
        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<SprintIsNotNextException>();
    }

    [Fact]
    public async Task HavingValidNewSprintInRepositoryAndNullResponseFromTheUser_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = 247;
        Sprint sprintFromRepository = new()
        {
            State = SprintState.New
        };

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        sprintRepository
            .Setup(x => x.IsFirstNewSprint(It.IsAny<int>()))
            .ReturnsAsync(true);

        requestBus
            .Setup(x => x.Send<AnalyzeSprintRequest, AnalyzeSprintResponse>(It.IsAny<AnalyzeSprintRequest>(), CancellationToken.None))
            .Returns(Task.FromResult(new AnalyzeSprintResponse()));

        userInterface
            .Setup(x => x.ConfirmStartSprint(It.IsAny<SprintStartConfirmationRequest>()))
            .Returns(null as SprintStartConfirmationResponse);

        StartSprintRequest request = new();
        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<InternalException>();
    }
}