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
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.DateIntervalTests
{
    public class DoesContinueWithTests
    {
        [Fact]
        public void HavingInfiniteDateInterval_WhenCheckingIfItContinuesWithFiniteInterval_ReturnsFalse()
        {
            DateInterval dateInterval = new();

            DateInterval dateInterval2 = new(new DateTime(5400, 12, 14));
            bool actual = dateInterval.DoesContinueWith(dateInterval2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingDateIntervalWithMaximumEndDate_WhenCheckingIfItContinuesWithFiniteInterval_ReturnsFalse()
        {
            DateInterval dateInterval = new(null, DateTime.MaxValue);

            DateInterval dateInterval2 = new(new DateTime(5400, 12, 14));
            bool actual = dateInterval.DoesContinueWith(dateInterval2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingFiniteDateInterval_WhenCheckingIfItContinuesWithStartInfiniteInterval_ReturnsFalse()
        {
            DateInterval dateInterval = new(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04));

            DateInterval dateInterval2 = new(new DateTime(5400, 12, 14));
            bool actual = dateInterval.DoesContinueWith(dateInterval2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingFiniteDateInterval_WhenCheckingIfItContinuesWithIntervalStartingInTheFuture_ReturnsFalse()
        {
            DateInterval dateInterval = new(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04));

            DateInterval dateInterval2 = new(new DateTime(5400, 12, 14));
            bool actual = dateInterval.DoesContinueWith(dateInterval2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingFiniteDateInterval_WhenCheckingIfItContinuesWithIntervalStartingDuringInterval_ReturnsFalse()
        {
            DateInterval dateInterval = new(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04));

            DateInterval dateInterval2 = new(new DateTime(1950, 12, 14));
            bool actual = dateInterval.DoesContinueWith(dateInterval2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingFiniteDateInterval_WhenCheckingIfItContinuesWithIntervalStartingBeforeInterval_ReturnsFalse()
        {
            DateInterval dateInterval = new(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04));

            DateInterval dateInterval2 = new(new DateTime(1800, 12, 14));
            bool actual = dateInterval.DoesContinueWith(dateInterval2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingFiniteDateInterval_WhenCheckingIfItContinuesWithIntervalStartingFromTheEndDayOfTheInterval_ReturnsFalse()
        {
            DateInterval dateInterval = new(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04));

            DateInterval dateInterval2 = new(new DateTime(2002, 08, 04));
            bool actual = dateInterval.DoesContinueWith(dateInterval2);

            actual.Should().BeFalse();
        }

        [Fact]
        public void HavingFiniteDateInterval_WhenCheckingIfItContinuesWithIntervalStartingNextDayAfterInterval_ReturnsTrue()
        {
            DateInterval dateInterval = new(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04));

            DateInterval dateInterval2 = new(new DateTime(2002, 08, 05));
            bool actual = dateInterval.DoesContinueWith(dateInterval2);

            actual.Should().BeTrue();
        }
    }
}