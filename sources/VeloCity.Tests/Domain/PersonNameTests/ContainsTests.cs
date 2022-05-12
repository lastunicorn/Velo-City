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
    public class ContainsTests
    {
        [Fact]
        public void HavingEmptyInstance_WhenSearchingForNull_ReturnsFalse()
        {
            PersonName personName = new();

            bool actual = personName.Contains(null);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingEmptyInstance_WhenSearchingForEmptyString_ReturnsFalse()
        {
            PersonName personName = new();

            bool actual = personName.Contains(string.Empty);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingEmptyInstance_WhenSearchingForAString_ReturnsFalse()
        {
            PersonName personName = new();

            bool actual = personName.Contains("text");

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingFirstName_WhenSearchingForAStringContainedByFirstName_ReturnsTrue()
        {
            PersonName personName = new()
            {
                FirstName = "first-name"
            };

            bool actual = personName.Contains("IRST");

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingFirstName_WhenSearchingForAStringNotContainedByFirstName_ReturnsFalse()
        {
            PersonName personName = new()
            {
                FirstName = "first-name"
            };

            bool actual = personName.Contains("text");

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingMiddleName_WhenSearchingForAStringContainedByMiddleName_ReturnsTrue()
        {
            PersonName personName = new()
            {
                MiddleName = "middle-name"
            };

            bool actual = personName.Contains("DDLE");

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingMiddleName_WhenSearchingForAStringNotContainedByMiddleName_ReturnsFalse()
        {
            PersonName personName = new()
            {
                MiddleName = "middle-name"
            };

            bool actual = personName.Contains("text");

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingLastName_WhenSearchingForAStringContainedByLastName_ReturnsTrue()
        {
            PersonName personName = new()
            {
                LastName = "last-name"
            };

            bool actual = personName.Contains("AST");

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingLastName_WhenSearchingForAStringNotContainedByLastName_ReturnsFalse()
        {
            PersonName personName = new()
            {
                LastName = "last-name"
            };

            bool actual = personName.Contains("text");

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingNickname_WhenSearchingForAStringContainedByNickname_ReturnsTrue()
        {
            PersonName personName = new()
            {
                Nickname = "nickname"
            };

            bool actual = personName.Contains("CKN");

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingNickname_WhenSearchingForAStringNotContainedByNickname_ReturnsFalse()
        {
            PersonName personName = new()
            {
                Nickname = "nickname"
            };

            bool actual = personName.Contains("text");

            actual.Should().BeFalse();
        }
    }
}