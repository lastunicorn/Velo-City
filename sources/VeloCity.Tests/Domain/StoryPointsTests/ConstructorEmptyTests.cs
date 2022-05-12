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
    public class ConstructorEmptyTests
    {
        private readonly StoryPoints storyPoints;

        public ConstructorEmptyTests()
        {
            storyPoints = new();
        }

        [Fact]
        public void WhenCreatingNewInstance_ThenValueIsZero()
        {
            storyPoints.Value.Should().Be(0);
        }

        [Fact]
        public void WhenCreatingNewInstance_ThenIsNullIsFalse()
        {
            storyPoints.IsNull.Should().BeFalse();
        }

        [Fact]
        public void WhenCreatingNewInstance_ThenIsNotNullIsTrue()
        {
            storyPoints.IsNotNull.Should().BeTrue();
        }

        [Fact]
        public void WhenCreatingNewInstance_ThenIsEmptyIsTrue()
        {
            storyPoints.IsEmpty.Should().BeTrue();
        }
    }
}