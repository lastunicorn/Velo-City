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
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.UpdateVacationHours.UpdateVacationHoursUseCaseTests;

public class Handle_EmploymentPositiveVacationTests
{
    private readonly UpdateVacationHoursUseCase useCase;
    private readonly TeamMember teamMember;
    private readonly EventBus eventBus;

    public Handle_EmploymentPositiveVacationTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ITeamMemberRepository> teamMemberRepository = new();

        unitOfWork
            .Setup(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        teamMember = new TeamMember
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2000, 01, 01),
                    HoursPerDay = 6
                }
            },
            Vacations = new VacationCollection
            {
                new SingleDayVacation
                {
                    Date = new DateTime(2023, 03, 26),
                    HourCount = 5
                }
            }
        };

        teamMemberRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(teamMember);

        eventBus = new EventBus();

        useCase = new UpdateVacationHoursUseCase(unitOfWork.Object, eventBus);
    }

    [Fact]
    public async Task HavingTeamMemberWithPositiveVacationAndRequestWithZeroHours_WhenExecutingTheUseCase_ThenVacationIsRemoved()
    {
        UpdateVacationHoursRequest request = new()
        {
            Date = new DateTime(2023, 03, 26),
            Hours = 0
        };

        await useCase.Handle(request, CancellationToken.None);

        teamMember.Vacations?.GetVacationsFor(new DateTime(2023, 03, 26)).Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task HavingTeamMemberWithPositiveVacationAndRequestWithZeroHours_WhenExecutingTheUseCase_ThenTeamMemberVacationChangedEventIsPublished()
    {
        EventBusClient<TeamMemberVacationChangedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<TeamMemberVacationChangedEvent>();

        UpdateVacationHoursRequest request = new()
        {
            Date = new DateTime(2023, 03, 26),
            Hours = 0
        };

        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.EventWasTriggered.Should().BeTrue();
    }

    [Fact]
    public async Task HavingTeamMemberWithPositiveVacationAndRequestWithPositiveHours_WhenExecutingTheUseCase_ThenVacationIsUpdated()
    {
        UpdateVacationHoursRequest request = new()
        {
            Date = new DateTime(2023, 03, 26),
            Hours = 2
        };

        await useCase.Handle(request, CancellationToken.None);

        Vacation actualVacation = teamMember.Vacations.GetVacationsFor(new DateTime(2023, 03, 26)).Single();
        actualVacation.HourCount.Should().Be(2);
    }

    [Fact]
    public async Task HavingTeamMemberWithPositiveVacationAndRequestWithPositiveHours_WhenExecutingTheUseCase_ThenTeamMemberVacationChangedEventIsPublished()
    {
        EventBusClient<TeamMemberVacationChangedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<TeamMemberVacationChangedEvent>();

        UpdateVacationHoursRequest request = new()
        {
            Date = new DateTime(2023, 03, 26),
            Hours = 2
        };

        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.EventWasTriggered.Should().BeTrue();
    }

    [Fact]
    public async Task HavingTeamMemberWithPositiveVacationAndRequestWithNegativeHours_WhenExecutingTheUseCase_ThenVacationIsRemoved()
    {
        UpdateVacationHoursRequest request = new()
        {
            Date = new DateTime(2023, 03, 26),
            Hours = -3
        };

        await useCase.Handle(request, CancellationToken.None);

        teamMember.Vacations?.GetVacationsFor(new DateTime(2023, 03, 26)).Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task HavingTeamMemberWithPositiveVacationAndRequestWithNegativeHours_WhenExecutingTheUseCase_ThenTeamMemberVacationChangedEventIsPublished()
    {
        EventBusClient<TeamMemberVacationChangedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<TeamMemberVacationChangedEvent>();

        UpdateVacationHoursRequest request = new()
        {
            Date = new DateTime(2023, 03, 26),
            Hours = -3
        };

        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.EventWasTriggered.Should().BeTrue();
    }
}