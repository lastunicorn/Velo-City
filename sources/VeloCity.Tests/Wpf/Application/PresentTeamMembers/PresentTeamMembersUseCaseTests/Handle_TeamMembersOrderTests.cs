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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMembers;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentTeamMembers.PresentTeamMembersUseCaseTests;

public class Handle_TeamMembersOrderTests
{
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly Mock<ITeamMemberRepository> teamMemberRepository;
    private readonly ApplicationState applicationState;
    private readonly PresentTeamMembersUseCase useCase;

    public Handle_TeamMembersOrderTests()
    {
        unitOfWork = new Mock<IUnitOfWork>();
        teamMemberRepository = new Mock<ITeamMemberRepository>();

        unitOfWork
            .SetupGet(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        applicationState = new ApplicationState();

        useCase = new PresentTeamMembersUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingTwoEmployedTeamMembersOutOfOrderInRepository_WhenUseCaseIsExecuted_ThenReturnsTheTwoTeamMembersOrderedAscendingByEmploymentStartDate()
    {
        List<TeamMember> teamMembersFromRepository = new()
        {
            new TeamMember
            {
                Id = 1,
                Employments = new EmploymentCollection
                {
                    new Employment
                    {
                        StartDate = new DateTime(2020, 03, 12)
                    }
                }
            },
            new TeamMember
            {
                Id = 2,
                Employments = new EmploymentCollection
                {
                    new Employment
                    {
                        StartDate = new DateTime(2019, 01, 29)
                    }
                }
            }
        };

        await PerformTestsAndAssertTheOrder(teamMembersFromRepository, new[] { 2, 1 });
    }

    [Fact]
    public async Task HavingTwoUnemployedTeamMembersOutOfOrderInRepository_WhenUseCaseIsExecuted_ThenReturnsTheTwoTeamMembersOrderedDescendingByEmploymentEndDate()
    {
        List<TeamMember> teamMembersFromRepository = new()
        {
            new TeamMember
            {
                Id = 1,
                Employments = new EmploymentCollection
                {
                    new Employment
                    {
                        StartDate = new DateTime(2019, 01, 29),
                        EndDate = new DateTime(2019, 07, 11)
                    }
                }
            },
            new TeamMember
            {
                Id = 2,
                Employments = new EmploymentCollection
                {
                    new Employment
                    {
                        StartDate = new DateTime(2020, 03, 12),
                        EndDate = new DateTime(2021, 06, 25)
                    }
                }
            }
        };

        await PerformTestsAndAssertTheOrder(teamMembersFromRepository, new[] { 2, 1 });
    }

    [Fact]
    public async Task HavingOneUnemployedAndOneEmployedTeamMemberInRepository_WhenUseCaseIsExecuted_ThenReturnsTheTwoTeamMembersOrderedHavingTheEmployedOneFirst()
    {
        List<TeamMember> teamMembersFromRepository = new()
        {
            new TeamMember
            {
                Id = 1,
                Employments = new EmploymentCollection
                {
                    new Employment
                    {
                        StartDate = new DateTime(2019, 01, 29),
                        EndDate = new DateTime(2019, 07, 11)
                    }
                }
            },
            new TeamMember
            {
                Id = 2,
                Employments = new EmploymentCollection
                {
                    new Employment
                    {
                        StartDate = new DateTime(2020, 03, 12)
                    }
                }
            }
        };

        await PerformTestsAndAssertTheOrder(teamMembersFromRepository, new[] { 2, 1 });
    }

    private async Task PerformTestsAndAssertTheOrder(List<TeamMember> teamMembersFromRepository, int[] expectedTeamMemberIds)
    {
        teamMemberRepository
            .Setup(x => x.GetAll())
            .Returns(teamMembersFromRepository);

        PresentTeamMembersRequest request = new();
        PresentTeamMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        List<int> actualTeamMemberIds = response.TeamMembers
            .Select(x => x.Id)
            .ToList();
        actualTeamMemberIds.Should().Equal(expectedTeamMemberIds);
    }
}