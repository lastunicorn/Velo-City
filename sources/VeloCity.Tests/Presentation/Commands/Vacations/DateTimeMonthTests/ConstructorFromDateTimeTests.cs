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
using DustInTheWind.VeloCity.Presentation.Commands.Vacations;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Presentation.Commands.Vacations.DateTimeMonthTests
{
    public class ConstructorFromDateTimeTests
    {
        [Fact]
        public void WhenCreatingNewInstanceWithSpecificDateTime_ThenYearIsSetToTheDateTimeYearValue()
        {
            DateTime dateTime = new(2020, 09, 03);
            DateTimeMonth dateTimeMonth = new(dateTime);

            dateTimeMonth.Year.Should().Be(2020);
        }

        [Fact]
        public void WhenCreatingNewInstanceWithSpecificDateTime_ThenMonthIsSetToTheDateTimeMonthValue()
        {
            DateTime dateTime = new(2020, 09, 03);
            DateTimeMonth dateTimeMonth = new(dateTime);

            dateTimeMonth.Month.Should().Be(09);
        }
    }
}