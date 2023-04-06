﻿using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteEnd_WithInfiniteStart_Tests
{
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
}