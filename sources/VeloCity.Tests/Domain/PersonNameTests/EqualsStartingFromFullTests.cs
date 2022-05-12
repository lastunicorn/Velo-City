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

namespace DustInTheWind.VeloCity.Tests.Domain.PersonNameTests
{
    public class EqualsStartingFromFullTests
    {
        [Fact]
        public void HavingTwoPersonNamesWithIdenticalParts_WhenCompared_ThenReturnsTrue()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingDifferentFirstName_WhenCompared_ThenReturnsFalse()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name-1",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name-2",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingDifferentMiddleName_WhenCompared_ThenReturnsFalse()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name-1",
                LastName = "last-name",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name-2",
                LastName = "last-name",
                Nickname = "nickname"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingDifferentLastName_WhenCompared_ThenReturnsFalse()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name-1",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name-2",
                Nickname = "nickname"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingDifferentNickname_WhenCompared_ThenReturnsFalse()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname-1"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname-2"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeFalse();
        }
    }
}