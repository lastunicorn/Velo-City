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

namespace DustInTheWind.VeloCity.Tests.Domain.HoursValueTests
{
    public class OperatorMinusBetweenHoursValueTests
    {
        [Fact]
        public void HavingOneZeroHoursValues_WhenSubtractingAZeroHoursValue_ThenResultsAZeroHoursValue()
        {
            HoursValue hoursValue1 = new();
            HoursValue hoursValue2 = new();

            HoursValue actual = hoursValue1 - hoursValue2;

            actual.Value.Should().Be(0);
        }

        [Fact]
        public void HavingOneZeroHoursValue_WhenSubtractingANonZeroHourValue_ThenResultsANegativeHoursValue()
        {
            HoursValue hoursValue1 = new();
            HoursValue hoursValue2 = new() { Value = 5 };

            HoursValue actual = hoursValue1 - hoursValue2;

            actual.Value.Should().Be(-5);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 4, -4)]
        [InlineData(6, 2, 4)]
        [InlineData(6, 9, -3)]
        [InlineData(6, -9, 15)]
        [InlineData(-3, 2, -5)]
        [InlineData(-3, 7, -10)]
        [InlineData(-3, -7, 4)]
        public void HavingTwoHoursValues_WhenSubtractingThem_ThenResultsAHoursValueHavingTheDifferenceAsValue(int value1, int value2, int expected)
        {
            HoursValue hoursValue1 = new() { Value = value1 };
            HoursValue hoursValue2 = new() { Value = value2 };

            HoursValue actual = hoursValue1 - hoursValue2;

            actual.Value.Should().Be(expected);
        }
    }
}