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

using System;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Domain.DateIntervalTests;

public class IsZeroTests
{
    [Fact]
    public void HavingInstanceWithoutDates_ThenIsZeroIsFalse()
    {
        DateInterval dateInterval = new();
        dateInterval.IsZero.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithOnlyStartDate_ThenIsZeroIsFalse()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15));
        dateInterval.IsZero.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithOnlyEndDate_ThenIsZeroIsFalse()
    {
        DateInterval dateInterval = new(null, new DateTime(2020, 03, 15));
        dateInterval.IsZero.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithBothDates_ThenIsZeroIsFalse()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12));
        dateInterval.IsZero.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithSameValueForStartAndEndDates_ThenIsZeroIsTrue()
    {
        DateInterval dateInterval = new(new DateTime(2021, 07, 05), new DateTime(2021, 07, 05));
        dateInterval.IsZero.Should().BeTrue();
    }
}