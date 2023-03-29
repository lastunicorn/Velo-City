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
    public class CompareToFromEmptyInstanceTests
    {
        [Fact]
        public void HavingTwoEmptyInstances_WhenCompared_ReturnsZero()
        {
            PersonName personName1 = new();
            PersonName personName2 = new();

            int actual = personName1.CompareTo(personName2);

            actual.Should().Be(0);
        }

        [Fact]
        public void HavingTwoInstancesWithSameFirstName_WhenCompared_ReturnsZero()
        {
            PersonName personName1 = new()
            {
                FirstName = "first-name"
            };
            PersonName personName2 = new()
            {
                FirstName = "first-name"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().Be(0);
        }

        [Fact]
        public void HavingOneInstanceWithFirstNameLowerThanASecondOne_WhenCompared_ReturnsNegativeValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "aaa"
            };
            PersonName personName2 = new()
            {
                FirstName = "bbb"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void HavingOneInstanceWithFirstNameGreaterThanASecondOne_WhenCompared_ReturnsPositiveValue()
        {
            PersonName personName1 = new()
            {
                FirstName = "ggg"
            };
            PersonName personName2 = new()
            {
                FirstName = "bbb"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void HavingTwoInstancesWithSameMiddleName_WhenCompared_ReturnsZero()
        {
            PersonName personName1 = new()
            {
                MiddleName = "middle-name"
            };
            PersonName personName2 = new()
            {
                MiddleName = "middle-name"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().Be(0);
        }

        [Fact]
        public void HavingOneInstanceWithMiddleNameLowerThanASecondOne_WhenCompared_ReturnsNegativeValue()
        {
            PersonName personName1 = new()
            {
                MiddleName = "aaa"
            };
            PersonName personName2 = new()
            {
                MiddleName = "bbb"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void HavingOneInstanceWithMiddleNameGreaterThanASecondOne_WhenCompared_ReturnsPositiveValue()
        {
            PersonName personName1 = new()
            {
                MiddleName = "ggg"
            };
            PersonName personName2 = new()
            {
                MiddleName = "bbb"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void HavingTwoInstancesWithSameLastName_WhenCompared_ReturnsZero()
        {
            PersonName personName1 = new()
            {
                LastName = "last-name"
            };
            PersonName personName2 = new()
            {
                LastName = "last-name"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().Be(0);
        }

        [Fact]
        public void HavingOneInstanceWithLastNameLowerThanASecondOne_WhenCompared_ReturnsNegativeValue()
        {
            PersonName personName1 = new()
            {
                LastName = "aaa"
            };
            PersonName personName2 = new()
            {
                LastName = "bbb"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void HavingOneInstanceWithLastNameGreaterThanASecondOne_WhenCompared_ReturnsPositiveValue()
        {
            PersonName personName1 = new()
            {
                LastName = "ggg"
            };
            PersonName personName2 = new()
            {
                LastName = "bbb"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void HavingTwoInstancesWithSameNickname_WhenCompared_ReturnsZero()
        {
            PersonName personName1 = new()
            {
                Nickname = "nick"
            };
            PersonName personName2 = new()
            {
                Nickname = "nick"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().Be(0);
        }

        [Fact]
        public void HavingOneInstanceWithNicknameLowerThanASecondOne_WhenCompared_ReturnsNegativeValue()
        {
            PersonName personName1 = new()
            {
                Nickname = "aaa"
            };
            PersonName personName2 = new()
            {
                Nickname = "bbb"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeLessThan(0);
        }

        [Fact]
        public void HavingOneInstanceWithNicknameGreaterThanASecondOne_WhenCompared_ReturnsPositiveValue()
        {
            PersonName personName1 = new()
            {
                Nickname = "ggg"
            };
            PersonName personName2 = new()
            {
                Nickname = "bbb"
            };

            int actual = personName1.CompareTo(personName2);

            actual.Should().BeGreaterThan(0);
        }
    }
}