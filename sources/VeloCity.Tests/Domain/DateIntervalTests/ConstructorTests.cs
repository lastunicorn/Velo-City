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

using System;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Domain.DateIntervalTests;

public class ConstructorTests
{
    [Fact]
    public void WhenCreatingInstanceWithoutDates_ThenStartDateIsNull()
    {
        DateInterval dateInterval = new();
        dateInterval.StartDate.Should().BeNull();
    }

    [Fact]
    public void WhenCreatingInstanceWithoutDates_ThenEndDateIsNull()
    {
        DateInterval dateInterval = new();
        dateInterval.EndDate.Should().BeNull();
    }

    [Fact]
    public void WhenCreatingInstanceWithStartDate_ThenStartDateHasTheProvidedValue()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15));
        dateInterval.StartDate.Should().Be(new DateTime(2020, 03, 15));
    }

    [Fact]
    public void WhenCreatingInstanceWithStartDate_ThenEndDateIsNull()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15));
        dateInterval.EndDate.Should().BeNull();
    }

    [Fact]
    public void WhenCreatingInstanceWithEndDate_ThenStartDateHasTheProvidedValue()
    {
        DateInterval dateInterval = new(null, new DateTime(2020, 03, 15));
        dateInterval.StartDate.Should().BeNull();
    }

    [Fact]
    public void WhenCreatingInstanceWithEndDate_ThenEndDateIsNull()
    {
        DateInterval dateInterval = new(null, new DateTime(2020, 03, 15));
        dateInterval.EndDate.Should().Be(new DateTime(2020, 03, 15));
    }

    [Fact]
    public void WhenCreatingInstanceWithBothDates_ThenStartDateHasTheProvidedValue()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12));
        dateInterval.StartDate.Should().Be(new DateTime(2020, 03, 15));
    }

    [Fact]
    public void WhenCreatingInstanceWithBothDates_ThenEndDateHasTheProvidedValue()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12));
        dateInterval.EndDate.Should().Be(new DateTime(2022, 04, 12));
    }

    [Fact]
    public void WhenCreatingInstanceWithSameValueForStartAndEndDates_ThenStartDateHasTheProvidedValue()
    {
        DateInterval dateInterval = new(new DateTime(2021, 07, 05), new DateTime(2021, 07, 05));
        dateInterval.StartDate.Should().Be(new DateTime(2021, 07, 05));
    }

    [Fact]
    public void WhenCreatingInstanceWithSameValueForStartAndEndDates_ThenEndDateHasTheProvidedValue()
    {
        DateInterval dateInterval = new(new DateTime(2021, 07, 05), new DateTime(2021, 07, 05));
        dateInterval.EndDate.Should().Be(new DateTime(2021, 07, 05));
    }

    [Fact]
    public void WhenCreatingInstanceWithReversedDates_ThenThrows()
    {
        Action action = () => new DateInterval(new DateTime(2022, 01, 04), new DateTime(2021, 07, 05));

        action.Should().Throw<ArgumentException>();
    }
}