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

using DustInTheWind.VeloCity.Presentation.Infrastructure;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Infrastructure.ArgumentsTests
{
    public class Constructor_TwoOrdinalArgumentsTests
    {
        private readonly Arguments arguments;

        public Constructor_TwoOrdinalArgumentsTests()
        {
            string[] args = { "param1", "param2" };

            arguments = new Arguments(args);
        }

        [Fact]
        public void HavingArgsStringWithTwoOrdinalArguments_WhenParsed_ThenArgumentsContainsTwoItems()
        {
            arguments.Count.Should().Be(2);
        }

        [Fact]
        public void HavingArgsStringWithTwoOrdinalArguments_WhenParsed_ThenSecondArgumentHasTypeOrdinal()
        {
            arguments[1].Type.Should().Be(ArgumentType.Ordinal);
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenSecondArgumentHasNullName()
        {
            arguments[1].Name.Should().BeNull();
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenSecondArgumentHasCorrectValue()
        {
            arguments[1].Value.Should().Be("param2");
        }
    }
}