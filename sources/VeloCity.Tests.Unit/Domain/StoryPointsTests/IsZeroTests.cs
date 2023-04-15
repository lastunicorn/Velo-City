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

public class IsZeroTests
{
    [Fact]
    public void HavingInstanceWithNoValueSet_ThenIsZeroIsTrue()
    {
        StoryPoints storyPoints = new();

        storyPoints.IsZero.Should().BeTrue();
    }

    [Fact]
    public void HavingInstanceWithValueZero_ThenIsZeroIsTrue()
    {
        StoryPoints storyPoints = new();

        storyPoints.IsZero.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(3458120)]
    [InlineData(-7634903)]
    public void HavingInstanceWithValueSet_ThenIsZeroIsFalse(float value)
    {
        StoryPoints storyPoints = new()
        {
            Value = value
        };

        storyPoints.IsZero.Should().BeFalse();
    }
}