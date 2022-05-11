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

namespace DustInTheWind.VeloCity.Tests.Presentation.Infrastructure.ArgumentsTests
{
    public class IndexerByIndexTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(-1)]
        public void HavingArgumentsInstanceWithNoArgument_WhenRetrievingNonexistentItem_ThenReturnsNull(int index)
        {
            Arguments arguments = new(Array.Empty<string>());

            Argument argument = arguments[index];

            argument.Should().BeNull();
        }

        [Fact]
        public void HavingArgumentsInstanceWithOneArgument_WhenRetrievingIt_ThenReturnsTheArgument()
        {
            Arguments arguments = new(new[] { "argument1" });

            Argument argument = arguments[0];

            argument.Value.Should().Be("argument1");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(-1)]
        public void HavingArgumentsInstanceWithOneArgument_WhenRetrievingNonexistentItem_ThenReturnsNull(int index)
        {
            Arguments arguments = new(new[] { "argument1" });

            Argument argument = arguments[index];

            argument.Should().BeNull();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(-1)]
        public void HavingArgumentsInstanceWithTwoArguments_WhenRetrievingNonexistentItem_ThenReturnsNull(int index)
        {
            Arguments arguments = new(new[] { "argument1", "argument2" });

            Argument argument = arguments[index];

            argument.Should().BeNull();
        }
    }
}