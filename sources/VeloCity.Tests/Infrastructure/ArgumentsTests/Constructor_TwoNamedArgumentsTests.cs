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
    public class Constructor_TwoNamedArgumentsTests
    {
        private readonly Arguments arguments;

        public Constructor_TwoNamedArgumentsTests()
        {
            string[] args = { "-param1", "value1", "-param2", "value2" };

            arguments = new Arguments(args);
        }

        [Fact]
        public void HavingArgsStringWithTwoNamedArgument_WhenParsed_ThenSecondArgumentsContainsTwoItems()
        {
            arguments.Count.Should().Be(2);
        }

        [Fact]
        public void HavingArgsStringWithTwoNamedArgument_WhenParsed_ThenSecondArgumentHasTypeNamed()
        {
            arguments[1].Type.Should().Be(ArgumentType.Named);
        }

        [Fact]
        public void HavingArgsStringWithTwoNamedArgument_WhenParsed_ThenSecondArgumentHasCorrectName()
        {
            arguments[1].Name.Should().Be("param2");
        }

        [Fact]
        public void HavingArgsStringWithTwoNamedArgument_WhenParsed_ThenSecondArgumentHasCorrectValue()
        {
            arguments[1].Value.Should().Be("value2");
        }
    }
}