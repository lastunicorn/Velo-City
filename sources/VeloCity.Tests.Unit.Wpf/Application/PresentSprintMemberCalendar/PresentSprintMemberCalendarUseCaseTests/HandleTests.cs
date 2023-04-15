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
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintMemberCalendar;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentSprintMemberCalendar.PresentSprintMemberCalendarUseCaseTests;

public class HandleTests
{
    private readonly PresentSprintMemberCalendarUseCase useCase;
    private readonly Mock<ISprintRepository> sprintRepository;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        Mock<ISystemClock> systemClock = new();

        useCase = new PresentSprintMemberCalendarUseCase(unitOfWork.Object, systemClock.Object);
    }

    [Fact]
    public async Task HavingRepositoryIdInRequest_WhenUseCaseIsExecuted_ThenThatSprintIsRetrievedFromRepository()
    {
        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 3
        };

        try
        {
            await useCase.Handle(request, CancellationToken.None);
        }
        catch { }

        sprintRepository.Verify(x => x.Get(3), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(null as Sprint);

        PresentSprintMemberCalendarRequest request = new();

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<SprintDoesNotExistException>();
    }

    [Fact]
    public async Task HavingSprintInRepositoryButTeamMemberNotInSprint_WhenUseCaseIsExecuted_ThenThrows()
    {
        Sprint sprintFromRepository = new();

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        PresentSprintMemberCalendarRequest request = new();

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<TeamMemberNotInSprintException>();
    }
}