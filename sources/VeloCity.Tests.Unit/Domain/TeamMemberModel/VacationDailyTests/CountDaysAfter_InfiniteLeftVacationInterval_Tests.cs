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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.VacationDailyTests;

public class CountDaysAfter_InfiniteLeftVacationInterval_Tests
{
    private readonly VacationDaily vacationDaily;

    public CountDaysAfter_InfiniteLeftVacationInterval_Tests()
    {
        vacationDaily = new VacationDaily
        {
            DateInterval = new DateInterval(null, new DateTime(2023, 04, 09))
        };
    }

    [Fact]
    public void HavingReferenceDateAsMinDateTime_WhenCountingDays_ReturnsBigNumber()
    {
        DateTime referenceDate = DateTime.MinValue;

        uint actual = vacationDaily.CountDaysAfter(referenceDate);

        actual.Should().Be(738618u);
    }

    [Fact]
    public void HavingReferenceDateDuringVacation_WhenCountingDays_ReturnsBigNumber()
    {
        DateTime referenceDate = new(2000, 01, 01);

        uint actual = vacationDaily.CountDaysAfter(referenceDate);

        actual.Should().Be(8499u);
    }

    [Fact]
    public void HavingReferenceDateAsLastDayOfVacation_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = new(2023, 04, 09);

        uint actual = vacationDaily.CountDaysAfter(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingReferenceDateAsFirstDayAfterVacation_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = new(2023, 04, 10);

        uint actual = vacationDaily.CountDaysAfter(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingReferenceDateWayAfterVacation_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = new(2023, 10, 01);

        uint actual = vacationDaily.CountDaysAfter(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingReferenceDateAsMaxDateTime_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = DateTime.MaxValue;

        uint actual = vacationDaily.CountDaysAfter(referenceDate);

        actual.Should().Be(0);
    }
}