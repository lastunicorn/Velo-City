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

namespace DustInTheWind.VeloCity.Tests.Infrastructure.DateMonthTests;

public class ToStringTests
{
    [Theory]
    [InlineData(2025, 04, "2025 04")]
    [InlineData(3458, 01, "3458 01")]
    [InlineData(1, 03, "0001 03")]
    [InlineData(100, 12, "0100 12")]
    public void HavingAnInstance_WhenSerialized_ThenReturnsYearAndMonthAsNumbers(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        string actual = dateMonth.ToString();

        actual.Should().Be(expected);
    }
}