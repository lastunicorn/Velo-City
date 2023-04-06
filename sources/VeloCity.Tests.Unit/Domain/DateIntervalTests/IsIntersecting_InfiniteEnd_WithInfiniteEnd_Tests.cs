using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteEnd_WithInfiniteEnd_Tests
{
    [Fact]
    public void HavingEndInfiniteDateInterval_WhenIntersectingWithEndInfinite_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(2030, 08, 19));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}