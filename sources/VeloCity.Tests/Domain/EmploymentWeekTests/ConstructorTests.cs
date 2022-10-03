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
using System.Collections.Generic;
using DustInTheWind.VeloCity.Domain;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.EmploymentWeekTests
{
    public class ConstructorTests
    {
        [Fact]
        public void WhenCreatingEmploymentWeekWithoutDays_ThenContainsMondayToFridayDays()
        {
            EmploymentWeek employmentWeek = new();

            IEnumerable<DayOfWeek> days = employmentWeek;

            days.Should().HaveCount(5)
                .And.ContainInOrder(new[]
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday
                });
        }

        [Fact]
        public void WhenCreatingEmploymentWeekWithoutDays_ThenIsDefaultIsTrue()
        {
            EmploymentWeek employmentWeek = new();

            employmentWeek.IsDefault.Should().BeTrue();
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
        public void WhenCreatingEmploymentWeekWithNullCollectionOfDays_ThenContainsMondayToFridayDays()
        {
            EmploymentWeek employmentWeek = new(null);

            IEnumerable<DayOfWeek> days = employmentWeek;

            days.Should().HaveCount(5)
                .And.ContainInOrder(new[]
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday
                });
        }

        [Fact]
        public void WhenCreatingEmploymentWeekWithNullCollectionOfDays_ThenIsDefaultIsTrue()
        {
            EmploymentWeek employmentWeek = new(null);

            employmentWeek.IsDefault.Should().BeTrue();
        }
    }
}