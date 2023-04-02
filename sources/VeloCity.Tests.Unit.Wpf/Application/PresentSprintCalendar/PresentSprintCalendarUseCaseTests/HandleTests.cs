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
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentSprintCalendar.PresentSprintCalendarUseCaseTests;

public class HandleTests
{
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly ApplicationState applicationState;
    private readonly PresentSprintCalendarUseCase useCase;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        applicationState = new ApplicationState();
        Mock<ISystemClock> systemClock = new();

        useCase = new PresentSprintCalendarUseCase(unitOfWork.Object, applicationState, systemClock.Object);
    }

    [Fact]
    public async Task HavingNoSprintSelectedAndNoInProgressSprintInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = null;

        sprintRepository
            .Setup(x => x.GetLastInProgress())
            .ReturnsAsync(null as Sprint);

        Func<Task> action = async () =>
        {
            PresentSprintCalendarRequest request = new();
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<NoSprintInProgressException>();
    }

    [Fact]
    public async Task HavingSprintSelectedButNotInRepository_WhenUseCaseIsExecuted_ThenRetrievesThatSprintFromRepository()
    {
        applicationState.SelectedSprintId = 97;

        sprintRepository
            .Setup(x => x.Get(97))
            .ReturnsAsync(null as Sprint);

        try
        {
            PresentSprintCalendarRequest request = new();
            await useCase.Handle(request, CancellationToken.None);
        }
        catch { }

        sprintRepository.Verify(x => x.Get(97), Times.Once);
    }

    [Fact]
    public async Task HavingSprintSelectedButNotInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        applicationState.SelectedSprintId = 97;

        sprintRepository
            .Setup(x => x.Get(97))
            .ReturnsAsync(null as Sprint);

        Func<Task> action = async () =>
        {
            PresentSprintCalendarRequest request = new();
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<SprintDoesNotExistException>();
    }
}