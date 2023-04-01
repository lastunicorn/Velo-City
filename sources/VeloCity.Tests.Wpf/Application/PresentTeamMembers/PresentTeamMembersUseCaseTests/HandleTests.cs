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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMembers;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentTeamMembers.PresentTeamMembersUseCaseTests;

public class HandleTests
{
    private readonly Mock<ITeamMemberRepository> teamMemberRepository;
    private readonly ApplicationState applicationState;
    private readonly PresentTeamMembersUseCase useCase;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        teamMemberRepository = new Mock<ITeamMemberRepository>();

        unitOfWork
            .SetupGet(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        applicationState = new ApplicationState();

        useCase = new PresentTeamMembersUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingUseCaseInstance_WhenUseCaseIsExecuted_ThenRetrievesAllTeamMembersFromRepository()
    {
        PresentTeamMembersRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        teamMemberRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task HavingNoTeamMemberInRepository_WhenUseCaseIsExecuted_ThenReturnsEmptyListOfTeamMembers()
    {
        teamMemberRepository
            .Setup(x => x.GetAll())
            .Returns(null as IEnumerable<TeamMember>);

        PresentTeamMembersRequest request = new();
        PresentTeamMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TeamMembers.Should().BeEmpty();
    }

    [Fact]
    public async Task HavingOneTeamMemberInRepository_WhenUseCaseIsExecuted_ThenReturnsThatTeamMembers()
    {
        List<TeamMember> teamMembersFromRepository = new()
        {
            new TeamMember { Id = 209348 }
        };

        teamMemberRepository
            .Setup(x => x.GetAll())
            .Returns(teamMembersFromRepository);

        PresentTeamMembersRequest request = new();
        PresentTeamMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        List<int> actualTeamMemberIds = response.TeamMembers
            .Select(x => x.Id)
            .ToList();
        actualTeamMemberIds.Should().Equal(209348);
    }

    [Fact]
    public async Task HavingNoTeamMemberSpecifiedInApplicationState_WhenUseCaseIsExecuted_ThenReturnsNullCurrentTeamMemberId()
    {
        PresentTeamMembersRequest request = new();
        PresentTeamMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.CurrentTeamMemberId.Should().BeNull();
    }

    [Fact]
    public async Task HavingTeamMemberSpecifiedInApplicationState_WhenUseCaseIsExecuted_ThenCurrentTeamMemberIdIsSpecified()
    {
        applicationState.SelectedTeamMemberId = 48;

        PresentTeamMembersRequest request = new();
        PresentTeamMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.CurrentTeamMemberId.Should().Be(48);
    }
}