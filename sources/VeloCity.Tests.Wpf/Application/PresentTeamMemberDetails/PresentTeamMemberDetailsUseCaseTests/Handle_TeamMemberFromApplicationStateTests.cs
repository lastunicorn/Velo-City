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
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberDetails;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentTeamMemberDetails.PresentTeamMemberDetailsUseCaseTests;

public class Handle_TeamMemberFromApplicationStateTests
{
    private readonly Mock<ITeamMemberRepository> teamMemberRepository;
    private readonly ApplicationState applicationState;
    private readonly PresentTeamMemberDetailsUseCase useCase;

    public Handle_TeamMemberFromApplicationStateTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        teamMemberRepository = new Mock<ITeamMemberRepository>();

        unitOfWork
            .Setup(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        applicationState = new ApplicationState();

        useCase = new PresentTeamMemberDetailsUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingTeamMemberIdSpecifiedInApplicationStateButNotExistingInStorage_WhenUseCaseIsExecuted_ThenReturnsNullName()
    {
        teamMemberRepository
            .Setup(x => x.Get(101))
            .ReturnsAsync(null as TeamMember);

        applicationState.SelectedTeamMemberId = 101;

        PresentTeamMemberDetailsRequest request = new();

        PresentTeamMemberDetailsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TeamMemberName.Should().BeNull();
    }

    [Fact]
    public async Task HavingTeamMemberIdSpecifiedInApplicationState_WhenUseCaseIsExecuted_ThenTeamMemberWithSpecifiedIdIsRequestedFromUnitOfWork()
    {
        TeamMember teamMember = new();

        teamMemberRepository
            .Setup(x => x.Get(3568))
            .ReturnsAsync(teamMember);

        applicationState.SelectedTeamMemberId = 3568;

        PresentTeamMemberDetailsRequest request = new();
        _ = await useCase.Handle(request, CancellationToken.None);

        teamMemberRepository.Verify(x => x.Get(3568), Times.Once);
    }

    [Fact]
    public async Task HavingTeamMemberIdSpecifiedInTheApplicationState_WhenUseCaseIsExecuted_ThenNameOfTheSpecifiedTeamMemberIsReturnedInTheResponse()
    {
        TeamMember teamMember = new()
        {
            Name = "name 312"
        };

        teamMemberRepository
            .Setup(x => x.Get(3568))
            .ReturnsAsync(teamMember);

        applicationState.SelectedTeamMemberId = 3568;

        PresentTeamMemberDetailsRequest request = new();
        PresentTeamMemberDetailsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TeamMemberName.Should().Be("name 312");
    }
}