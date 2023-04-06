using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteStart_WithInfiniteStart_Tests
{
    [Fact]
    public void HavingStartInfiniteDateInterval_WhenIntersectingWithStartInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(null, new DateTime(2030, 08, 19));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}