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
    public class CountTests
    {
        [Fact]
        public void HavingArgumentsInstanceWithNoArgument_ThenCountIs0()
        {
            Arguments arguments = new(Array.Empty<string>());

            arguments.Count.Should().Be(0);
        }

        [Fact]
        public void HavingArgumentsInstanceWithOneArgument_ThenCountIs1()
        {
            Arguments arguments = new(new[] { "argument1" });

            arguments.Count.Should().Be(1);
        }

        [Fact]
        public void HavingArgumentsInstanceWithTwoArguments_ThenCountIs2()
        {
            Arguments arguments = new(new[] { "argument1", "argument2" });

            arguments.Count.Should().Be(2);
        }

        [Fact]
        public void HavingArgumentsInstanceWithThreeArguments_ThenCountIs3()
        {
            Arguments arguments = new(new[] { "argument1", "argument2", "argument3" });

            arguments.Count.Should().Be(3);
        }
    }
}