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

using DustInTheWind.VeloCity.Domain;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.StoryPointsTests
{
    public class NullInstanceTests
    {
        [Fact]
        public void HavingTheNullStaticInstance_ThenValueIsZero()
        {
            StoryPoints.Null.Value.Should().Be(0);
        }

        [Fact]
        public void HavingTheNullStaticInstance_ThenIsNullIsTrue()
        {
            StoryPoints.Null.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void HavingTheNullStaticInstance_ThenIsNotNullIsFalse()
        {
            StoryPoints.Null.IsNotEmpty.Should().BeFalse();
        }

        [Fact]
        public void HavingTheNullStaticInstance_ThenIsEmptyIsTrue()
        {
            StoryPoints.Null.IsZero.Should().BeTrue();
        }

        [Fact]
        public void HavingTheNullStaticInstance_WhenSerialized_ThenDashIsUsedInsteadOfValue()
        {
            string actual = StoryPoints.Null.ToString();

            actual.Should().Be("- SP");
        }

        [Fact]
        public void HavingTheNullStaticInstance_WhenSerializedWithFormatting_ThenDashIsUsedInsteadOfValueAndFormatIsIgnored()
        {
            string actual = StoryPoints.Null.ToString("D5");

            actual.Should().Be("- SP");
        }

        [Fact]
        public void HavingTheNullStaticInstance_WhenSerializedToStandardDigitsString_ThenDashIsUsedInsteadOfValueAndFormatIsIgnored()
        {
            string actual = StoryPoints.Null.ToStandardDigitsString();

            actual.Should().Be("- SP");
        }

        [Fact]
        public void HavingTheNullStaticInstance_WhenImplicitlyCastToFloat_ThenZeroIsReturned()
        {
            float actual = StoryPoints.Null;

            actual.Should().Be(0);
        }

        [Fact]
        public void HavingTheNullStaticInstance_WhenImplicitlyCastToNullableFloat_ThenNullIsReturned()
        {
            float? actual = StoryPoints.Null;

            actual.Should().BeNull();
        }
    }
}