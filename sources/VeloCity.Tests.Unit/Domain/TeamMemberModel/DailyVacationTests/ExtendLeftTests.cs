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

public class ExtendLeftTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2234234)]
    public void HavingFullInfiniteVacation_WhenExtendingLeft_ThenStartDateRemainsNull(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = DateInterval.FullInfinite
        };

        dailyVacation.ExtendLeft(dayCount);

        dailyVacation.StartDate.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2234234)]
    public void HavingFullInfiniteVacation_WhenExtendingLeft_ThenEndDateRemainsNull(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = DateInterval.FullInfinite
        };

        dailyVacation.ExtendLeft(dayCount);

        dailyVacation.EndDate.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2234234)]
    public void HavingStartInfiniteVacation_WhenExtendingLeft_ThenStartDateRemainsNull(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(null, new DateTime(2023, 04, 14))
        };

        dailyVacation.ExtendLeft(dayCount);

        dailyVacation.StartDate.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2234234)]
    public void HavingStartInfiniteVacation_WhenExtendingLeft_ThenEndDateRemainsUnchanged(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(null, new DateTime(2023, 04, 14))
        };

        dailyVacation.ExtendLeft(dayCount);

        dailyVacation.EndDate.Should().Be(new DateTime(2023, 04, 14));
    }

    [Theory]
    [InlineData(0, "2023 04 14")]
    [InlineData(1, "2023 04 13")]
    [InlineData(14, "2023 03 31")]
    [InlineData(365, "2022 04 14")]
    public void HavingEndInfiniteVacation_WhenExtendingLeft_ThenStartDateIsChangedAccordingly(uint dayCount, string expectedDateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14))
        };

        dailyVacation.ExtendLeft(dayCount);

        DateTime expectedDateTime = expectedDateString.ToDateTime();
        dailyVacation.StartDate.Should().Be(expectedDateTime);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2234234)]
    public void HavingEndInfiniteVacation_WhenExtendingLeft_ThenEndDateRemainsNull(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14))
        };

        dailyVacation.ExtendLeft(dayCount);

        dailyVacation.EndDate.Should().BeNull();
    }

    [Theory]
    [InlineData(0, "2023 04 14")]
    [InlineData(1, "2023 04 13")]
    [InlineData(14, "2023 03 31")]
    [InlineData(365, "2022 04 14")]
    public void HavingFiniteVacation_WhenExtendingLeft_ThenStartDateIsChangedAccordingly(uint dayCount, string expectedDateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14), new DateTime(2023, 04, 20))
        };

        dailyVacation.ExtendLeft(dayCount);

        DateTime expectedDateTime = expectedDateString.ToDateTime();
        dailyVacation.StartDate.Should().Be(expectedDateTime);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2234234)]
    public void HavingFiniteVacation_WhenExtendingLeft_ThenEndDateRemainsUnchanged(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14), new DateTime(2023, 04, 20))
        };

        dailyVacation.ExtendLeft(dayCount);

        dailyVacation.EndDate.Should().Be(new DateTime(2023, 04, 20));
    }
}