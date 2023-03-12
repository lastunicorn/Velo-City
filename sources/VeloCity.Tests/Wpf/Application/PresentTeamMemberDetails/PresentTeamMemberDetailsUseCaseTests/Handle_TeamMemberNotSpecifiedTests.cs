﻿// VeloCity
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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintDetails;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberDetails;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentTeamMemberDetails.PresentTeamMemberDetailsUseCaseTests
{
    public class Handle_TeamMemberNotSpecifiedTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<ITeamMemberRepository> teamMemberRepository;
        private readonly ApplicationState applicationState;
        private readonly PresentTeamMemberDetailsUseCase useCase;

        public Handle_TeamMemberNotSpecifiedTests()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            teamMemberRepository = new Mock<ITeamMemberRepository>();

            unitOfWork
                .Setup(x => x.TeamMemberRepository)
                .Returns(teamMemberRepository.Object);

            applicationState = new ApplicationState();

            useCase = new PresentTeamMemberDetailsUseCase(unitOfWork.Object, applicationState);
        }

        [Fact]
        public async Task HavingNoTeamMemberIdSpecified_WhenUseCaseIsExecuted_ThenNoTeamMemberIsRequestedFromUnitOfWork()
        {
            PresentTeamMemberDetailsRequest request = new();

            PresentTeamMemberDetailsResponse response = await useCase.Handle(request, CancellationToken.None);

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
}