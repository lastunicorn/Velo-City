// VeloCity
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

public class IsIntersecting_FiniteLimits_WithFiniteLimits_Tests
    {
        [Fact]
    public void HavingFiniteInterval_WhenIntersectingWithFiniteThatStartsAfterTheOtherEnd_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23), new DateTime(2040, 02, 15));

        DateInterval dateInterval2 = new(new DateTime(2050, 03, 21), new DateTime(2103, 07, 05));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingFiniteInterval_WhenIntersectingWithFiniteThatStartsDuringTheOtherInterval_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23), new DateTime(2040, 02, 15));

        DateInterval dateInterval2 = new(new DateTime(2034, 03, 21), new DateTime(2038, 07, 05));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingFiniteInterval_WhenIntersectingWithFiniteThatEndsBeforeTheOtherStart_ThenReturnsFalse()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23), new DateTime(2040, 02, 15));

        DateInterval dateInterval2 = new(new DateTime(2000, 03, 21), new DateTime(2021, 07, 05));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingFiniteInterval_WhenIntersectingWithFiniteThatEndsDuringTheOtherInterval_ThenReturnsTrue()
    {
        DateInterval dateInterval1 = new(new DateTime(2022, 05, 23), new DateTime(2040, 02, 15));

        DateInterval dateInterval2 = new(new DateTime(2021, 03, 21), new DateTime(2038, 07, 05));
        bool actual = dateInterval1.IsIntersecting(dateInterval2);

        actual.Should().BeTrue();
    }
}