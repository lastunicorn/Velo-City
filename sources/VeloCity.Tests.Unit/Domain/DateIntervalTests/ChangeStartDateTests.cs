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

public class ChangeStartDateTests
{
    private readonly DateInterval dateInterval;

    public ChangeStartDateTests()
    {
        dateInterval = new DateInterval(new DateTime(2023, 04, 03), new DateTime(2023, 04, 09));
    }

    [Fact]
    public void HavingFiniteInterval_WhenMakingInfiniteStart_ThenOriginalInstanceIsNotChanged()
    {
        dateInterval.ChangeStartDate(null);

        DateInterval expected = new(new DateTime(2023, 04, 03), new DateTime(2023, 04, 09));
        dateInterval.Should().Be(expected);
    }

    [Fact]
    public void HavingFiniteInterval_WhenMakingInfiniteStart_ThenNewInstanceHasNullStart()
    {
        DateInterval actual = dateInterval.ChangeStartDate(null);

        actual.StartDate.Should().BeNull();
    }

    [Fact]
    public void HavingFiniteInterval_WhenMakingInfiniteStart_ThenNewInstanceHasSameEnd()
    {
        DateInterval actual = dateInterval.ChangeStartDate(null);

        actual.EndDate.Should().Be(new DateTime(2023, 04, 09));
    }

    [Fact]
    public void HavingFiniteInterval_WhenChangingStartDate_ThenOriginalInstanceIsNotChanged()
    {
        dateInterval.ChangeStartDate(new DateTime(2022, 01, 19));

        DateInterval expected = new(new DateTime(2023, 04, 03), new DateTime(2023, 04, 09));
        dateInterval.Should().Be(expected);
    }

    [Fact]
    public void HavingFiniteInterval_WhenChangingStartDate_ThenNewInstanceHasNewStart()
    {
        DateInterval actual = dateInterval.ChangeStartDate(new DateTime(2022, 01, 19));

        actual.StartDate.Should().Be(new DateTime(2022, 01, 19));
    }

    [Fact]
    public void HavingFiniteInterval_WhenChangingStartDate_ThenNewInstanceHasSameEnd()
    {
        DateInterval actual = dateInterval.ChangeStartDate(new DateTime(2022, 01, 19));

        actual.EndDate.Should().Be(new DateTime(2023, 04, 09));
    }

    [Fact]
    public void HavingFiniteInterval_WhenChangingStartDateToBeAfterEndDate_ThenThrows()
    {
        Action action = () =>
        {
            DateInterval actual = dateInterval.ChangeStartDate(new DateTime(2023, 04, 20));
        };

        action.Should().Throw<ArgumentException>();
    }
}