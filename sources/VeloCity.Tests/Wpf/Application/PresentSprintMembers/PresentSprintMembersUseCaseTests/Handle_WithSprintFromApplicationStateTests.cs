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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintMembers;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprintMembers.PresentSprintMembersUseCaseTests;

public class Handle_WithSprintFromApplicationStateTests
{
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly PresentSprintMembersUseCase useCase;

    public Handle_WithSprintFromApplicationStateTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        ApplicationState applicationState = new()
        {
            SelectedSprintId = 42
        };

        useCase = new PresentSprintMembersUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingSprintSpecifiedInApplicationState_WhenUseCaseIsExecuted_ThenThatSprintIsRetrievedFromRepository()
    {
        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(new Sprint());

        PresentSprintMembersRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintRepository.Verify(x => x.Get(42), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintInRepository_WhenUseCaseIsExecuted_ThenThrows()
    {
        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(null as Sprint);

        Func<Task> action = async () =>
        {
            PresentSprintMembersRequest request = new();
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<SprintDoesNotExistException>();
    }

    [Fact]
    public async Task HavingSprintWithOneMemberInRepository_WhenUseCaseIsExecuted_ThenResponseContainsThatSprintMember()
    {
        Sprint sprintFromRepository = new();
        TeamMember teamMemberFromRepository = new()
        {
            Id = 2
        };
        sprintFromRepository.AddSprintMember(teamMemberFromRepository);

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        PresentSprintMembersRequest request = new();
        PresentSprintMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        IEnumerable<int> actualSprintMemberIds = response.SprintMembers
            .Select(x => x.TeamMemberId);

        actualSprintMemberIds.Should().BeEquivalentTo(new[] { 2 });
    }

    [Fact]
    public async Task HavingSprintWithTwoMembersInRepositoryInAscendingOrderByEmploymentDate_WhenUseCaseIsExecuted_ThenResponseContainsBothSprintMembersInThatOrder()
    {
        Sprint sprintFromRepository = new();

        TeamMember teamMember1FromRepository = new()
        {
            Id = 2,
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2022, 07, 01)
                }
            }
        };
        TeamMember teamMember2FromRepository = new()
        {
            Id = 47,
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2022, 09, 01)
                }
            }
        };

        sprintFromRepository.AddSprintMember(teamMember1FromRepository);
        sprintFromRepository.AddSprintMember(teamMember2FromRepository);

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        PresentSprintMembersRequest request = new();
        PresentSprintMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        IEnumerable<int> actualSprintMemberIds = response.SprintMembers
            .Select(x => x.TeamMemberId);

        actualSprintMemberIds.Should().Equal(2, 47);
    }

    [Fact]
    public async Task HavingSprintWithTwoMembersInRepositoryInDescendingOrderByEmploymentDate_WhenUseCaseIsExecuted_ThenResponseContainsBothSprintMembersInReversedOrder()
    {
        Sprint sprintFromRepository = new();

        TeamMember teamMember1FromRepository = new()
        {
            Id = 2,
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2022, 09, 01)
                }
            }
        };
        TeamMember teamMember2FromRepository = new()
        {
            Id = 47,
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2022, 07, 01)
                }
            }
        };

        sprintFromRepository.AddSprintMember(teamMember1FromRepository);
        sprintFromRepository.AddSprintMember(teamMember2FromRepository);

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        PresentSprintMembersRequest request = new();
        PresentSprintMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        IEnumerable<int> actualSprintMemberIds = response.SprintMembers
            .Select(x => x.TeamMemberId);

        actualSprintMemberIds.Should().Equal(47, 2);
    }
}