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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.DailyVacationTests;

public class CountDaysBefore_FiniteVacationInterval_Tests
{
    private readonly DailyVacation dailyVacation;

    public CountDaysBefore_FiniteVacationInterval_Tests()
    {
        dailyVacation = new DailyVacation
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 03), new DateTime(2023, 04, 09))
        };
    }

    [Fact]
    public void Having7DayVacationAndReferenceDateAsMinDateTime_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = DateTime.MinValue;

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void Having7DayVacationAndReferenceDateWayBeforeStart_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = new(2023, 01, 01);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void Having7DayVacationAndReferenceDateAsFirstDayBeforeStart_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = new(2023, 04, 02);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void Having7DayVacationAndReferenceDateAsFirstVacationDate_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = new(2023, 04, 03);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void Having7DayVacationAndReferenceDateAsSecondDayOfVacation_WhenCountingDays_ReturnsOne()
    {
        DateTime referenceDate = new(2023, 04, 04);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(1);
    }

    [Fact]
    public void Having7DayVacationAndReferenceDateAsLastDayOfVacation_WhenCountingDays_ReturnsSix()
    {
        DateTime referenceDate = new(2023, 04, 09);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(6);
    }

    [Fact]
    public void Having7DayVacationAndReferenceDateAsFirstDayAfterVacation_WhenCountingDays_ReturnsSeven()
    {
        DateTime referenceDate = new(2023, 04, 10);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(7);
    }

    [Fact]
    public void Having7DayVacationAndReferenceDateWayAfterVacation_WhenCountingDays_ReturnsSeven()
    {
        DateTime referenceDate = new(2023, 10, 01);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(7);
    }

    [Fact]
    public void Having7DayVacationAndReferenceDateAsMaxDateTime_WhenCountingDays_ReturnsSeven()
    {
        DateTime referenceDate = DateTime.MaxValue;

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(7);
    }
}