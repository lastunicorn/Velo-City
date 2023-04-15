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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.PersonNameTests;

public class ToStringTest
{
    [Fact]
    public void HavingInstanceWithOnlyFirstName_ThenFullNameContainsFirstName()
    {
        PersonName personName = new()
        {
            FirstName = "first"
        };

        string actual = personName.ToString();

        actual.Should().Be("first");
    }

    [Fact]
    public void HavingInstanceWithOnlyMiddleName_ThenFullNameContainsMiddleName()
    {
        PersonName personName = new()
        {
            MiddleName = "middle"
        };

        string actual = personName.ToString();

        actual.Should().Be("middle");
    }

    [Fact]
    public void HavingInstanceWithOnlyLastName_ThenFullNameContainsLastName()
    {
        PersonName personName = new()
        {
            LastName = "last"
        };

        string actual = personName.ToString();

        actual.Should().Be("last");
    }

    [Fact]
    public void HavingInstanceWithOnlyNickname_ThenFullNameContainsNickname()
    {
        PersonName personName = new()
        {
            Nickname = "nick"
        };

        string actual = personName.ToString();

        actual.Should().Be("nick");
    }

    [Fact]
    public void HavingInstanceWithFirstMiddleAndLastParts_ThenFullNameContainsAllParts()
    {
        PersonName personName = new()
        {
            FirstName = "first",
            MiddleName = "middle",
            LastName = "last"
        };

        string actual = personName.ToString();

        actual.Should().Be("first middle last");
    }

    [Fact]
    public void HavingInstanceWithFirstMiddleLastAndNicknameParts_ThenFullNameContainsAllParts()
    {
        PersonName personName = new()
        {
            FirstName = "first",
            MiddleName = "middle",
            LastName = "last",
            Nickname = "nick"
        };

        string actual = personName.ToString();

        actual.Should().Be("first middle last (nick)");
    }
}