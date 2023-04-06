using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class Intersect_InfiniteEnd_WithInfiniteStart_Tests
{
    [Fact]
    public void HavingIntervalWithInfiniteEnd_WhenIntersectingWithInfiniteStartThatEndsBeforeTheOtherStart_ThenReturnsNull()
    {
        // --------------------------[=========================
        // =====================]------------------------------

        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new(null, new DateTime(2000, 08, 19));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        actual.Should().BeNull();
    }

    [Fact]
    public void HavingIntervalWithInfiniteEnd_WhenIntersectingWithInfiniteStartThatEndsOneDayBeforeTheOtherStart_ThenReturnsNull()
    {
        // --------------------------[=========================
        // =========================]--------------------------

        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new(null, new DateTime(2022, 05, 22));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        actual.Should().BeNull();
    }

    [Fact]
    public void HavingIntervalWithInfiniteEnd_WhenIntersectingWithInfiniteStartThatEndsInSameDayWithTheOtherStart_ThenReturnsIntervalContainingOnlyTheCommonDay()
    {
        // --------------------------[=========================
        // ==========================]-------------------------

        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new(null, new DateTime(2022, 05, 23));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        DateInterval expected = new(new DateTime(2022, 05, 23), new DateTime(2022, 05, 23));
        actual.Value.Should().Be(expected);
    }

    [Fact]
    public void HavingIntervalWithInfiniteEnd_WhenIntersectingWithInfiniteStartThatEndsAfterTheOtherStart_ThenReturnsIntervalWithFirstStartAndSecondEnd()
    {
        // --------------------------[=========================
        // ===============================]--------------------

        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new(null, new DateTime(2030, 03, 21));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        DateInterval expected = new(new DateTime(2022, 05, 23), new DateTime(2030, 03, 21));
        actual.Value.Should().Be(expected);
    }
}