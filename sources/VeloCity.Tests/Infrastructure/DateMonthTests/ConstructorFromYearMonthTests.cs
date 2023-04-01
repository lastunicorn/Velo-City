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

using DustInTheWind.VeloCity.Infrastructure;

namespace DustInTheWind.VeloCity.Tests.Infrastructure.DateMonthTests;

public class ConstructorFromYearMonthTests
{
    [Theory]
    [InlineData(-345987234)]
    [InlineData(-200)]
    [InlineData(0)]
    [InlineData(1320)]
    [InlineData(248964875)]
    public void HavingANumberAsYear_WhenCreatingAnInstanceWithThatYear_ThenYearHasSpecifiedValue(int year)
    {
        DateMonth dateMonth = new(year, 1);

        dateMonth.Year.Should().Be(year);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    public void HavingANumberAsMonth_WhenCreatingAnInstanceWithThatMonth_ThenMonthHasSpecifiedValue(int month)
    {
        DateMonth dateMonth = new(0, month);

        dateMonth.Month.Should().Be(month);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void HavingANumberLessThan1AsMonth_WhenCreateInstanceWithThatMonth_ThenThrows(int month)
    {
        Action action = () =>
        {
            _ = new DateMonth(0, month);
        };

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(13)]
    [InlineData(20)]
    [InlineData(100)]
    public void HavingANumberGreaterThan12AsMonth_WhenCreateInstanceWithThatMonth_ThenThrows(int month)
    {
        Action action = () =>
        {
            _ = new DateMonth(0, month);
        };

        action.Should().Throw<ArgumentOutOfRangeException>();
    }
}