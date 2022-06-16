// Velo City
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
using DustInTheWind.VeloCity.Cli.Presentation.Commands.Vacations;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Presentation.Commands.Vacations.DateTimeMonthTests
{
    public class ConstructorFromYearMonthTests
    {
        [Theory]
        [InlineData(2022)]
        [InlineData(1000000)]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenCreatingNewInstanceWithSpecificYearAndMonth_ThenYearPropertyIsSetToProvidedYear(int year)
        {
            DateTimeMonth dateTimeMonth = new(year, 7);

            dateTimeMonth.Year.Should().Be(year);
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
        public void WhenCreatingNewInstanceWithSpecificYearAndMonth_ThenMonthPropertyIsSetToProvidedMonth(int month)
        {
            DateTimeMonth dateTimeMonth = new(2002, month);

            dateTimeMonth.Month.Should().Be(month);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void WhenCreatingNewInstanceWithSpecificYearAndNegativeMonth_ThenThrows(int month)
        {
            Action action = () => new DateTimeMonth(2002, month);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WhenCreatingNewInstanceWithSpecificYearAndMonthEqualToZero_ThenThrows()
        {
            Action action = () => new DateTimeMonth(2002, 0);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(100)]
        public void WhenCreatingNewInstanceWithSpecificYearAndMonthGreaterThan12_ThenThrows(int month)
        {
            Action action = () => new DateTimeMonth(2002, month);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}