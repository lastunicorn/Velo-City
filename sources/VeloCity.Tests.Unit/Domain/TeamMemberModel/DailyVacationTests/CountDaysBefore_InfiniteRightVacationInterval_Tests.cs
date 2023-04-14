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

public class CountDaysBefore_InfiniteRightVacationInterval_Tests
{
    private readonly DailyVacation dailyVacation;

    public CountDaysBefore_InfiniteRightVacationInterval_Tests()
    {
        dailyVacation = new DailyVacation
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 03))
        };
    }

    [Fact]
    public void HavingReferenceDateAsMinDateTime_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = DateTime.MinValue;

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingReferenceDateWayBeforeStart_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = new(2023, 01, 01);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingReferenceDateAsFirstDayBeforeStart_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = new(2023, 04, 02);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingReferenceDateAsFirstVacationDate_WhenCountingDays_ReturnsZero()
    {
        DateTime referenceDate = new(2023, 04, 03);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingReferenceDateAsSecondDayOfVacation_WhenCountingDays_ReturnsOne()
    {
        DateTime referenceDate = new(2023, 04, 04);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(1);
    }

    [Fact]
    public void HavingReferenceDateAsTenthDayOfVacation_WhenCountingDays_ReturnsNine()
    {
        DateTime referenceDate = new(2023, 04, 12);

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(9);
    }

    [Fact]
    public void HavingReferenceDateAsMaxDateTime_WhenCountingDays_ReturnsBigNumber()
    {
        DateTime referenceDate = DateTime.MaxValue;

        uint actual = dailyVacation.CountDaysBefore(referenceDate);

        actual.Should().Be(2913446u);
    }
}