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
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentSprintCalendar.PresentSprintCalendarUseCaseTests;

public class Handle_SprintSelected_DateList_Tests
{
    private readonly Sprint sprintFromRepository;
    private readonly PresentSprintCalendarUseCase useCase;

    public Handle_SprintSelected_DateList_Tests()
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

        useCase = new PresentSprintCalendarUseCase(unitOfWork.Object, applicationState, systemClock.Object);
    }

    [Fact]
    public async Task HavingSprintWithOneDayInRepository_WhenUseCaseIsExecuted_ThenResponseContainsOnSprintDay()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 20), new DateTime(2023, 03, 20));

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        DateTime[] expectedDateTimes =
        {
            new(2023, 03, 20)
        };
        response.SprintCalendarDays.Select(x => x.Date).Should().Equal(expectedDateTimes);
    }

    [Fact]
    public async Task HavingSprintWith7DaysInRepository_WhenUseCaseIsExecuted_ThenResponseContainsThose7SprintDays()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 20), new DateTime(2023, 03, 26));

        PresentSprintCalendarRequest request = new();
        PresentSprintCalendarResponse response = await useCase.Handle(request, CancellationToken.None);

        DateTime[] expectedDateTimes =
        {
            new(2023, 03, 20),
            new(2023, 03, 21),
            new(2023, 03, 22),
            new(2023, 03, 23),
            new(2023, 03, 24),
            new(2023, 03, 25),
            new(2023, 03, 26)
        };
        response.SprintCalendarDays.Select(x => x.Date).Should().Equal(expectedDateTimes);
    }
}