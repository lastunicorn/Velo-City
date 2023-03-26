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
using DustInTheWind.VeloCity.Ports.UserAccess.SprintStartConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Wpf.Application.CloseSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.StartSprint.StartSprintUseCaseTests;

public class HandleTests
{
    private readonly ApplicationState applicationState;
    private readonly Mock<IRequestBus> requestBus;
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly StartSprintUseCase useCase;
    private readonly Mock<IUserInterface> userInterface;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        sprintRepository = new Mock<ISprintRepository>();
        applicationState = new ApplicationState();
        EventBus eventBus = new();
        userInterface = new Mock<IUserInterface>();
        requestBus = new Mock<IRequestBus>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        useCase = new StartSprintUseCase(unitOfWork.Object, applicationState, eventBus, userInterface.Object, requestBus.Object);
    }

    [Fact]
    public async Task HavingNoSprintIdSetInApplicationState_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = null;
        StartSprintRequest request = new();

        Func<Task> action = async () => { await useCase.Handle(request, CancellationToken.None); };

        await action.Should().ThrowAsync<NoSprintSelectedException>();
    }

    [Fact]
    public async Task HavingSprintIdSetInApplicationState_WhenUseCaseIsExecuted_ThenThatSprintIsRetrievedFromRepository()
    {
        applicationState.SelectedSprintId = 247;
        Sprint sprintFromRepository = new()
        {
            State = SprintState.New
        };

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .Returns(sprintFromRepository);

        sprintRepository
            .Setup(x => x.IsFirstNewSprint(It.IsAny<int>()))
            .Returns(true);

        requestBus
            .Setup(x => x.Send<AnalyzeSprintRequest, AnalyzeSprintResponse>(It.IsAny<AnalyzeSprintRequest>(), CancellationToken.None))
            .Returns(Task.FromResult(new AnalyzeSprintResponse()));

        userInterface
            .Setup(x => x.ConfirmStartSprint(It.IsAny<SprintStartConfirmationRequest>()))
            .Returns(new SprintStartConfirmationResponse());

        StartSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintRepository.Verify(x => x.Get(247), Times.Once);
    }

    [Fact]
    public async Task HavingSprintIdSetInApplicationStateButNoSprintInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = 247;

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .Returns(null as Sprint);

        StartSprintRequest request = new();
        Func<Task> action = async () => { await useCase.Handle(request, CancellationToken.None); };

        await action.Should().ThrowAsync<SprintDoesNotExistException>();
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
            .Returns(sprintFromRepository);

        sprintRepository
            .Setup(x => x.IsFirstNewSprint(It.IsAny<int>()))
            .Returns(true);

        requestBus
            .Setup(x => x.Send<AnalyzeSprintRequest, AnalyzeSprintResponse>(It.IsAny<AnalyzeSprintRequest>(), CancellationToken.None))
            .Returns(Task.FromResult(new AnalyzeSprintResponse()));

        userInterface
            .Setup(x => x.ConfirmStartSprint(It.IsAny<SprintStartConfirmationRequest>()))
            .Returns(null as SprintStartConfirmationResponse);


        StartSprintRequest request = new();
        Func<Task> action = async () => { await useCase.Handle(request, CancellationToken.None); };

        await action.Should().ThrowAsync<InternalException>();
    }
}