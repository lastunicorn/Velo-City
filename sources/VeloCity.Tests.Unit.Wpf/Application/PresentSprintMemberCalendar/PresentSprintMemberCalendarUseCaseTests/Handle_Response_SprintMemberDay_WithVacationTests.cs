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

public class Handle_Response_SprintMemberDay_WithVacationTests
{
    private readonly PresentSprintMemberCalendarUseCase useCase;
    private readonly Mock<ISystemClock> systemClock;
    private readonly TeamMember teamMemberFromRepository;

    public Handle_Response_SprintMemberDay_WithVacationTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        systemClock = new Mock<ISystemClock>();

        Sprint sprintFromRepository = new()
        {
            Id = 5,
            DateInterval = new DateInterval(new DateTime(2023, 03, 20), new DateTime(2023, 03, 26))
        };

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprintFromRepository);

        teamMemberFromRepository = new TeamMember
        {
            Id = 10,
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2000, 01, 01),
                    EmploymentWeek = EmploymentWeek.NewDefault,
                    HoursPerDay = 8
                }
            }
        };

        sprintFromRepository.AddSprintMember(teamMemberFromRepository);

        useCase = new PresentSprintMemberCalendarUseCase(unitOfWork.Object, systemClock.Object);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayFullVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsTeamMemberId()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21)
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Days.Should().AllSatisfy(x =>
        {
            x.TeamMemberId.Should().Be(10);
        });
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayFullVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsTheDate()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21)
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        DateTime[] expectedDates =
        {
            new(2023, 03, 20),
            new(2023, 03, 21),
            new(2023, 03, 22),
            new(2023, 03, 23),
            new(2023, 03, 24),
            new(2023, 03, 25),
            new(2023, 03, 26)
        };
        response.Days.Select(x => x.Date).Should().Equal(expectedDates);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayFullVacationAndSystemClockSetForSecondDayOfTheSprint_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsTheIsCurrentDayFlag()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21)
            }
        };

        systemClock
            .Setup(x => x.Today)
            .Returns(new DateTime(2023, 03, 21));

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        bool[] expectedDates =
        {
            false,
            true,
            false,
            false,
            false,
            false,
            false
        };
        response.Days.Select(x => x.IsCurrentDay).Should().Equal(expectedDates);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayFullVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsTheIsWorkDayFlag()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21)
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        bool[] expectedDates =
        {
            true,
            true,
            true,
            true,
            true,
            false,
            false
        };
        response.Days.Select(x => x.IsWorkDay).Should().Equal(expectedDates);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayFullVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsCorrectAbsenceHours()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21)
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        HoursValue?[] expectedDates =
        {
            new(0),
            new(8),
            new(0),
            new(0),
            new(0),
            null,
            null
        };
        response.Days.Select(x => x.AbsenceHours).Should().Equal(expectedDates);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayPartialVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsCorrectAbsenceHours()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21),
                HourCount = 4
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        HoursValue?[] expectedDates =
        {
            new(0),
            new(4),
            new(0),
            new(0),
            new(0),
            null,
            null
        };
        response.Days.Select(x => x.AbsenceHours).Should().Equal(expectedDates);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayPartialVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsCorrectAbsenceReason()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21),
                HourCount = 4
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        AbsenceReason[] expectedDates =
        {
            AbsenceReason.None,
            AbsenceReason.Vacation,
            AbsenceReason.None,
            AbsenceReason.None,
            AbsenceReason.None,
            AbsenceReason.WeekEnd,
            AbsenceReason.WeekEnd
        };
        response.Days.Select(x => x.AbsenceReason).Should().Equal(expectedDates);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayFullVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsCorrectAbsenceComments()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21)
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        HoursValue?[] expectedDates =
        {
            new(0),
            new(8),
            new(0),
            new(0),
            new(0),
            null,
            null
        };
        response.Days.Select(x => x.AbsenceHours).Should().Equal(expectedDates);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayFullVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsCanAddVacationFlagTrue()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21)
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        bool[] expected =
        {
            true,
            false,
            true,
            true,
            true,
            false,
            false
        };
        response.Days.Select(x => x.CanAddVacation).Should().Equal(expected);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayPartialVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsCanAddVacationFlagTrue()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21),
                HourCount = 4
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        bool[] expected =
        {
            true,
            true,
            true,
            true,
            true,
            false,
            false
        };
        response.Days.Select(x => x.CanAddVacation).Should().Equal(expected);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayFullVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsCanRemoveVacationFlagFalse()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21)
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        bool[] expected =
        {
            false,
            true,
            false,
            false,
            false,
            false,
            false
        };
        response.Days.Select(x => x.CanRemoveVacation).Should().Equal(expected);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneDayPartialVacation_WhenUseCaseIsExecuted_ThenEachDayFromResponseContainsCanRemoveVacationFlagFalse()
    {
        teamMemberFromRepository.Vacations = new VacationCollection
        {
            new SingleDayVacation
            {
                Date = new DateTime(2023, 03, 21),
                HourCount = 4
            }
        };

        PresentSprintMemberCalendarRequest request = new()
        {
            SprintId = 5,
            TeamMemberId = 10
        };
        PresentSprintMemberCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        bool[] expected =
        {
            false,
            true,
            false,
            false,
            false,
            false,
            false
        };
        response.Days.Select(x => x.CanRemoveVacation).Should().Equal(expected);
    }
}