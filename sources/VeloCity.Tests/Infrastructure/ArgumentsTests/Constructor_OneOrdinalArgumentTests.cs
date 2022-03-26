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
    public class Constructor_OneOrdinalArgumentTests
    {
        private readonly Arguments arguments;

        public Constructor_OneOrdinalArgumentTests()
        {
            string[] args = { "param1" };

            arguments = new Arguments(args);
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenArgumentsContainsOneItem()
        {
            arguments.Count.Should().Be(1);
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenArgumentHasTypeOrdinal()
        {
            arguments[0].Type.Should().Be(ArgumentType.Ordinal);
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenArgumentHasNullName()
        {
            arguments[0].Name.Should().BeNull();
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenArgumentHasCorrectValue()
        {
            arguments[0].Value.Should().Be("param1");
        }
    }
}