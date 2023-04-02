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
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprintCalendar.PresentSprintCalendarUseCaseTests;

public class Handle_SprintSelected_OneSprintMember_NoVacation_Tests
{
    private readonly PresentSprintCalendarUseCase useCase;
    private readonly TeamMember teamMember;

    public Handle_SprintSelected_OneSprintMember_NoVacation_Tests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        ApplicationState applicationState = new();
        Mock<ISystemClock> systemClock = new();

        applicationState.SelectedSprintId = 97;

        Sprint sprintFromRepository = new();

        sprintRepository
            .Setup(x => x.Get(97))
            .ReturnsAsync(sprintFromRepository);

        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 20), new DateTime(2023, 03, 26));
        teamMember = new TeamMember
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

        useCase = new PresentSprintCalendarUseCase(unitOfWork.Object, applicationState, systemClock.Object);
    }

    [Fact]
    public async Task HavingEmploymentWith8Hours_WhenUseCaseIsExecuted_ThenSprintDaysFromResponseContainCorrectWorkHours()
    {
        teamMember.Employments.First().HoursPerDay = 8;

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        HoursValue?[] expectedWorkHours = { 8, 8, 8, 8, 8, 0, 0 };
        response.SprintCalendarDays.Select(x => x.WorkHours).Should().Equal(expectedWorkHours);
    }

    [Fact]
    public async Task HavingEmploymentWith6Hours_WhenUseCaseIsExecuted_ThenSprintDaysFromResponseContainCorrectWorkHours()
    {
        teamMember.Employments.First().HoursPerDay = 6;

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        HoursValue?[] expectedWorkHours = { 6, 6, 6, 6, 6, 0, 0 };
        response.SprintCalendarDays.Select(x => x.WorkHours).Should().Equal(expectedWorkHours);
    }

    [Fact]
    public async Task HavingEmploymentWith8Hours_WhenUseCaseIsExecuted_ThenSprintDaysFromResponseContainCorrectAbsenceHours()
    {
        teamMember.Employments.First().HoursPerDay = 8;

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        HoursValue?[] expectedAbsenceHours = { 0, 0, 0, 0, 0, null, null };
        response.SprintCalendarDays.Select(x => x.AbsenceHours).Should().Equal(expectedAbsenceHours);
    }

    [Fact]
    public async Task HavingEmploymentWith8Hours_WhenUseCaseIsExecuted_ThenSprintDaysFromResponseContainCorrectIsWorkDayFlag()
    {
        teamMember.Employments.First().HoursPerDay = 8;

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        bool[] expectedIsWorkDay = { true, true, true, true, true, false, false };
        response.SprintCalendarDays.Select(x => x.IsWorkDay).Should().Equal(expectedIsWorkDay);
    }
}