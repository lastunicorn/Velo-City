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
    public class ToStringTests
    {
        [Fact]
        public void HavingZeroHoursValue_WhenSerialized_ThenReturnsTextContainingDash()
        {
            HoursValue hoursValue = new();

            string actual = hoursValue.ToString();

            actual.Should().Be("- h");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(12)]
        [InlineData(129347628)]
        public void HavingPositiveHoursValue_WhenSerialized_ThenReturnsTextContainingTheValue(int hours)
        {
            HoursValue hoursValue = new()
            {
                Value = hours
            };

            string actual = hoursValue.ToString();

            actual.Should().Be(hours + " h");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-5)]
        [InlineData(-12)]
        [InlineData(-2345603)]
        public void HavingNegativeHoursValue_WhenSerialized_ThenReturnsTextContainingTheValue(int hours)
        {
            HoursValue hoursValue = new()
            {
                Value = hours
            };

            string actual = hoursValue.ToString();

            actual.Should().Be(hours + " h");
        }
    }
}