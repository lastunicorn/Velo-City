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

namespace DustInTheWind.VeloCity.Tests.Domain.DateIntervalTests;

public class IsIntersectingWithStartInfiniteTests
{
    [Fact]
    public void HavingInfiniteDateInterval_WhenIntersectingWithStartInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new();

        DateInterval dateInterval2 = new(null, new DateTime(2030, 08, 19));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingStartInfiniteDateInterval_WhenIntersectingWithStartInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(null, new DateTime(2030, 08, 19));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEndInfiniteDateInterval_WhenIntersectingWithStartInfiniteEndingBeforeTheOtherStart_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(null, new DateTime(2000, 08, 19));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingEndInfiniteDateInterval_WhenIntersectingWithStartInfiniteEndingOneDayBeforeTheOtherStart_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(null, new DateTime(2022, 05, 22));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingEndInfiniteDateInterval_WhenIntersectingWithStartInfiniteEndingInSameDayWithTheOtherStart_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(null, new DateTime(2022, 05, 23));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEndInfiniteDateInterval_WhenIntersectingWithStartInfiniteEndingAfterTheOtherStart_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(null, new DateTime(2030, 03, 21));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingFiniteDateInterval_WhenIntersectingWithStartInfiniteThatEndsBeforeTheOtherStart_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23), new DateTime(2040, 02, 15));

        DateInterval dateInterval2 = new(null, new DateTime(2000, 03, 21));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingFiniteDateInterval_WhenIntersectingWithStartInfiniteThatEndsAfterTheOtherStart_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23), new DateTime(2040, 02, 15));

        DateInterval dateInterval2 = new(null, new DateTime(2030, 03, 21));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}