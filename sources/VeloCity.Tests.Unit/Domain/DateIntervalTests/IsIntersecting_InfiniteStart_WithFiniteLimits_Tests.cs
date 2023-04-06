using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteStart_WithFiniteLimits_Tests
{
    [Fact]
    public void HavingStartInfiniteInterval_WhenIntersectingWithFiniteStartingAfterTheOtherEnd_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(2025, 12, 22), new DateTime(2038, 07, 05));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingStartInfiniteInterval_WhenIntersectingWithFiniteStartingOneDayAfterTheOtherEnd_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(2022, 05, 24), new DateTime(2038, 07, 05));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingStartInfiniteInterval_WhenIntersectingWithFiniteStartingInSameDayWithTheOtherEnd_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(2022, 05, 23), new DateTime(2038, 07, 05));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingStartInfiniteInterval_WhenIntersectingWithFiniteStartingBeforeTheOtherEnd_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(2021, 03, 21), new DateTime(2038, 07, 05));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}