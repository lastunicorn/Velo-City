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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.DailyVacationTests;

public class ExtendRightTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(365)]
    public void HavingFullInfiniteVacation_WhenExtendingRight_ThenStartDateRemainsNull(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = DateInterval.FullInfinite
        };

        dailyVacation.ExtendRight(dayCount);

        dailyVacation.StartDate.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(365)]
    public void HavingFullInfiniteVacation_WhenExtendingRight_ThenEndDateRemainsNull(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = DateInterval.FullInfinite
        };

        dailyVacation.ExtendRight(dayCount);

        dailyVacation.EndDate.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(21)]
    [InlineData(365)]
    public void HavingStartInfiniteVacation_WhenExtendingRight_ThenStartDateRemainsNull(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(null, new DateTime(2023, 04, 14))
        };

        dailyVacation.ExtendRight(dayCount);

        dailyVacation.StartDate.Should().BeNull();
    }

    [Theory]
    [InlineData(0, "2023 04 14")]
    [InlineData(1, "2023 04 15")]
    [InlineData(21, "2023 05 05")]
    [InlineData(365, "2024 04 13")]
    public void HavingStartInfiniteVacation_WhenExtendingRight_ThenEndDateIsUpdatedAccordingly(uint dayCount, string expectedDateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(null, new DateTime(2023, 04, 14))
        };

        dailyVacation.ExtendRight(dayCount);

        DateTime expectedDateTime = expectedDateString.ToDateTime();
        dailyVacation.EndDate.Should().Be(expectedDateTime);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(365)]
    public void HavingEndInfiniteVacation_WhenExtendingRight_ThenStartDateRemainsUnchanged(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14))
        };

        dailyVacation.ExtendRight(dayCount);
        
        dailyVacation.StartDate.Should().Be(new DateTime(2023, 04, 14));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(365)]
    public void HavingEndInfiniteVacation_WhenExtendingRight_ThenEndDateRemainsNull(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14))
        };

        dailyVacation.ExtendRight(dayCount);

        dailyVacation.EndDate.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(21)]
    [InlineData(365)]
    public void HavingFiniteVacation_WhenExtendingRight_ThenStartDateRemainsUnchanged(uint dayCount)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14), new DateTime(2023, 04, 20))
        };

        dailyVacation.ExtendRight(dayCount);

        dailyVacation.StartDate.Should().Be(new DateTime(2023, 04, 14));
    }

    [Theory]
    [InlineData(0, "2023 04 20")]
    [InlineData(1, "2023 04 21")]
    [InlineData(21, "2023 05 11")]
    [InlineData(365, "2024 04 19")]
    public void HavingFiniteVacation_WhenExtendingRight_ThenEndDateIsUpdatedAccordingly(uint dayCount, string expectedDateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14), new DateTime(2023, 04, 20))
        };

        dailyVacation.ExtendRight(dayCount);

        DateTime expectedDateTime = expectedDateString.ToDateTime();
        dailyVacation.EndDate.Should().Be(expectedDateTime);
    }
}