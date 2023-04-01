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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintCloseConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CloseSprint;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.CloseSprint.CloseSprintUseCaseTests;

public class Handle_SprintStateTests
{
    private readonly Sprint sprintFromRepository;
    private readonly CloseSprintUseCase useCase;
    private readonly Mock<IUserInterface> userInterface;

    public Handle_SprintStateTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();
        ApplicationState applicationState = new();
        EventBus eventBus = new();
        userInterface = new Mock<IUserInterface>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        applicationState.SelectedSprintId = 247;
        sprintFromRepository = new Sprint();

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        useCase = new CloseSprintUseCase(unitOfWork.Object, applicationState, eventBus, userInterface.Object);
    }

    [Fact]
    public async Task HavingSprintWithStatusUnknownInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        sprintFromRepository.State = SprintState.Unknown;

        CloseSprintRequest request = new();

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<InvalidSprintStateException>();
    }

    [Fact]
    public async Task HavingSprintWithStatusNewInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        sprintFromRepository.State = SprintState.New;

        CloseSprintRequest request = new();

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<InvalidSprintStateException>();
    }

    [Fact]
    public async Task HavingSprintWithStatusInProgressInRepository_WhenUseCaseIsExecuted_ThenDoesNotThrow()
    {
        sprintFromRepository.State = SprintState.InProgress;

        userInterface
            .Setup(x => x.ConfirmCloseSprint(It.IsAny<SprintCloseConfirmationRequest>()))
            .Returns(() => new SprintCloseConfirmationResponse());

        CloseSprintRequest request = new();

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task HavingSprintWithStatusClosedInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        sprintFromRepository.State = SprintState.Closed;

        CloseSprintRequest request = new();

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<InvalidSprintStateException>();
    }
}