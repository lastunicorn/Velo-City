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
using System.Linq;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.VacationCollectionTests;

public class AddForDateTests
{
    private readonly VacationCollection vacationCollection;
    private readonly DateTime firstDay;
    private readonly DateTime secondDay;

    public AddForDateTests()
    {
        firstDay = new DateTime(2023, 03, 27);
        secondDay = new DateTime(2023, 03, 28);

        vacationCollection = new VacationCollection();
    }

    [Fact]
    public void HavingVacationOf4HoursForOneDayInCollection_WhenAddingVacationOf8HoursForNextDay_ThenFirstDayContainsVacationOf4Hours()
    {
        VacationOnce vacation1 = new()
        {
            Date = firstDay,
            HourCount = 4
        };
        vacationCollection.Add(vacation1);

        vacationCollection.AddForDate(secondDay);

        Vacation actualVacation = vacationCollection.GetVacationsFor(firstDay).Single();
        actualVacation.HourCount.Should().Be(4);
    }

    [Fact]
    public void HavingVacationOf4HoursForOneDayInCollection_WhenAddingFullDayVacationForNextDay_ThenSecondDayContainsVacationHoursNull()
    {
        VacationOnce vacation1 = new()
        {
            Date = firstDay,
            HourCount = 4
        };
        vacationCollection.Add(vacation1);

        vacationCollection.AddForDate(secondDay);

        Vacation actualVacation = vacationCollection.GetVacationsFor(secondDay).Single();
        actualVacation.HourCount.Should().BeNull();
    }
}