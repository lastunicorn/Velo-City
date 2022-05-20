﻿// Velo City
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
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.EmploymentTests
{
    public class EndDate_GetTests
    {
        [Fact]
        public void HavingFullInfiniteEmployment_ThenEndDateIsNull()
        {
            Employment employment = new();

            employment.EndDate.Should().BeNull();
        }

        [Fact]
        public void HavingEmploymentWithOnlyEndDate_ThenEndDateIsThatValue()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(null, new DateTime(1765, 03, 02))
            };

            employment.EndDate.Should().Be(new DateTime(1765, 03, 02));
        }

        [Fact]
        public void HavingEmploymentWithOnlyStartDate_ThenEndDateIsNull()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(1765, 03, 02))
            };

            employment.EndDate.Should().BeNull();
        }

        [Fact]
        public void HavingEmploymentWithBothStartAndEndDate_ThenEndDateIsThatValue()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(1765, 03, 02), new DateTime(1900, 05, 12))
            };

            employment.EndDate.Should().Be(new DateTime(1900, 05, 12));
        }
    }
}