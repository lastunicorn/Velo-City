// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.StoryPointsTests;

public class ImplicitOperatorToFloatTests
{
    [Fact]
    public void HavingInstanceWithNoValueSet_WhenConvertedToFloat_ThenReturnsZero()
    {
        StoryPoints storyPoints = new();

        float value = storyPoints;

        value.Should().Be(0);
    }

    [Fact]
    public void HavingInstanceWithValueZero_WhenConvertedToFloat_ThenReturnsZero()
    {
        StoryPoints storyPoints = new();

        float value = storyPoints;

        value.Should().Be(0);
    }

    [Fact]
    public void HavingEmptyInstance_WhenConvertedToFloat_ThenReturnsNull()
    {
        StoryPoints storyPoints = StoryPoints.Empty;

        float? value = storyPoints;

        value.Should().BeNull();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(10)]
    [InlineData(-7)]
    public void HavingInstanceWithValueSet_WhenConvertedToFloat_ThenReturnsThatValue(float initialValue)
    {
        StoryPoints storyPoints = new()
        {
            Value = initialValue
        };

        float actualValue = storyPoints;

        actualValue.Should().Be(initialValue);
    }
}