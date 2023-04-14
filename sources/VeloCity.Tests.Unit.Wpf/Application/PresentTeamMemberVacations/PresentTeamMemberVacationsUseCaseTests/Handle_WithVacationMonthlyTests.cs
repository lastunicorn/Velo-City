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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberVacations;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentTeamMemberVacations.PresentTeamMemberVacationsUseCaseTests;

public class Handle_WithVacationMonthlyTests
{
    private readonly PresentTeamMemberVacationsUseCase useCase;
    private readonly MonthlyVacation vacation;

    public Handle_WithVacationMonthlyTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ITeamMemberRepository> teamMemberRepository = new();

        unitOfWork
            .Setup(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        ApplicationState applicationState = new()
        {
            SelectedTeamMemberId = 123
        };

        vacation = new MonthlyVacation();

        TeamMember teamMember = new()
        {
            Vacations = new VacationCollection { vacation }
        };

        teamMemberRepository
            .Setup(x => x.Get(123))
            .ReturnsAsync(teamMember);

        useCase = new PresentTeamMemberVacationsUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingVacationWithSpecificDateIntervalInRepository_WhenUseCaseIsExecuted_ThenResponseContainsVacationWithSameDateInterval()
    {
        vacation.DateInterval = new DateInterval(new DateTime(2023, 01, 04), new DateTime(2023, 01, 14));

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        VacationMonthlyInfo vacationMonthlyInfo = response.Vacations.First() as VacationMonthlyInfo;
        DateInterval expectedDateInterval = new(new DateTime(2023, 01, 04), new DateTime(2023, 01, 14));
        vacationMonthlyInfo.DateInterval.Should().Be(expectedDateInterval);
    }

    [Fact]
    public async Task HavingVacationWithSpecificMonthDaysInRepository_WhenUseCaseIsExecuted_ThenResponseContainsVacationWithSameMonthDays()
    {
        vacation.MonthDays = new List<int> { 4, 7, 9 };

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        VacationMonthlyInfo vacationMonthlyInfo = response.Vacations.First() as VacationMonthlyInfo;
        int[] expectedMonthDays = { 4, 7, 9 };
        vacationMonthlyInfo.MonthDays.Should().Equal(expectedMonthDays);
    }

    [Fact]
    public async Task HavingVacationWithSpecificHoursCountInRepository_WhenUseCaseIsExecuted_ThenResponseContainsVacationWithSameHoursCount()
    {
        vacation.HourCount = 23;

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        VacationMonthlyInfo vacationMonthlyInfo = response.Vacations.First() as VacationMonthlyInfo;
        vacationMonthlyInfo.HourCount.Should().Be(23);
    }

    [Fact]
    public async Task HavingVacationWithSpecificCommentsInRepository_WhenUseCaseIsExecuted_ThenResponseContainsVacationWithSameComments()
    {
        vacation.Comments = "hihihi";

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        VacationMonthlyInfo vacationMonthlyInfo = response.Vacations.First() as VacationMonthlyInfo;
        vacationMonthlyInfo.Comments.Should().Be("hihihi");
    }
}