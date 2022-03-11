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
using Xunit;

namespace DustInTheWind.VeloCity.Tests.ArgumentsTests
{
    public class OneOrdinalArgumentTests
    {
        private readonly Arguments arguments;

        public OneOrdinalArgumentTests()
        {
            string[] args = { "param1" };

            arguments = new Arguments(args);
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenArgumentsContainsOneArgument()
        {
            Assert.Equal(1, arguments.Count);
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenArgumentHasTypeOrdinal()
        {
            Assert.Equal(ArgumentType.Ordinal, arguments[0].Type);
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenArgumentHasNullName()
        {
            Assert.Null(arguments[0].Name);
        }

        [Fact]
        public void HavingArgsStringWithOneOrdinalArgument_WhenParsed_ThenArgumentHasCorrectValue()
        {
            Assert.Equal("param1", arguments[0].Value);
        }
    }
}
