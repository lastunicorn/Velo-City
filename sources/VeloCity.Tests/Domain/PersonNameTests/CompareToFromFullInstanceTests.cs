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
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.PersonNameTests
{
    public class CompareToFromFullInstanceTests
    {
        [Fact]
        public void HavingTwoPersonNamesWithIdenticalParts_WhenCompared_ReturnsZero()
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

            int actual = personName1.CompareTo(personName2);

            actual.Should().Be(0);
        }

        [Fact]
        public void HavingOnePersonNameWithFirstNameGreaterThanASecondOne_WhenCompared_ReturnsPositiveValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "fff",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "ccc",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void HavingOnePersonNameWithFirstNameLowerThanASecondOne_WhenCompared_ReturnsNegativeValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "aaa",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "ccc",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "nickname"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void HavingOnePersonNameWithMiddleNameGreaterThanASecondOne_WhenCompared_ReturnsPositiveValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "yyy",
                LastName = "last-name",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "ggg",
                LastName = "last-name",
                Nickname = "nickname"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void HavingOnePersonNameWithMiddleNameLowerThanASecondOne_WhenCompared_ReturnsNegativeValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "ddd",
                LastName = "last-name",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "jjj",
                LastName = "last-name",
                Nickname = "nickname"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void HavingOnePersonNameWithLastNameGreaterThanASecondOne_WhenCompared_ReturnsPositiveValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "ooo",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "eee",
                Nickname = "nickname"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void HavingOnePersonNameWithLastNameLowerThanASecondOne_WhenCompared_ReturnsNegativeValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "ttt",
                Nickname = "nickname"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "vvv",
                Nickname = "nickname"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void HavingOnePersonNameWithNicknameGreaterThanASecondOne_WhenCompared_ReturnsPositiveValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "www"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "hhh"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void HavingOnePersonNameWithNicknameLowerThanASecondOne_WhenCompared_ReturnsNegativeValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "iii"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name",
                MiddleName = "middle-name",
                LastName = "last-name",
                Nickname = "ppp"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeLessThan(0);
        }
    }
}