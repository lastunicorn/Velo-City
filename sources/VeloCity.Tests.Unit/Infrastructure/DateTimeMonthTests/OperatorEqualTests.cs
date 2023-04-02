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

namespace DustInTheWind.VeloCity.Tests.Unit.Infrastructure.DateTimeMonthTests;

public class OperatorEqualTests
{
    [Fact]
    public void HavingOneInstances_WhenComparedWithNull_ThenReturnsFalse()
    {
        DateTimeMonth dateTimeMonth = new(2023, 03);

        bool actual = dateTimeMonth == null;

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingTwoInstancesWithSameValues_WhenCompared_ThenReturnsTrue()
    {
        DateTimeMonth dateTimeMonth1 = new(2023, 03);
        DateTimeMonth dateTimeMonth2 = new(2023, 03);

        bool actual = dateTimeMonth1 == dateTimeMonth2;

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingTwoInstancesWithSameYearButDifferentMonth_WhenCompared_ThenReturnsFalse()
    {
        DateTimeMonth dateTimeMonth1 = new(2023, 03);
        DateTimeMonth dateTimeMonth2 = new(2023, 04);

        bool actual = dateTimeMonth1 == dateTimeMonth2;

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingTwoInstancesWithSameMonthButDifferentYear_WhenCompared_ThenReturnsFalse()
    {
        DateTimeMonth dateTimeMonth1 = new(2023, 03);
        DateTimeMonth dateTimeMonth2 = new(2027, 03);

        bool actual = dateTimeMonth1 == dateTimeMonth2;

        actual.Should().BeFalse();
    }
}