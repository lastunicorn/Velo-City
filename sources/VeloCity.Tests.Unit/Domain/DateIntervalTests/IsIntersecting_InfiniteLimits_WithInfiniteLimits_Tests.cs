using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteLimits_WithInfiniteLimits_Tests
{
    [Fact]
    public void HavingInfiniteDateInterval_WhenIntersectingWithHullInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new();

        DateInterval dateInterval2 = new();
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}