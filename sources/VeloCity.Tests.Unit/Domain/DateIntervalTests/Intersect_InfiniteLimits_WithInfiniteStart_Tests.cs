using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class Intersect_InfiniteLimits_WithInfiniteStart_Tests
{
    [Fact]
    public void HavingInfiniteDateInterval_WhenIntersectingWithInfiniteStart_ThenReturnsTheInfiniteStartInterval()
    {
        // ====================================================
        // ==========================]-------------------------

        DateInterval dateInterval1 = new();
        DateInterval dateInterval2 = new(null, new DateTime(2030, 08, 19));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        actual.Value.Should().Be(dateInterval2);
    }
}