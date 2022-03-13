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

using System;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Infrastructure.ArgumentsTests
{
    public class IndexerByNameTests
    {
        [Fact]
        public void HavingArgumentsInstanceWithNoArgument_WhenRetrievingNonexistentItem_ThenReturnsNull()
        {
            Arguments arguments = new(Array.Empty<string>());

            Argument argument = arguments["nonexistent"];

            argument.Should().BeNull();
        }

        [Fact]
        public void HavingArgumentsInstanceWithOneNamedArgument_WhenRetrievingNonexistentItem_ThenReturnsNull()
        {
            Arguments arguments = new(new[] { "-argument1", "value1" });

            Argument argument = arguments["nonexistent"];

            argument.Should().BeNull();
        }

        [Fact]
        public void HavingArgumentsInstanceWithOneNamedArgument_WhenRetrievingIt_ThenReturnsTheArgument()
        {
            Arguments arguments = new(new[] { "-argument1", "value1" });

            Argument argument = arguments["argument1"];

            argument.Name.Should().Be("argument1");
        }

        [Fact]
        public void HavingArgumentsInstanceWithTwoNamedArguments_WhenRetrievingNonexistentItem_ThenReturnsNull()
        {
            Arguments arguments = new(new[] { "-argument1", "value1", "-argument2", "value2" });

            Argument argument = arguments["nonexistent"];

            argument.Should().BeNull();
        }

        [Theory]
        [InlineData("argument1")]
        [InlineData("argument2")]
        public void HavingArgumentsInstanceWithTwoNamedArguments_WhenRetrievingOneItem_ThenReturnsCorrectArgument(string name)
        {
            Arguments arguments = new(new[] { "-argument1", "value1", "-argument2", "value2" });

            Argument argument = arguments[name];

            argument.Name.Should().Be(name);
        }

        [Fact]
        public void HavingArgumentsInstanceWithTwoUnnamedArguments_WhenRetrievingNonexistentItem_ThenReturnsNull()
        {
            Arguments arguments = new(new[] { "argument1", "argument2" });

            Argument argument = arguments["nonexistent"];

            argument.Should().BeNull();
        }
    }
}