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

using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersectingWithFullInfiniteTests
{
    [Fact]
    public void HavingInfiniteDateInterval_WhenIntersectingWithHullInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new();

        bool actual = IntersectWithFullInfinite(dateInterval1);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingStartInfiniteDateInterval_WhenIntersectingWithHullInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        bool actual = IntersectWithFullInfinite(dateInterval1);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEndInfiniteDateInterval_WhenIntersectingWithHullInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        bool actual = IntersectWithFullInfinite(dateInterval1);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingFiniteDateInterval_WhenIntersectingWithHullInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23), new DateTime(2040, 02, 15));

        bool actual = IntersectWithFullInfinite(dateInterval1);

        actual.Should().BeTrue();
    }

    private static bool IntersectWithFullInfinite(DateInterval dateInterval1)
    {
        DateInterval dateInterval2 = new();
        return dateInterval1.IsIntersecting(dateInterval2);
    }
}