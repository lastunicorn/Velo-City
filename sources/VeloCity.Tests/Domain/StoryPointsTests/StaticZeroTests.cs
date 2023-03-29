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

using System.Globalization;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Domain.StoryPointsTests
{
    public class StaticZeroTests
    {
        [Fact]
        public void HavingTheStaticZeroInstance_ThenValueIsZero()
        {
            StoryPoints.Zero.Value.Should().Be(0);
        }

        [Fact]
        public void HavingTheStaticZeroInstance_ThenIsNullIsFalse()
        {
            StoryPoints.Zero.IsEmpty.Should().BeFalse();
        }

        [Fact]
        public void HavingTheStaticZeroInstance_ThenIsNotNullIsTrue()
        {
            StoryPoints.Zero.IsNotEmpty.Should().BeTrue();
        }

        [Fact]
        public void HavingTheStaticZeroInstance_ThenIsZeroIsTrue()
        {
            StoryPoints.Zero.IsZero.Should().BeTrue();
        }

        [Fact]
        public void HavingTheStaticZeroInstance_WhenSerialized_ThenStringContainsZeroSP()
        {
            string actual = StoryPoints.Zero.ToString();

            actual.Should().Be("0 SP");
        }

        [Fact]
        public void HavingTheStaticZeroInstanceInEnUsCulture_WhenSerializedWithF2Formatting_ThenStringContainsZerosWithTwoDigitsSP()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            string actual = StoryPoints.Zero.ToString("F2");

            actual.Should().Be("0.00 SP");
        }

        [Fact]
        public void HavingTheStaticZeroInstanceInEnUsCulture_WhenSerializedToStandardDigitsString_ThenStringContainsZeroSP()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            string actual = StoryPoints.Zero.ToStandardDigitsString();

            actual.Should().Be("0 SP");
        }

        [Fact]
        public void HavingTheStaticZeroInstance_WhenImplicitlyCastToFloat_ThenZeroIsReturned()
        {
            float actual = StoryPoints.Zero;

            actual.Should().Be(0);
        }

        [Fact]
        public void HavingTheStaticZeroInstance_WhenImplicitlyCastToNullableFloat_ThenZeroIsReturned()
        {
            float? actual = StoryPoints.Zero;

            actual.Should().Be(0);
        }
    }
}