using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class Intersect_InfiniteStart_WithInfiniteLimits_Tests
{
    [Fact]
    public void HavingIntervalWithInfiniteStart_WhenIntersectingWithFullInfinite_ThenReturnsInitialInterval()
    {
        // ==========================]-------------------------
        // ====================================================

        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new();

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        actual.Value.Should().Be(dateInterval1);
    }
}