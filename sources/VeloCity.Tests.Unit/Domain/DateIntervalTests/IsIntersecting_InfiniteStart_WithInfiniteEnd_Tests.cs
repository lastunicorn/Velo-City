using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class IsIntersecting_InfiniteStart_WithInfiniteEnd_Tests
{
    [Fact]
    public void HavingStartInfiniteDateInterval_WhenIntersectingWithEndInfiniteStartingAfterTheOtherEnd_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(2040, 08, 19));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingStartInfiniteDateInterval_WhenIntersectingWithEndInfiniteStartingOneDayAfterTheOtherEnd_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(2022, 05, 24));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingStartInfiniteDateInterval_WhenIntersectingWithEndInfiniteStartingInSameDayWithTheOtherEnd_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(2022, 05, 23));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingStartInfiniteDateInterval_WhenIntersectingWithEndInfiniteStartingBeforeTheOtherEnd_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));

        DateInterval dateInterval2 = new(new DateTime(2021, 03, 21));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}