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
    public class ParseThreeWordsTests
    {
        private readonly PersonName personName;

        public ParseThreeWordsTests()
        {
            personName = PersonName.Parse("word1 word2 word3");
        }

        [Fact]
        public void WhenParsingThreeWords_ThenFirstNameIsFirstWord()
        {
            personName.FirstName.Should().Be("word1");
        }

        [Fact]
        public void WhenParsingThreeWords_ThenMiddleNameIsSecondWord()
        {
            personName.MiddleName.Should().Be("word2");
        }

        [Fact]
        public void WhenParsingThreeWords_ThenLastNameIsThirdWord()
        {
            personName.LastName.Should().Be("word3");
        }

        [Fact]
        public void WhenParsingThreeWords_ThenNicknameIsNull()
        {
            personName.Nickname.Should().BeNull();
        }
    }
}