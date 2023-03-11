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

using DustInTheWind.VeloCity.Infrastructure;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Infrastructure.DateMonthTests
{
    public class ConstructorWithoutParametersTests
    {
        DateMonth dateMonth;

        public ConstructorWithoutParametersTests()
        {
            dateMonth = new DateMonth();
        }

        [Fact]
        public void WhenCreateInstanceWithoutParameters_ThenYearIsZero()
        {
            dateMonth.Year.Should().Be(0);
        }

        [Fact]
        public void WhenCreateInstanceWithoutParameters_ThenMonthIsOne()
        {
            dateMonth.Month.Should().Be(1);
        }
    }
}
