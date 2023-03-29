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

namespace DustInTheWind.VeloCity.Tests.Domain.PersonNameTests
{
    public class EqualsStartingFromEmptyInstanceTests
    {
        [Fact]
        public void HavingTwoEmptyPersonNames_WhenCompared_ThenReturnsTrue()
        {
            PersonName personName1 = new();
            PersonName personName2 = new();

            bool actual = personName1.Equals(personName2);

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingSameFirstName_WhenCompared_ThenReturnsTrue()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingDifferentFirstName_WhenCompared_ThenReturnsFalse()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name-1"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name-2"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingSameMiddleName_WhenCompared_ThenReturnsTrue()
        {
            PersonName personName1 = new()
            {
                MiddleName = "middle-name"
            };
            PersonName personName2 = new()
            {
                MiddleName = "middle-name"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingDifferentMiddleName_WhenCompared_ThenReturnsFalse()
        {
            PersonName personName1 = new()
            {
                MiddleName = "middle-name-1"
            };
            PersonName personName2 = new()
            {
                MiddleName = "middle-name-2"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingSameLastName_WhenCompared_ThenReturnsTrue()
        {
            PersonName personName1 = new()
            {
                LastName = "last-name"
            };
            PersonName personName2 = new()
            {
                LastName = "last-name"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingDifferentLastName_WhenCompared_ThenReturnsFalse()
        {
            PersonName personName1 = new()
            {
                LastName = "last-name-1"
            };
            PersonName personName2 = new()
            {
                LastName = "last-name-2"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingSameNickname_WhenCompared_ThenReturnsTrue()
        {
            PersonName personName1 = new()
            {
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                Nickname = "nickname"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingTwoPersonNamesContainingDifferentNickname_WhenCompared_ThenReturnsFalse()
        {
            PersonName personName1 = new()
            {
                Nickname = "nickname-1"
            };
            PersonName personName2 = new()
            {
                Nickname = "nickname-2"
            };

            bool actual = personName1.Equals(personName2);

            actual.Should().BeFalse();
        }
    }
}