﻿// VeloCity
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

using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberDetails;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentTeamMemberDetails.PresentTeamMemberDetailsUseCaseTests;

public class Handle_TeamMemberNotSpecifiedTests
{
    private readonly Mock<ITeamMemberRepository> teamMemberRepository;
    private readonly PresentTeamMemberDetailsUseCase useCase;

    public Handle_TeamMemberNotSpecifiedTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        teamMemberRepository = new Mock<ITeamMemberRepository>();

        unitOfWork
            .Setup(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        ApplicationState applicationState = new();

        useCase = new PresentTeamMemberDetailsUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingNoTeamMemberIdSpecified_WhenUseCaseIsExecuted_ThenNoTeamMemberIsRequestedFromUnitOfWork()
    {
        PresentTeamMemberDetailsRequest request = new();

        _ = await useCase.Handle(request, CancellationToken.None);

        teamMemberRepository.Verify(x => x.Get(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task HavingNoTeamMemberIdSpecified_WhenUseCaseIsExecuted_ThenNoTeamMemberNameIsReturnedInTheResponse()
    {
        PresentTeamMemberDetailsRequest request = new();

        PresentTeamMemberDetailsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TeamMemberName.Should().BeNull();
    }
}