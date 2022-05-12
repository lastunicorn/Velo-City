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

using System.Globalization;
using DustInTheWind.VeloCity.Domain;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.StoryPointsTests
{
    public class EmptyInstanceTests
    {
        [Fact]
        public void HavingTheEmptyStaticInstance_ThenValueIsZero()
        {
            StoryPoints.Empty.Value.Should().Be(0);
        }

        [Fact]
        public void HavingTheEmptyStaticInstance_ThenIsNullIsFalse()
        {
            StoryPoints.Empty.IsNull.Should().BeFalse();
        }

        [Fact]
        public void HavingTheEmptyStaticInstance_ThenIsNotNullIsTrue()
        {
            StoryPoints.Empty.IsNotNull.Should().BeTrue();
        }

        [Fact]
        public void HavingTheEmptyStaticInstance_ThenIsEmptyIsTrue()
        {
            StoryPoints.Empty.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void HavingTheEmptyStaticInstance_WhenSerialized_ThenStringContainsZeroSP()
        {
            string actual = StoryPoints.Empty.ToString();

            actual.Should().Be("0 SP");
        }

        [Fact]
        public void HavingTheEmptyStaticInstanceInEnUsCulture_WhenSerializedWithF2Formatting_ThenStringContainsZerosWithTwoDigitsSP()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            string actual = StoryPoints.Empty.ToString("F2");

            actual.Should().Be("0.00 SP");
        }

        [Fact]
        public void HavingTheEmptyStaticInstanceInEnUsCulture_WhenSerializedToStandardDigitsString_ThenStringContainsZeroSP()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            string actual = StoryPoints.Empty.ToStandardDigitsString();

            actual.Should().Be("0 SP");
        }

        [Fact]
        public void HavingTheEmptyStaticInstance_WhenImplicitlyCastToFloat_ThenZeroIsReturned()
        {
            float actual = StoryPoints.Empty;

            actual.Should().Be(0);
        }

        [Fact]
        public void HavingTheEmptyStaticInstance_WhenImplicitlyCastToNullableFloat_ThenZeroIsReturned()
        {
            float? actual = StoryPoints.Empty;

            actual.Should().Be(0);
        }
    }
}