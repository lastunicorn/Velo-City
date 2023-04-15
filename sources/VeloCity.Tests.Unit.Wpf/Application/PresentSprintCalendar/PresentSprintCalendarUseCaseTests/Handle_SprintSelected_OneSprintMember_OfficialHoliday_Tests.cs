// VeloCity
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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentSprintCalendar.PresentSprintCalendarUseCaseTests;

public class Handle_SprintSelected_OneSprintMember_OfficialHoliday_Tests
{
    private readonly Sprint sprintFromRepository;
    private readonly PresentSprintCalendarUseCase useCase;

    public Handle_SprintSelected_OneSprintMember_OfficialHoliday_Tests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        ApplicationState applicationState = new();
        Mock<ISystemClock> systemClock = new();

        applicationState.SelectedSprintId = 97;

        sprintFromRepository = new Sprint();

        sprintRepository
            .Setup(x => x.Get(97))
            .ReturnsAsync(sprintFromRepository);

        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 20), new DateTime(2023, 03, 26));
        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2000, 06, 01),
                    HoursPerDay = 8,
                    EmploymentWeek = EmploymentWeek.NewDefault
                }
            },
            Vacations = new VacationCollection()
        };
        sprintFromRepository.AddSprintMember(teamMember);

        useCase = new PresentSprintCalendarUseCase(unitOfWork.Object, applicationState, systemClock.Object);
    }

    [Fact]
    public async Task HavingOneOfficialHoliday_WhenUseCaseIsExecuted_ThenDaysFromResponseContainCorrectWorkHours()
    {
        OfficialHoliday officialHoliday = new OfficialHolidayOnce
        {
            Date = new DateTime(2023, 03, 21)
        };
        sprintFromRepository.OfficialHolidays.Add(officialHoliday);

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        HoursValue?[] expectedWorkHours = { 8, 0, 8, 8, 8, 0, 0 };
        response.SprintCalendarDays.Select(x => x.WorkHours).Should().Equal(expectedWorkHours);
    }

    [Fact]
    public async Task HavingOneOfficialHoliday_WhenUseCaseIsExecuted_ThenDaysFromResponseContainCorrectAbsenceHours()
    {
        OfficialHoliday officialHoliday = new OfficialHolidayOnce
        {
            Date = new DateTime(2023, 03, 21)
        };
        sprintFromRepository.OfficialHolidays.Add(officialHoliday);

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        HoursValue?[] expectedAbsenceHours = { 0, 8, 0, 0, 0, null, null };
        response.SprintCalendarDays.Select(x => x.AbsenceHours).Should().Equal(expectedAbsenceHours);
    }

    [Fact]
    public async Task HavingOneOfficialHoliday_WhenUseCaseIsExecuted_ThenDaysFromResponseContainCorrectIsWorkDay()
    {
        OfficialHoliday officialHoliday = new OfficialHolidayOnce
        {
            Date = new DateTime(2023, 03, 21)
        };
        sprintFromRepository.OfficialHolidays.Add(officialHoliday);

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        bool[] expectedIsWorkDay = { true, true, true, true, true, false, false };
        response.SprintCalendarDays.Select(x => x.IsWorkDay).Should().Equal(expectedIsWorkDay);
    }

    [Fact]
    public async Task HavingOneOfficialHoliday_WhenUseCaseIsExecuted_ThenDayFromResponseContainOfficialHolidayInAbsenceGroup()
    {
        OfficialHoliday officialHoliday = new OfficialHolidayOnce
        {
            Date = new DateTime(2023, 03, 21),
            Name = "Robonica",
            Country = "NNY",
            ShortDescription = "description"
        };
        sprintFromRepository.OfficialHolidays.Add(officialHoliday);

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        SprintCalendarDay dayWithOfficialHoliday = response.SprintCalendarDays
            .Skip(1)
            .Take(1)
            .Single();

        AbsenceGroup absenceGroup = dayWithOfficialHoliday.AbsenceGroups.Single();
        absenceGroup.OfficialHoliday.HolidayName.Should().Be("Robonica");
        absenceGroup.OfficialHoliday.HolidayCountry.Should().Be("NNY");
        absenceGroup.OfficialHoliday.HolidayDescription.Should().Be("description");
    }
}