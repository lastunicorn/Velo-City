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

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.EmploymentTests
{
    public class EndDate_SetTests
    {
        [Fact]
        public void HavingFullInfiniteEmployment_WhenEndDateIsSetToNull_ThenEndDateIsNull()
        {
            Employment employment = new();

            employment.EndDate = null;

            employment.EndDate.Should().BeNull();
        }

        [Fact]
        public void HavingFullInfiniteEmployment_WhenEndDateIsSetToNull_ThenTimeIntervalHasNullEndDate()
        {
            Employment employment = new();

            employment.EndDate = null;

            employment.TimeInterval.EndDate.Should().BeNull();
        }

        [Fact]
        public void HavingFullInfiniteEmployment_WhenEndDateIsSetToFiniteValue_ThenEndDateIsThatValue()
        {
            Employment employment = new();

            employment.EndDate = new DateTime(1601, 09, 12);

            employment.EndDate.Should().Be(new DateTime(1601, 09, 12));
        }

        [Fact]
        public void HavingFullInfiniteEmployment_WhenEndDateIsSetToFiniteValue_ThenTimeIntervalEndDateIsThatValue()
        {
            Employment employment = new();

            employment.EndDate = new DateTime(1601, 09, 12);

            employment.TimeInterval.EndDate.Should().Be(new DateTime(1601, 09, 12));
        }

        [Fact]
        public void HavingFiniteEmployment_WhenEndDateIsSetToFiniteValue_ThenEndDateIsThatValue()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(2000, 01, 01), new DateTime(2011, 02, 02))
            };

            employment.EndDate = new DateTime(2022, 09, 12);

            employment.EndDate.Should().Be(new DateTime(2022, 09, 12));
        }

        [Fact]
        public void HavingFiniteEmployment_WhenEndDateIsSetToFiniteValue_ThenTimeIntervalEndDateIsThatValue()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(2000, 01, 01), new DateTime(2011, 02, 02))
            };

            employment.EndDate = new DateTime(2022, 09, 12);

            employment.TimeInterval.EndDate.Should().Be(new DateTime(2022, 09, 12));
        }

        [Fact]
        public void HavingFiniteEmployment_WhenEndDateIsSetToNull_ThenEndDateIsNull()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(2000, 01, 01), new DateTime(2011, 02, 02))
            };

            employment.EndDate = null;

            employment.EndDate.Should().BeNull();
        }

        [Fact]
        public void HavingFiniteEmployment_WhenEndDateIsSetToNull_ThenTimeIntervalEndDateIsNull()
        {
            Employment employment = new()
            {
                TimeInterval = new DateInterval(new DateTime(2000, 01, 01), new DateTime(2011, 02, 02))
            };

            employment.EndDate = null;

            employment.TimeInterval.EndDate.Should().BeNull();
        }
    }
}