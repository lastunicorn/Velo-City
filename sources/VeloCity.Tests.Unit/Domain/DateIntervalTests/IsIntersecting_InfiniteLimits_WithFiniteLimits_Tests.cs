using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteLimits_WithFiniteLimits_Tests
{
    [Fact]
    public void HavingInfiniteInterval_WhenIntersectingWithFinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new();

        DateInterval dateInterval2 = new(new DateTime(1999, 12, 22), new DateTime(2030, 08, 19));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}