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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.EmploymentWeekTests;

public class ConstructorTests
{
    [Fact]
    public void WhenCreatingEmploymentWeekWithoutDays_ThenContainsNoDays()
    {
        EmploymentWeek employmentWeek = new();

        IEnumerable<DayOfWeek> days = employmentWeek;

        days.Should().BeEmpty();
    }

    [Fact]
    public void WhenCreatingEmploymentWeekWithoutDays_ThenIsDefaultIsFalse()
    {
        EmploymentWeek employmentWeek = new();

        employmentWeek.IsDefault.Should().BeFalse();
    }

    [Theory]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public void WhenCreatingEmploymentWeekWithOneDay_ThenContainsThatDay(DayOfWeek dayOfWeek)
    {
        EmploymentWeek employmentWeek = new(new[] { dayOfWeek });

        IEnumerable<DayOfWeek> days = employmentWeek;

        days.Should().HaveCount(1)
            .And.ContainInOrder(dayOfWeek);
    }

    [Theory]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public void WhenCreatingEmploymentWeekWithOneDay_ThenIsDefaultIsFalse(DayOfWeek dayOfWeek)
    {
        EmploymentWeek employmentWeek = new(new[] { dayOfWeek });

        employmentWeek.IsDefault.Should().BeFalse();
    }

    [Fact]
    public void WhenCreatingEmploymentWeekWithNullCollectionOfDays_ThenContainsNoDays()
    {
        EmploymentWeek employmentWeek = new(null);

        IEnumerable<DayOfWeek> days = employmentWeek;

        days.Should().BeEmpty();
    }

    [Fact]
    public void WhenCreatingEmploymentWeekWithNullCollectionOfDays_ThenIsDefaultIsFalse()
    {
        EmploymentWeek employmentWeek = new(null);

        employmentWeek.IsDefault.Should().BeFalse();
    }
}