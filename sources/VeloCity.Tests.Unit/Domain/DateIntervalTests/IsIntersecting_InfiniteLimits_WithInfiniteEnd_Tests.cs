using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteLimits_WithInfiniteEnd_Tests
{
    [Fact]
    public void HavingInfiniteDateInterval_WhenIntersectingWithEndInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new();

        DateInterval dateInterval2 = new(null, new DateTime(2030, 08, 19));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}