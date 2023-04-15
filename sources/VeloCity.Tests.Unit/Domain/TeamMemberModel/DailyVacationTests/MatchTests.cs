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

public class MatchTests
{
    [Theory]
    [InlineData("-", true)]
    [InlineData("2023 04 14", true)]
    [InlineData("+", true)]
    public void HavingFullInfiniteVacation_WhenMatchingDate(string dateString, bool expected)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = DateInterval.FullInfinite
        };
        DateTime date = dateString.ToDateTime();

        bool actual = dailyVacation.Match(date);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("-", true)]
    [InlineData("2023 04 13", true)]
    [InlineData("2023 04 14", true)]
    [InlineData("2023 04 15", false)]
    [InlineData("+", false)]
    public void HavingStartInfiniteVacation_WhenMatchingDate(string dateString, bool expected)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(null, new DateTime(2023, 04, 14))
        };
        DateTime date = dateString.ToDateTime();

        bool actual = dailyVacation.Match(date);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("-", false)]
    [InlineData("2023 04 13", false)]
    [InlineData("2023 04 14", true)]
    [InlineData("2023 04 15", true)]
    [InlineData("+", true)]
    public void HavingEndInfiniteVacation_WhenMatchingDate(string dateString, bool expected)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14))
        };
        DateTime date = dateString.ToDateTime();

        bool actual = dailyVacation.Match(date);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("-", false)]
    [InlineData("2023 04 13", false)]
    [InlineData("2023 04 14", true)]
    [InlineData("2023 04 15", true)]
    [InlineData("2023 04 19", true)]
    [InlineData("2023 04 20", true)]
    [InlineData("2023 04 21", false)]
    [InlineData("+", false)]
    public void HavingFiniteVacation_WhenMatchingDate(string dateString, bool expected)
    {
        DailyVacation dailyVacation = new()
        {
            DateInterval = new DateInterval(new DateTime(2023, 04, 14), new DateTime(2023, 04, 20))
        };
        DateTime date = dateString.ToDateTime();

        bool actual = dailyVacation.Match(date);

        actual.Should().Be(expected);
    }
}