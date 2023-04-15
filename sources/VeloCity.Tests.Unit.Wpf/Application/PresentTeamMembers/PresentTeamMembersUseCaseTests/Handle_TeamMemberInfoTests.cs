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
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMembers;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentTeamMembers.PresentTeamMembersUseCaseTests;

public class Handle_TeamMemberInfoTests
{
    private readonly Mock<ITeamMemberRepository> teamMemberRepository;
    private readonly PresentTeamMembersUseCase useCase;

    public Handle_TeamMemberInfoTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        teamMemberRepository = new Mock<ITeamMemberRepository>();

        unitOfWork
            .SetupGet(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        ApplicationState applicationState = new();

        useCase = new PresentTeamMembersUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingOneTeamMemberInRepository_WhenUseCaseIsExecuted_ThenReturnedTeamMemberContainsTheId()
    {
        List<TeamMember> teamMembersFromRepository = new()
        {
            new TeamMember { Id = 209348 }
        };

        teamMemberRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(teamMembersFromRepository);

        PresentTeamMembersRequest request = new();
        PresentTeamMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TeamMembers[0].Id.Should().Be(209348);
    }

    [Fact]
    public async Task HavingOneTeamMemberInRepository_WhenUseCaseIsExecuted_ThenReturnedTeamMemberContainsTheName()
    {
        List<TeamMember> teamMembersFromRepository = new()
        {
            new TeamMember { Name = "name 1" }
        };

        teamMemberRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(teamMembersFromRepository);

        PresentTeamMembersRequest request = new();
        PresentTeamMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TeamMembers[0].Name.Should().Be("name 1");
    }

    [Fact]
    public async Task HavingOneTeamMemberWithActiveEmploymentInRepository_WhenUseCaseIsExecuted_ThenReturnedTeamMemberContainsIsEmployedTrue()
    {
        List<TeamMember> teamMembersFromRepository = new()
        {
            new TeamMember
            {
                Employments = new EmploymentCollection
                {
                    new()
                    {
                        StartDate = new DateTime(2022, 03, 07)
                    }
                }
            }
        };

        teamMemberRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(teamMembersFromRepository);

        PresentTeamMembersRequest request = new();
        PresentTeamMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TeamMembers[0].IsEmployed.Should().BeTrue();
    }
}