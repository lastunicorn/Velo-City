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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CanStartSprint;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.CanStartSprint.CanStartSprintUseCaseTests;

public class Handle_SprintStateTests
{
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly ApplicationState applicationState;
    private readonly CanStartSprintUseCase useCase;

    public Handle_SprintStateTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        sprintRepository
            .Setup(x => x.IsAnyInProgress())
            .Returns(false);

        sprintRepository
            .Setup(x => x.IsFirstNewSprint(It.IsAny<int>()))
            .Returns(true);

        applicationState = new ApplicationState
        {
            SelectedSprintId = 4
        };

        useCase = new CanStartSprintUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingSprintWithInvalidStateInStorage_WhenUseCaseIsExecuted_ThenCanStartSprintIsFalse()
    {
        Sprint sprintFromStorage = new()
        {
            State = (SprintState)98732497
        };

        sprintRepository
            .Setup(x => x.Get(4))
            .ReturnsAsync(sprintFromStorage);

        CanStartSprintRequest request = new();
        CanStartSprintResponse response = await useCase.Handle(request, CancellationToken.None);

        response.CanStartSprint.Should().BeFalse();
    }

    [Fact]
    public async Task HavingSprintWithUnknownStateInStorage_WhenUseCaseIsExecuted_ThenCanStartSprintIsFalse()
    {
        applicationState.SelectedSprintId = 4;

        Sprint sprintFromStorage = new()
        {
            State = SprintState.Unknown
        };

        sprintRepository
            .Setup(x => x.Get(4))
            .ReturnsAsync(sprintFromStorage);

        CanStartSprintRequest request = new();
        CanStartSprintResponse response = await useCase.Handle(request, CancellationToken.None);

        response.CanStartSprint.Should().BeFalse();
    }

    [Fact]
    public async Task HavingSprintWithInProgressStateInStorage_WhenUseCaseIsExecuted_ThenCanStartSprintIsFalse()
    {
        applicationState.SelectedSprintId = 4;

        Sprint sprintFromStorage = new()
        {
            State = SprintState.InProgress
        };

        sprintRepository
            .Setup(x => x.Get(4))
            .ReturnsAsync(sprintFromStorage);

        CanStartSprintRequest request = new();
        CanStartSprintResponse response = await useCase.Handle(request, CancellationToken.None);

        response.CanStartSprint.Should().BeFalse();
    }

    [Fact]
    public async Task HavingSprintWithClosedStateInStorage_WhenUseCaseIsExecuted_ThenCanStartSprintIsFalse()
    {
        applicationState.SelectedSprintId = 4;

        Sprint sprintFromStorage = new()
        {
            State = SprintState.Closed
        };

        sprintRepository
            .Setup(x => x.Get(4))
            .ReturnsAsync(sprintFromStorage);

        CanStartSprintRequest request = new();
        CanStartSprintResponse response = await useCase.Handle(request, CancellationToken.None);

        response.CanStartSprint.Should().BeFalse();
    }

    [Fact]
    public async Task HavingSprintWithNewStateInStorage_WhenUseCaseIsExecuted_ThenCanStartSprintIsTrue()
    {
        applicationState.SelectedSprintId = 4;

        Sprint sprintFromStorage = new()
        {
            State = SprintState.New
        };

        sprintRepository
            .Setup(x => x.Get(4))
            .ReturnsAsync(sprintFromStorage);

        CanStartSprintRequest request = new();
        CanStartSprintResponse response = await useCase.Handle(request, CancellationToken.None);

        response.CanStartSprint.Should().BeTrue();
    }
}