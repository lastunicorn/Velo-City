﻿// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DateIntervalTests;

public class Intersect_InfiniteStart_WithFiniteLimits_Tests
{
    [Fact]
    public void HavingIntervalWithInfiniteStart_WhenIntersectingWithFiniteStartingAfterTheOtherEnd_ThenReturnsNull()
    {
        // ==========================]-------------------------
        // ---------------------------------[==========]-------

        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new(new DateTime(2025, 12, 22), new DateTime(2038, 07, 05));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        actual.Should().BeNull();
    }

    [Fact]
    public void HavingIntervalWithInfiniteStart_WhenIntersectingWithFiniteStartingOneDayAfterTheOtherEnd_ThenReturnsNull()
    {
        // ==========================]-------------------------
        // ---------------------------[==========]-------------

        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new(new DateTime(2022, 05, 24), new DateTime(2038, 07, 05));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        actual.Should().BeNull();
    }

    [Fact]
    public void HavingIntervalWithInfiniteStart_WhenIntersectingWithFiniteStartingInSameDayWithTheOtherEnd_ThenReturnsIntervalForThatCommonDay()
    {
        // ==========================]-------------------------
        // --------------------------[==========]--------------

        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new(new DateTime(2022, 05, 23), new DateTime(2038, 07, 05));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        DateInterval expected = new(new DateTime(2022, 05, 23), new DateTime(2022, 05, 23));
        actual.Value.Should().Be(expected);
    }

    [Fact]
    public void HavingIntervalWithInfiniteStart_WhenIntersectingWithFiniteIntervalThatStartsBeforeTheOtherEndAndEndsAfterIt_ThenReturnsIntervalWithSecondStartAndFirstEnd()
    {
        // ==========================]-------------------------
        // ---------------------[==========]-------------------

        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new(new DateTime(2021, 03, 21), new DateTime(2038, 07, 05));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        DateInterval expected = new(new DateTime(2021, 03, 21), new DateTime(2022, 05, 23));
        actual.Value.Should().Be(expected);
    }

    [Fact]
    public void HavingIntervalWithInfiniteStart_WhenIntersectingWithFiniteIntervalThatEndsBeforeTheOtherEnd_ThenReturnsSecondInterval()
    {
        // ==========================]-------------------------
        // -----------[==========]-----------------------------

        DateInterval dateInterval1 = new(null, new DateTime(2022, 05, 23));
        DateInterval dateInterval2 = new(new DateTime(2010, 03, 21), new DateTime(2020, 07, 05));

        DateInterval? actual = DateInterval.Intersect(dateInterval1, dateInterval2);

        actual.Value.Should().Be(dateInterval2);
    }
}