// Velo City
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
using DustInTheWind.VeloCity.Cli.Presentation.Commands.Vacations;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Presentation.Commands.Vacations.DateTimeMonthTests
{
    public class OperatorGreaterThanDateTimeTests
    {
        [Fact]
        public void HavingOneInstance_WhenComparedToDateTimeFromSameMonth_ReturnsFalse()
        {
            DateTimeMonth dateTimeMonth = new(2022, 01);
            DateTime dateTime = new(2022, 01, 14);

            bool actual = dateTimeMonth > dateTime;

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingOneInstance_WhenComparedToDateTimeFromNextMonth_ReturnsFalse()
        {
            DateTimeMonth dateTimeMonth = new(2022, 01);
            DateTime dateTime = new(2022, 02, 14);

            bool actual = dateTimeMonth > dateTime;

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingOneInstance_WhenComparedToDateTimeFromNextYear_ReturnsFalse()
        {
            DateTimeMonth dateTimeMonth = new(2022, 01);
            DateTime dateTime = new(2023, 01, 14);

            bool actual = dateTimeMonth > dateTime;

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingOneInstance_WhenComparedToDateTimeFromPreviousMonth_ReturnsTrue()
        {
            DateTimeMonth dateTimeMonth = new(2022, 05);
            DateTime dateTime = new(2022, 04, 14);

            bool actual = dateTimeMonth > dateTime;

            actual.Should().BeTrue();
        }

        [Fact]
        public void HavingOneInstance_WhenComparedToDateTimeFromPreviousYear_ReturnsTrue()
        {
            DateTimeMonth dateTimeMonth = new(2022, 05);
            DateTime dateTime = new(2021, 05, 14);

            bool actual = dateTimeMonth > dateTime;

            actual.Should().BeTrue();
        }
    }
}