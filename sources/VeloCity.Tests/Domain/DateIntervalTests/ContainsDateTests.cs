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

namespace DustInTheWind.VeloCity.Tests.Domain.DateIntervalTests;

public class ContainsDateTests
{
    [Fact]
    public void HavingInstanceWithoutDates_WhenCheckIfADateIsContained_ThenReturnsTrue()
    {
        DateInterval dateInterval = new();

        bool actual = dateInterval.ContainsDate(new DateTime(2001, 04, 17));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingInstanceWithOnlyStartDate_WhenCheckIfADateBeforeStartDateIsContained_ThenReturnsFalse()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15));

        bool actual = dateInterval.ContainsDate(new DateTime(1999, 04, 17));

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithOnlyStartDate_WhenCheckIfAStartDateIsContained_ThenReturnsTrue()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15));

        bool actual = dateInterval.ContainsDate(new DateTime(2020, 03, 15));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingInstanceWithOnlyStartDate_WhenCheckIfADateAfterStartDateIsContained_ThenReturnsTrue()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15));

        bool actual = dateInterval.ContainsDate(new DateTime(2021, 09, 30));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingInstanceWithOnlyEndDate_WhenCheckIfADateBeforeEndDateIsContained_ThenReturnsTrue()
    {
        DateInterval dateInterval = new(null, new DateTime(2020, 03, 15));

        bool actual = dateInterval.ContainsDate(new DateTime(1999, 04, 17));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingInstanceWithOnlyEndDate_WhenCheckIfAEndDateIsContained_ThenReturnsTrue()
    {
        DateInterval dateInterval = new(null, new DateTime(2020, 03, 15));

        bool actual = dateInterval.ContainsDate(new DateTime(2020, 03, 15));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingInstanceWithOnlyEndDate_WhenCheckIfADateAfterEndDateIsContained_ThenReturnsFalse()
    {
        DateInterval dateInterval = new(null, new DateTime(2020, 03, 15));

        bool actual = dateInterval.ContainsDate(new DateTime(2021, 08, 17));

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithBothDates_WhenCheckIfADateBeforeStartDateIsContained_ThenReturnsFalse()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12));

        bool actual = dateInterval.ContainsDate(new DateTime(1999, 04, 17));

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithBothDates_WhenCheckIfADateBetweenStartDateAndEndDateIsContained_ThenReturnsTrue()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12));

        bool actual = dateInterval.ContainsDate(new DateTime(2021, 02, 27));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingInstanceWithBothDates_WhenCheckIfADateAfterEndDateIsContained_ThenReturnsFalse()
    {
        DateInterval dateInterval = new(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12));

        bool actual = dateInterval.ContainsDate(new DateTime(2050, 04, 17));

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithSameValueForStartAndEndDates_WhenCheckIfTheDateIsContained_ThenReturnsTrue()
    {
        DateInterval dateInterval = new(new DateTime(2021, 07, 05), new DateTime(2021, 07, 05));

        bool actual = dateInterval.ContainsDate(new DateTime(2021, 07, 05));

        actual.Should().BeTrue();
    }
}