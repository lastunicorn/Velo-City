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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberVacations;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentTeamMemberVacations.PresentTeamMemberVacationsUseCaseTests;

public class HandleTests
{
    private readonly ApplicationState applicationState;
    private readonly PresentTeamMemberVacationsUseCase useCase;
    private readonly Mock<ITeamMemberRepository> teamMemberRepository;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        teamMemberRepository = new Mock<ITeamMemberRepository>();

        unitOfWork
            .Setup(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        applicationState = new ApplicationState();

        useCase = new PresentTeamMemberVacationsUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingNoTeamMemberSelected_WhenUseCaseIsExecuted_ThenResponseContainsEmptyVacationList()
    {
        applicationState.SelectedTeamMemberId = null;

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Vacations.Should().BeEmpty();
    }

    [Fact]
    public async Task HavingNoTeamMemberInRepository_WhenUseCaseIsExecuted_ThenResponseContainsEmptyVacationList()
    {
        applicationState.SelectedTeamMemberId = 123;

        teamMemberRepository
            .Setup(x => x.Get(123))
            .ReturnsAsync(null as TeamMember);

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Vacations.Should().BeEmpty();
    }

    [Fact]
    public async Task HavingTeamMemberWithNullVacationsInRepository_WhenUseCaseIsExecuted_ThenResponseContainsEmptyVacationList()
    {
        applicationState.SelectedTeamMemberId = 123;

        TeamMember teamMember = new()
        {
            Vacations = null
        };

        teamMemberRepository
            .Setup(x => x.Get(123))
            .ReturnsAsync(teamMember);

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Vacations.Should().BeEmpty();
    }

    [Fact]
    public async Task HavingTeamMemberWithEmptyVacationCollectionInRepository_WhenUseCaseIsExecuted_ThenResponseContainsEmptyVacationList()
    {
        applicationState.SelectedTeamMemberId = 123;

        TeamMember teamMember = new()
        {
            Vacations = new VacationCollection()
        };

        teamMemberRepository
            .Setup(x => x.Get(123))
            .ReturnsAsync(teamMember);

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Vacations.Should().BeEmpty();
    }

    [Fact]
    public async Task HavingTeamMemberWithOneVacationInRepository_WhenUseCaseIsExecuted_ThenResponseContainsOneVacation()
    {
        applicationState.SelectedTeamMemberId = 123;

        TeamMember teamMember = new()
        {
            Vacations = new VacationCollection
            {
                new VacationOnce()
            }
        };

        teamMemberRepository
            .Setup(x => x.Get(123))
            .ReturnsAsync(teamMember);

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Vacations.Should().HaveCount(1);
    }

    [Fact]
    public async Task HavingTeamMemberWithTwoVacationsInRepository_WhenUseCaseIsExecuted_ThenResponseContainsTwoVacation()
    {
        applicationState.SelectedTeamMemberId = 123;

        TeamMember teamMember = new()
        {
            Vacations = new VacationCollection
            {
                new VacationOnce(),
                new VacationOnce()
            }
        };

        teamMemberRepository
            .Setup(x => x.Get(123))
            .ReturnsAsync(teamMember);

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Vacations.Should().HaveCount(2);
    }
}