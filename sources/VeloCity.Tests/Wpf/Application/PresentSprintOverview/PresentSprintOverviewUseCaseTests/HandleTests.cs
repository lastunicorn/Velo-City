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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintOverview;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprintOverview.PresentSprintOverviewUseCaseTests;

public class HandleTests
{
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly PresentSprintOverviewUseCase useCase;
    private readonly ApplicationState applicationState;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        applicationState = new ApplicationState();
        Mock<IRequestBus> requestBus = new();

        useCase = new PresentSprintOverviewUseCase(unitOfWork.Object, applicationState, requestBus.Object);
    }

    [Fact]
    public async Task HavingNoSprintSelected_WhenUseCaseIsExecuted_ThenLastSprintIsRetrievedFromRepository()
    {
        applicationState.SelectedSprintId = null;

        PresentSprintOverviewRequest request = new();

        try
        {
            await useCase.Handle(request, CancellationToken.None);
        }
        catch { }

        sprintRepository.Verify(x => x.GetLastInProgress(), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintSelectedAndNoInProgressSprintInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = null;

        PresentSprintOverviewRequest request = new();

        sprintRepository
            .Setup(x => x.GetLastInProgress())
            .Returns(null as Sprint);

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<NoSprintInProgressException>();
    }

    [Fact]
    public async Task HavingASprintSelected_WhenUseCaseIsExecuted_ThenThatSpecificSprintIsRetrievedFromRepository()
    {
        applicationState.SelectedSprintId = 79352;

        PresentSprintOverviewRequest request = new();

        try
        {
            await useCase.Handle(request, CancellationToken.None);
        }
        catch { }

        sprintRepository.Verify(x => x.Get(79352), Times.Once);
    }

    [Fact]
    public async Task HavingASprintSelectedThatIsNotInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = 165;

        sprintRepository
            .Setup(x => x.Get(165))
            .Returns(null as Sprint);

        PresentSprintOverviewRequest request = new();

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<SprintDoesNotExistException>();
    }
}