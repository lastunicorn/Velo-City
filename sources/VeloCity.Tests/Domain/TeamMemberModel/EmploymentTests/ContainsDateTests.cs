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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.EmploymentTests;

public class ContainsDateTests
{
    [Fact]
    public void HavingFullInfiniteEmployment_WhenCheckingIfItContainsADate_ThenReturnsTrue()
    {
        Employment employment = new();

        bool actual = employment.ContainsDate(new DateTime(1999, 05, 09));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEmploymentWithOnlyStartDate_WhenCheckIfADateBeforeStartDateIsContained_ThenReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2020, 03, 15))
        };

        bool actual = employment.ContainsDate(new DateTime(1999, 04, 17));

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingEmploymentWithOnlyStartDate_WhenCheckIfAStartDateIsContained_ThenReturnsTrue()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2020, 03, 15))
        };

        bool actual = employment.ContainsDate(new DateTime(2020, 03, 15));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEmploymentWithOnlyStartDate_WhenCheckIfADateAfterStartDateIsContained_ThenReturnsTrue()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2020, 03, 15))
        };

        bool actual = employment.ContainsDate(new DateTime(2021, 09, 30));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEmploymentWithOnlyEndDate_WhenCheckIfADateBeforeEndDateIsContained_ThenReturnsTrue()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(null, new DateTime(2020, 03, 15))
        };

        bool actual = employment.ContainsDate(new DateTime(1999, 04, 17));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEmploymentWithOnlyEndDate_WhenCheckIfAEndDateIsContained_ThenReturnsTrue()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(null, new DateTime(2020, 03, 15))
        };

        bool actual = employment.ContainsDate(new DateTime(2020, 03, 15));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEmploymentWithOnlyEndDate_WhenCheckIfADateAfterEndDateIsContained_ThenReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(null, new DateTime(2020, 03, 15))
        };

        bool actual = employment.ContainsDate(new DateTime(2021, 08, 17));

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingEmploymentWithBothDates_WhenCheckIfADateBeforeStartDateIsContained_ThenReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12))
        };

        bool actual = employment.ContainsDate(new DateTime(1999, 04, 17));

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingEmploymentWithBothDates_WhenCheckIfADateBetweenStartDateAndEndDateIsContained_ThenReturnsTrue()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12))
        };

        bool actual = employment.ContainsDate(new DateTime(2021, 02, 27));

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEmploymentWithBothDates_WhenCheckIfADateAfterEndDateIsContained_ThenReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2020, 03, 15), new DateTime(2022, 04, 12))
        };

        bool actual = employment.ContainsDate(new DateTime(2050, 04, 17));

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingEmploymentWithSameValueForStartAndEndDates_WhenCheckIfTheDateIsContained_ThenReturnsTrue()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2021, 07, 05), new DateTime(2021, 07, 05))
        };

        bool actual = employment.ContainsDate(new DateTime(2021, 07, 05));

        actual.Should().BeTrue();
    }
}