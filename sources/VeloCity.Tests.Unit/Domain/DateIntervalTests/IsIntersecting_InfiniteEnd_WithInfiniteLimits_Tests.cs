using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteEnd_WithInfiniteLimits_Tests
{
    [Fact]
    public void HavingEndInfiniteDateInterval_WhenIntersectingWithHullInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new();
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}