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

public class StaticEmptyTests
{
    [Fact]
    public void HavingTheStaticEmptyInstance_ThenValueIsZero()
    {
        StoryPoints.Empty.Value.Should().Be(0);
    }

    [Fact]
    public void HavingTheStaticEmptyInstance_ThenIsNullIsTrue()
    {
        StoryPoints.Empty.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void HavingTheStaticEmptyInstance_ThenIsNotNullIsFalse()
    {
        StoryPoints.Empty.IsNotEmpty.Should().BeFalse();
    }

    [Fact]
    public void HavingTheStaticEmptyInstance_ThenIsEmptyIsTrue()
    {
        StoryPoints.Empty.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void HavingTheStaticEmptyInstance_WhenSerialized_ThenDashIsUsedInsteadOfValue()
    {
        string actual = StoryPoints.Empty.ToString();

        actual.Should().Be("- SP");
    }

    [Fact]
    public void HavingTheStaticEmptyInstance_WhenSerializedWithFormatting_ThenDashIsUsedInsteadOfValueAndFormatIsIgnored()
    {
        string actual = StoryPoints.Empty.ToString("D5");

        actual.Should().Be("- SP");
    }

    [Fact]
    public void HavingTheStaticEmptyInstance_WhenSerializedToStandardDigitsString_ThenDashIsUsedInsteadOfValueAndFormatIsIgnored()
    {
        string actual = StoryPoints.Empty.ToStandardDigitsString();

        actual.Should().Be("- SP");
    }

    [Fact]
    public void HavingTheStaticEmptyInstance_WhenImplicitlyCastToFloat_ThenZeroIsReturned()
    {
        float actual = StoryPoints.Empty;

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingTheStaticEmptyInstance_WhenImplicitlyCastToNullableFloat_ThenNullIsReturned()
    {
        float? actual = StoryPoints.Empty;

        actual.Should().BeNull();
    }
}