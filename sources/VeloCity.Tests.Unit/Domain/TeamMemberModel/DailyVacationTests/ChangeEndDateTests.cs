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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.DailyVacationTests;

public class ChangeEndDateTests
{
    [Theory]
    [InlineData("2023 04 14")]
    [InlineData("+")]
    [InlineData(null)]
    public void HavingFullInfiniteVacation_WhenChangingEndDate_ThenStartDateRemainsNull(string dateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = DateInterval.FullInfinite
        };
        DateTime? date = dateString.ToNullableDateTime();

        dailyVacation.ChangeEndDate(date);

        dailyVacation.StartDate.Should().BeNull();
    }
    
    [Theory]
    [InlineData("2023 04 14")]
    [InlineData("+")]
    [InlineData(null)]
    public void HavingFullInfiniteVacation_WhenChangingEndDate_ThenEndDateIsUpdatedAccordingly(string dateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = DateInterval.FullInfinite
        };
        DateTime? date = dateString.ToNullableDateTime();

        dailyVacation.ChangeEndDate(date);

        dailyVacation.EndDate.Should().Be(date);
    }
    
    [Theory]
    [InlineData("2023 04 10")]
    [InlineData("2023 04 20")]
    [InlineData("+")]
    [InlineData(null)]
    public void HavingStartInfiniteVacation_WhenChangingEndDate_ThenStartDateRemainsNull(string dateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(null, new DateTime(2023, 04, 14))
        };
        DateTime? date = dateString.ToNullableDateTime();

        dailyVacation.ChangeEndDate(date);

        dailyVacation.StartDate.Should().BeNull();
    }

    [Theory]
    [InlineData("2023 04 10")]
    [InlineData("2023 04 20")]
    [InlineData("+")]
    [InlineData(null)]
    public void HavingStartInfiniteVacation_WhenChangingEndDate_ThenEndDateIsUpdatedAccordingly(string dateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(null, new DateTime(2023, 04, 14))
        };
        DateTime? date = dateString.ToNullableDateTime();

        dailyVacation.ChangeEndDate(date);

        dailyVacation.EndDate.Should().Be(date);
    }

    [Theory]
    [InlineData("2023 04 20")]
    [InlineData("+")]
    [InlineData(null)]
    public void HavingEndInfiniteVacation_WhenChangingEndDate_ThenStartDateRemainsUnchanged(string dateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14))
        };
        DateTime? date = dateString.ToNullableDateTime();

        dailyVacation.ChangeEndDate(date);

        dailyVacation.StartDate.Should().Be(new DateTime(2023, 04, 14));
    }

    [Theory]
    [InlineData("2023 04 20")]
    [InlineData("+")]
    [InlineData(null)]
    public void HavingEndInfiniteVacation_WhenChangingEndDate_ThenEndDateIsUpdatedAccordingly(string dateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14))
        };
        DateTime? date = dateString.ToNullableDateTime();

        dailyVacation.ChangeEndDate(date);

        dailyVacation.EndDate.Should().Be(date);
    }

    [Theory]
    [InlineData("2023 04 20")]
    [InlineData("2023 04 30")]
    [InlineData("+")]
    [InlineData(null)]
    public void HavingFiniteVacation_WhenChangingEndDate_ThenStartDateRemainsUnchanged(string dateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14), new DateTime(2023, 04, 25))
        };
        DateTime? date = dateString.ToNullableDateTime();

        dailyVacation.ChangeEndDate(date);

        dailyVacation.StartDate.Should().Be(new DateTime(2023, 04, 14));
    }

    [Theory]
    [InlineData("2023 04 20")]
    [InlineData("2023 04 30")]
    [InlineData("+")]
    [InlineData(null)]
    public void HavingFiniteVacation_WhenChangingEndDate_ThenEndDateIsUpdatedAccordingly(string dateString)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14), new DateTime(2023, 04, 25))
        };
        DateTime? date = dateString.ToNullableDateTime();

        dailyVacation.ChangeEndDate(date);

        dailyVacation.EndDate.Should().Be(date);
    }
}