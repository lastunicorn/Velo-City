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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberVacations;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentTeamMemberVacations.PresentTeamMemberVacationsUseCaseTests;

public class Handle_WithVacationOnceTests
{
    private readonly PresentTeamMemberVacationsUseCase useCase;
    private readonly SingleDayVacation singleDayVacation;

    public Handle_WithVacationOnceTests()
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

        singleDayVacation = new SingleDayVacation();

        TeamMember teamMember = new()
        {
            Vacations = new VacationCollection { singleDayVacation }
        };

        teamMemberRepository
            .Setup(x => x.Get(123))
            .ReturnsAsync(teamMember);

        useCase = new PresentTeamMemberVacationsUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingVacationWithSpecificDateInRepository_WhenUseCaseIsExecuted_ThenResponseContainsVacationWithSameDate()
    {
        singleDayVacation.Date = new DateTime(2023, 01, 04);

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        VacationOnceInfo vacationOnce = response.Vacations.First() as VacationOnceInfo;
        vacationOnce.Date.Should().Be(new DateTime(2023, 01, 04));
    }

    [Fact]
    public async Task HavingVacationWithSpecificHourCountInRepository_WhenUseCaseIsExecuted_ThenResponseContainsVacationWithSameHourCount()
    {
        singleDayVacation.HourCount = 20;

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        VacationOnceInfo vacationOnce = response.Vacations.First() as VacationOnceInfo;
        vacationOnce.HourCount.Should().Be(20);
    }

    [Fact]
    public async Task HavingVacationWithSpecificCommentsInRepository_WhenUseCaseIsExecuted_ThenResponseContainsVacationWithSameComments()
    {
        singleDayVacation.Comments = "some text";

        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

        VacationOnceInfo vacationOnce = response.Vacations.First() as VacationOnceInfo;
        vacationOnce.Comments.Should().Be("some text");
    }
}