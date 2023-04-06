using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteEnd_WithFiniteLimits_Tests
{
    [Fact]
    public void HavingEndInfiniteInterval_WhenIntersectingWithFiniteThatEndsBeforeTheOtherStart_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(1999, 12, 22), new DateTime(2021, 08, 19));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingEndInfiniteInterval_WhenIntersectingWithFiniteThatEndsOneDayBeforeTheOtherStart_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(1999, 12, 22), new DateTime(2022, 05, 22));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingEndInfiniteInterval_WhenIntersectingWithFiniteThatEndsSameDayWithTheOtherStart_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(1999, 12, 22), new DateTime(2022, 05, 23));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEndInfiniteInterval_WhenIntersectingWithFiniteThatEndsAfterTheOtherStart_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(1999, 12, 22), new DateTime(2038, 07, 05));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}