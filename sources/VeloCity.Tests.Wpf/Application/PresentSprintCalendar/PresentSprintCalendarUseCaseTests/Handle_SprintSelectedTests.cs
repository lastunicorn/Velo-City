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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprintCalendar.PresentSprintCalendarUseCaseTests;

public class Handle_SprintSelectedTests
{
    private readonly Sprint sprintFromRepository;
    private readonly PresentSprintCalendarUseCase useCase;
    private readonly Mock<ISystemClock> systemClock;

    public Handle_SprintSelectedTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        ApplicationState applicationState = new();
        systemClock = new Mock<ISystemClock>();

        applicationState.SelectedSprintId = 97;

        sprintFromRepository = new Sprint();

        sprintRepository
            .Setup(x => x.Get(97))
            .ReturnsAsync(sprintFromRepository);

        useCase = new PresentSprintCalendarUseCase(unitOfWork.Object, applicationState, systemClock.Object);
    }

    [Fact]
    public async Task HavingInProgressSprintWith7DaysInRepository_WhenUseCaseIsExecuted_ThenResponseContains7SprintDays()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 20), new DateTime(2023, 03, 26));

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintCalendarDays.Should().HaveCount(7);
    }

    [Fact]
    public async Task HavingSystemClockDaySetOnSecondDayOfSprint_WhenUseCaseIsExecuted_ThenResponseContainsSecondSprintDaysAsBeingCurrent()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 20), new DateTime(2023, 03, 26));
        systemClock
            .Setup(x => x.Today)
            .Returns(new DateTime(2023, 03, 21));

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        bool[] expectedIsCurrent = { false, true, false, false, false, false, false };
        response.SprintCalendarDays.Select(x => x.IsCurrentDay).Should().Equal(expectedIsCurrent);
    }

    [Fact]
    public async Task HavingOneTeamMember_WhenUseCaseIsExecuted_ThenSprintDaysFromResponseContainTheTeamMemberHours()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 20), new DateTime(2023, 03, 26));
        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2000, 06, 01),
                    HoursPerDay = 8,
                    EmploymentWeek = new EmploymentWeek()
                }
            }
        };
        sprintFromRepository.AddSprintMember(teamMember);

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        HoursValue?[] expectedWorkHours = { 8, 8, 8, 8, 8, 0, 0 };
        response.SprintCalendarDays.Select(x => x.WorkHours).Should().Equal(expectedWorkHours);
    }
}