// VeloCity
// Copyright (C) 2022 Dust in the Wind
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

public class ChangeEndDateTests
{
    private readonly DateInterval dateInterval;

    public ChangeEndDateTests()
    {
        dateInterval = new DateInterval(new DateTime(2023, 04, 03), new DateTime(2023, 04, 09));
    }

    [Fact]
    public void HavingFiniteInterval_WhenMakingInfiniteEnd_ThenOriginalInstanceIsNotChanged()
    {
        dateInterval.ChangeEndDate(null);

        DateInterval expected = new(new DateTime(2023, 04, 03), new DateTime(2023, 04, 09));
        dateInterval.Should().Be(expected);
    }

    [Fact]
    public void HavingFiniteInterval_WhenMakingInfiniteEnd_ThenNewInstanceHasNullEnd()
    {
        DateInterval actual = dateInterval.ChangeEndDate(null);

        actual.EndDate.Should().BeNull();
    }

    [Fact]
    public void HavingFiniteInterval_WhenMakingInfiniteEnd_ThenNewInstanceHasSameStart()
    {
        DateInterval actual = dateInterval.ChangeEndDate(null);

        actual.StartDate.Should().Be(new DateTime(2023, 04, 03));
    }

    [Fact]
    public void HavingFiniteInterval_WhenChangingEndDate_ThenOriginalInstanceIsNotChanged()
    {
        dateInterval.ChangeEndDate(new DateTime(2030, 10, 19));

        DateInterval expected = new(new DateTime(2023, 04, 03), new DateTime(2023, 04, 09));
        dateInterval.Should().Be(expected);
    }

    [Fact]
    public void HavingFiniteInterval_WhenChangingEndDate_ThenNewInstanceHasNewEnd()
    {
        DateInterval actual = dateInterval.ChangeEndDate(new DateTime(2030, 10, 19));

        actual.EndDate.Should().Be(new DateTime(2030, 10, 19));
    }

    [Fact]
    public void HavingFiniteInterval_WhenChangingEndDate_ThenNewInstanceHasSameStart()
    {
        DateInterval actual = dateInterval.ChangeEndDate(new DateTime(2030, 10, 19));

        actual.StartDate.Should().Be(new DateTime(2023, 04, 03));
    }

    [Fact]
    public void HavingFiniteInterval_WhenChangingEndDateToBeBeforeStartDate_ThenThrows()
    {
        Action action = () =>
        {
            DateInterval actual = dateInterval.ChangeEndDate(new DateTime(2023, 04, 01));
        };

        action.Should().Throw<ArgumentException>();
    }
}