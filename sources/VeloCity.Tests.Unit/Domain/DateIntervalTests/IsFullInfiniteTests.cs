// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsFullInfiniteTests
{
    [Fact]
    public void HavingInstanceWithoutDates_ThenIsFullInfiniteIsTrue()
    {
        DateInterval dateInterval = new();
        dateInterval.IsFullInfinite.Should().BeTrue();
    }

    [Fact]
    public void HavingInstanceWithOnlyStartDate_ThenIsFullInfiniteIsFalse()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15));
        dateInterval.IsFullInfinite.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithOnlyEndDate_ThenIsFullInfiniteIsFalse()
    {
        DateInterval dateInterval = new(null, new DateTime(2020, 03, 15));
        dateInterval.IsFullInfinite.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithBothDates_ThenIsFullInfiniteIsFalse()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12));
        dateInterval.IsFullInfinite.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithSameValueForStartAndEndDates_ThenIsFullInfiniteIsFalse()
    {
        DateInterval dateInterval = new(new DateTime(2021, 07, 05), new DateTime(2021, 07, 05));
        dateInterval.IsFullInfinite.Should().BeFalse();
    }
}