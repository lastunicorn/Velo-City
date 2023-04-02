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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.PersonNameTests;

public class ParseStringWithTabsTests
{
    [Fact]
    public void WhenParsingStringStartingWithMultipleSpaces_ThenSpacesAreIgnored()
    {
        PersonName personName = PersonName.Parse("   word1 word2 word3");

        personName.FirstName.Should().Be("word1");
        personName.MiddleName.Should().Be("word2");
        personName.LastName.Should().Be("word3");
    }

    [Fact]
    public void WhenParsingStringEndingWithMultipleSpaces_ThenSpacesAreIgnored()
    {
        PersonName personName = PersonName.Parse("word1 word2 word3   ");

        personName.FirstName.Should().Be("word1");
        personName.MiddleName.Should().Be("word2");
        personName.LastName.Should().Be("word3");
    }

    [Fact]
    public void WhenParsingStringStartingWithMultipleTabs_ThenTabsAreIgnored()
    {
        PersonName personName = PersonName.Parse("\t\t\tword1 word2 word3");

        personName.FirstName.Should().Be("word1");
        personName.MiddleName.Should().Be("word2");
        personName.LastName.Should().Be("word3");
    }

    [Fact]
    public void WhenParsingStringEndingWithMultipleTabs_ThenTabsAreIgnored()
    {
        PersonName personName = PersonName.Parse("word1 word2 word3\t\t\t");

        personName.FirstName.Should().Be("word1");
        personName.MiddleName.Should().Be("word2");
        personName.LastName.Should().Be("word3");
    }

    [Fact]
    public void WhenParsingStringWithPartsSeparatedByTabs_ThenPartsAreRecognizedCorrectly()
    {
        PersonName personName = PersonName.Parse("word1\tword2\tword3");

        personName.FirstName.Should().Be("word1");
        personName.MiddleName.Should().Be("word2");
        personName.LastName.Should().Be("word3");
    }

    [Fact]
    public void WhenParsingStringWithPartsSeparatedByMultipleTabs_ThenPartsAreRecognizedCorrectly()
    {
        PersonName personName = PersonName.Parse("word1\t\t\tword2\t\t\tword3");

        personName.FirstName.Should().Be("word1");
        personName.MiddleName.Should().Be("word2");
        personName.LastName.Should().Be("word3");
    }
}