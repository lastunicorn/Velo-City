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
    public class ConstructorWithValueTests
    {
        private readonly StoryPoints storyPoints;

        public ConstructorWithValueTests()
        {
            storyPoints = new()
            {
                Value = 14
            };
        }

        [Fact]
        public void WhenCreatingNewInstanceWithValue14_ThenValueIs14()
        {
            storyPoints.Value.Should().Be(14);
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
        public void WhenCreatingNewInstance_ThenIsEmptyIsFalse()
        {
            storyPoints.IsEmpty.Should().BeFalse();
        }
    }
}