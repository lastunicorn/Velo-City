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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintMembers;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprintMembers.PresentSprintMembersUseCaseTests;

public class Handle_SprintMemberDtoPropertiesTests
{
    private readonly PresentSprintMembersUseCase useCase;
    private readonly Sprint sprintFromRepository;

    public Handle_SprintMemberDtoPropertiesTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        ApplicationState applicationState = new()
        {
            SelectedSprintId = 42
        };

        useCase = new PresentSprintMembersUseCase(unitOfWork.Object, applicationState);

        sprintFromRepository = new Sprint();

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .Returns(sprintFromRepository);
    }

    [Fact]
    public async Task HavingOneSprintWithOneSprintMemberInRepository_WhenUseCaseIsExecuted_ThenResponseContainsTeamMemberId()
    {
        TeamMember teamMemberFromRepository = new()
        {
            Id = 326
        };
        sprintFromRepository.AddSprintMember(teamMemberFromRepository);

        PresentSprintMembersRequest request = new();
        PresentSprintMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintMembers[0].TeamMemberId.Should().Be(326);
    }

    [Fact]
    public async Task HavingOneSprintWithOneSprintMemberInRepository_WhenUseCaseIsExecuted_ThenResponseContainsTeamMemberName()
    {
        TeamMember teamMemberFromRepository = new()
        {
            Name = new PersonName
            {
                FirstName = "John",
                LastName = "Wick"
            }
        };
        sprintFromRepository.AddSprintMember(teamMemberFromRepository);

        PresentSprintMembersRequest request = new();
        PresentSprintMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        PersonName expectedName = new()
        {
            FirstName = "John",
            LastName = "Wick"
        };
        response.SprintMembers[0].Name.Should().Be(expectedName);
    }

    [Fact]
    public async Task HavingOneSprintWithOneSprintMemberInRepository_WhenUseCaseIsExecuted_ThenResponseContainsWorkHours()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 13), new DateTime(2023, 03, 26));
        TeamMember teamMemberFromRepository = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    EmploymentWeek = new EmploymentWeek(),
                    HoursPerDay = 4,
                    StartDate = new DateTime(2000, 01, 01)
                }
            }
        };
        sprintFromRepository.AddSprintMember(teamMemberFromRepository);

        PresentSprintMembersRequest request = new();
        PresentSprintMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintMembers[0].WorkHours.Should().Be((HoursValue)40);
    }

    [Fact]
    public async Task HavingOneSprintWithOneSprintMemberInRepository_WhenUseCaseIsExecuted_ThenResponseContainsAbsenceHours()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 13), new DateTime(2023, 03, 26));
        TeamMember teamMemberFromRepository = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    EmploymentWeek = new EmploymentWeek(),
                    HoursPerDay = 4,
                    StartDate = new DateTime(2000, 01, 01)
                }
            },
            Vacations = new VacationCollection
            {
                new VacationDaily
                {
                    HourCount = 4,
                    DateInterval = new DateInterval(new DateTime(2023, 03, 13), new DateTime(2023, 03, 17))
                }
            }
        };
        sprintFromRepository.AddSprintMember(teamMemberFromRepository);

        PresentSprintMembersRequest request = new();
        PresentSprintMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintMembers[0].AbsenceHours.Should().Be((HoursValue)20);
    }

    [Fact]
    public async Task HavingOneSprintWithOneSprintMemberInRepository_WhenUseCaseIsExecuted_ThenResponseContainsSprintId()
    {
        sprintFromRepository.Id = 978;
        TeamMember teamMemberFromRepository = new();
        sprintFromRepository.AddSprintMember(teamMemberFromRepository);

        PresentSprintMembersRequest request = new();
        PresentSprintMembersResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintMembers[0].SprintId.Should().Be(978);
    }
}