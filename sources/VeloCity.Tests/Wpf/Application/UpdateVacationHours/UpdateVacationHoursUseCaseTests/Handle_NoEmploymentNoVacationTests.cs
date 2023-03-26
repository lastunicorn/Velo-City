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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.UpdateVacationHours.UpdateVacationHoursUseCaseTests;

public class Handle_NoEmploymentNoVacationTests
{
    private readonly UpdateVacationHoursUseCase useCase;
    private readonly TeamMember teamMember;

    public Handle_NoEmploymentNoVacationTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ITeamMemberRepository> teamMemberRepository = new();

        unitOfWork
            .Setup(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        teamMember = new TeamMember
        {
            Employments = null,
            Vacations = null
        };

        teamMemberRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .Returns(teamMember);

        EventBus eventBus = new();

        useCase = new UpdateVacationHoursUseCase(unitOfWork.Object, eventBus);
    }

    [Fact]
    public async Task HavingTeamMemberWithNoVacationAndRequestWithZeroHours_WhenExecutingTheUseCase_ThenThereIsNoVacation()
    {
        UpdateVacationHoursRequest request = new()
        {
            Date = new DateTime(2023, 03, 26),
            Hours = 0
        };

        await useCase.Handle(request, CancellationToken.None);

        teamMember.Vacations?.GetVacationsFor(new DateTime(2023, 03, 26)).Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task HavingTeamMemberWithNoVacationAndRequestWithPositiveHours_WhenExecutingTheUseCase_ThenThrows()
    {
        UpdateVacationHoursRequest request = new()
        {
            Date = new DateTime(2023, 03, 26),
            Hours = 5
        };

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<NotEmployedException>();
    }

    [Fact]
    public async Task HavingTeamMemberWithNoVacationAndRequestWithNegativeHours_WhenExecutingTheUseCase_ThenThereIsNoVacation()
    {
        UpdateVacationHoursRequest request = new()
        {
            Date = new DateTime(2023, 03, 26),
            Hours = -3
        };

        await useCase.Handle(request, CancellationToken.None);

        teamMember.Vacations?.GetVacationsFor(new DateTime(2023, 03, 26)).Should().BeNullOrEmpty();
    }
}