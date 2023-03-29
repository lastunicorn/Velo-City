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

namespace DustInTheWind.VeloCity.Tests.Domain.StoryPointsTests
{
    public class ImplicitOperatorFromFloatTests
    {
        [Fact]
        public void HavingNullNumber_WhenConvertedToStoryPoints_ThenTheNewInstanceIsEmpty()
        {
            float? initialValue = null;

            StoryPoints storyPoints = initialValue;

            storyPoints.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void HavingTheNumberZero_WhenConvertedToStoryPoints_ThenTheNewInstanceHasValueZero()
        {
            float initialValue = 0;

            StoryPoints storyPoints = initialValue;

            storyPoints.Value.Should().Be(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(3458120)]
        [InlineData(-7634903)]
        public void HavingANonZeroNumber_WhenConvertedToStoryPoints_ThenTheNewInstanceHasThatValue(float initialValue)
        {
            StoryPoints storyPoints = initialValue;

            storyPoints.Value.Should().Be(initialValue);
        }
    }
}
