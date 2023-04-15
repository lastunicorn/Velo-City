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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.UpdateVacationHours.UpdateVacationHoursUseCaseTests;

public class HandleTests
{
    private readonly UpdateVacationHoursUseCase useCase;
    private readonly Mock<ITeamMemberRepository> teamMemberRepository;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        teamMemberRepository = new Mock<ITeamMemberRepository>();

        unitOfWork
            .Setup(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        EventBus eventBus = new();

        useCase = new UpdateVacationHoursUseCase(unitOfWork.Object, eventBus);
    }

    [Fact]
    public async Task HavingInRequestTeamMemberId_WhenExecutingTheUseCase_ThenThatIdIsRequestedFromRepository()
    {
        UpdateVacationHoursRequest request = new()
        {
            TeamMemberId = 905
        };

        try
        {
            await useCase.Handle(request, CancellationToken.None);
        }
        catch { }

        teamMemberRepository.Verify(x => x.Get(905), Times.Once);
    }

    [Fact]
    public async Task HavingInRequestTeamMemberIdThatDoesNotExistInRepository_WhenExecutingTheUseCase_ThenThrows()
    {
        UpdateVacationHoursRequest request = new()
        {
            TeamMemberId = 905
        };

        teamMemberRepository
            .Setup(x => x.Get(905))
            .ReturnsAsync(null as TeamMember);

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<TeamMemberDoesNotExistException>();
    }
}