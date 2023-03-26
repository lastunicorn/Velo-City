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
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.EmploymentTests
{
    public class StartDate_SetTests
    {
        [Fact]
        public void HavingFullInfiniteEmployment_WhenStartDateIsSetToNull_ThenStartDateIsNull()
        {
            Employment employment = new();

            employment.StartDate = null;

            employment.StartDate.Should().BeNull();
        }

        [Fact]
        public void HavingFullInfiniteEmployment_WhenStartDateIsSetToNull_ThenTimeIntervalHasNullStartDate()
        {
            Employment employment = new();

            employment.StartDate = null;

            employment.TimeInterval.StartDate.Should().BeNull();
        }

        [Fact]
        public void HavingFullInfiniteEmployment_WhenStartDateIsSetToFiniteValue_ThenStartDateIsThatValue()
        {
            Employment employment = new();

            employment.StartDate = new DateTime(1601, 09, 12);

            employment.StartDate.Should().Be(new DateTime(1601, 09, 12));
        }

        [Fact]
        public void HavingFullInfiniteEmployment_WhenStartDateIsSetToFiniteValue_ThenTimeIntervalStartDateIsThatValue()
        {
            Employment employment = new();

            employment.StartDate = new DateTime(1601, 09, 12);

            employment.TimeInterval.StartDate.Should().Be(new DateTime(1601, 09, 12));
        }

        [Fact]
        public void HavingFiniteEmployment_WhenStartDateIsSetToFiniteValue_ThenStartDateIsThatValue()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(2000, 01, 01), new DateTime(2011, 02, 02))
            };

            employment.StartDate = new DateTime(1601, 09, 12);

            employment.StartDate.Should().Be(new DateTime(1601, 09, 12));
        }

        [Fact]
        public void HavingFiniteEmployment_WhenStartDateIsSetToFiniteValue_ThenTimeIntervalStartDateIsThatValue()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(2000, 01, 01), new DateTime(2011, 02, 02))
            };

            employment.StartDate = new DateTime(1601, 09, 12);

            employment.TimeInterval.StartDate.Should().Be(new DateTime(1601, 09, 12));
        }

        [Fact]
        public void HavingFiniteEmployment_WhenStartDateIsSetToNull_ThenStartDateIsNull()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(2000, 01, 01), new DateTime(2011, 02, 02))
            };

            employment.StartDate = null;

            employment.StartDate.Should().BeNull();
        }

        [Fact]
        public void HavingFiniteEmployment_WhenStartDateIsSetToNull_ThenTimeIntervalStartDateIsNull()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(2000, 01, 01), new DateTime(2011, 02, 02))
            };

            employment.StartDate = null;

            employment.TimeInterval.StartDate.Should().BeNull();
        }
    }
}