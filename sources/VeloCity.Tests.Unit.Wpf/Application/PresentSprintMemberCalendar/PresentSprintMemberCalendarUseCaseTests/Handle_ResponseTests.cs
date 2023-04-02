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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintMemberCalendar;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentSprintMemberCalendar.PresentSprintMemberCalendarUseCaseTests;

public class Handle_ResponseTests
{
    private readonly PresentSprintMemberCalendarUseCase useCase;
    private readonly Sprint sprintFromRepository;

    public Handle_ResponseTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        Mock<ISystemClock> systemClock = new();

        sprintFromRepository = new Sprint
        {
            Id = 5
        };

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        useCase = new PresentSprintMemberCalendarUseCase(unitOfWork.Object, systemClock.Object);
    }

    [Fact]
    public async Task HavingTeamMemberInSprint_WhenUseCaseIsExecuted_ThenResponseContainsTeamMemberId()
    {
        TeamMember teamMemberFromSprint = new()
        {
            Id = 10
        };
        sprintFromRepository.AddSprintMember(teamMemberFromSprint);

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TeamMemberId.Should().Be(10);
    }

    [Fact]
    public async Task HavingTeamMemberInSprint_WhenUseCaseIsExecuted_ThenResponseContainsTeamMemberName()
    {
        TeamMember teamMemberFromSprint = new()
        {
            Id = 10,
            Name = new PersonName
            {
                FirstName = "Matilda",
                LastName = "Smith"
            }
        };
        sprintFromRepository.AddSprintMember(teamMemberFromSprint);

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TeamMemberId.Should().Be(10);
    }

    [Fact]
    public async Task HavingTeamMemberInSprint_WhenUseCaseIsExecuted_ThenResponseContainsSprintId()
    {
        TeamMember teamMemberFromSprint = new()
        {
            Id = 10
        };
        sprintFromRepository.AddSprintMember(teamMemberFromSprint);

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintId.Should().Be(5);
    }

    [Fact]
    public async Task HavingTeamMemberInSprint_WhenUseCaseIsExecuted_ThenResponseContainsSprintNumber()
    {
        TeamMember teamMemberFromSprint = new()
        {
            Id = 10
        };
        sprintFromRepository.AddSprintMember(teamMemberFromSprint);
        sprintFromRepository.Number = 123;

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintNumber.Should().Be(123);
    }

    [Fact]
    public async Task HavingTeamMemberInSprint_WhenUseCaseIsExecuted_ThenResponseContainsSprintMemberDays()
    {
        TeamMember teamMemberFromSprint = new()
        {
            Id = 10
        };
        sprintFromRepository.AddSprintMember(teamMemberFromSprint);
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 20), new DateTime(2023, 03, 26));

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Days.Count.Should().Be(7);
    }
}