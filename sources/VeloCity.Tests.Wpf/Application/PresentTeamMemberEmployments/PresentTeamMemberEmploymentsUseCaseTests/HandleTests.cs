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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberEmployments;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentTeamMemberEmployments.PresentTeamMemberEmploymentsUseCaseTests;

public class HandleTests
{
    private readonly Mock<ITeamMemberRepository> teamMemberRepository;
    private readonly ApplicationState applicationState;
    private readonly PresentTeamMemberEmploymentsUseCase useCase;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        teamMemberRepository = new Mock<ITeamMemberRepository>();
        applicationState = new ApplicationState();

        unitOfWork
            .Setup(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        useCase = new PresentTeamMemberEmploymentsUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingNoTeamMemberInApplicationState_WhenUseCaseIsExecuted_ThenResponseContainsNoEmployment()
    {
        applicationState.SelectedTeamMemberId = null;

        PresentTeamMemberEmploymentsRequest request = new();
        PresentTeamMemberEmploymentsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Employments.Should().BeEmpty();
    }

    [Fact]
    public async Task HavingTeamMemberSpecifiedInApplicationState_WhenUseCaseIsExecuted_ThenRetrieveThatTeammemberFromRepository()
    {
        applicationState.SelectedTeamMemberId = 6;

        PresentTeamMemberEmploymentsRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        teamMemberRepository.Verify(x => x.Get(6), Times.Once);
    }

    [Fact]
    public async Task HavingTeamMemberInApplicationStateButMissingFromRepository_WhenUseCaseIsExecuted_ThenResponseContainsNoEmployment()
    {
        applicationState.SelectedTeamMemberId = 387;

        PresentTeamMemberEmploymentsRequest request = new();
        PresentTeamMemberEmploymentsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Employments.Should().BeEmpty();
    }

    [Fact]
    public async Task HavingTeamMemberWithOneEmploymentInRepository_WhenUseCaseIsExecuted_ThenResponseContainsOneEmployment()
    {
        // Arrange

        applicationState.SelectedTeamMemberId = 6;

        TeamMember teamMemberFromRepository = new()
        {
            Employments = new EmploymentCollection
            {
                new()
            }
        };

        teamMemberRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(teamMemberFromRepository);

        // Act

        PresentTeamMemberEmploymentsRequest request = new();
        PresentTeamMemberEmploymentsResponse response = await useCase.Handle(request, CancellationToken.None);

        // Assert

        response.Employments.Count.Should().Be(1);
    }

    [Fact]
    public async Task HavingTeamMemberWithTwoEmploymentsInRepository_WhenUseCaseIsExecuted_ThenResponseContainsTwoEmployments()
    {
        // Arrange

        applicationState.SelectedTeamMemberId = 6;

        TeamMember teamMemberFromRepository = new()
        {
            Employments = new EmploymentCollection
            {
                new(),
                new()
            }
        };

        teamMemberRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(teamMemberFromRepository);

        // Act

        PresentTeamMemberEmploymentsRequest request = new();
        PresentTeamMemberEmploymentsResponse response = await useCase.Handle(request, CancellationToken.None);

        // Assert

        response.Employments.Count.Should().Be(2);
    }
}