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
using System.Linq;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.EmploymentBatchTests
{
    public class TryAddTests
    {
        [Fact]
        public void HavingEmptyInstance_WhenTryToAddNull_ThenThrows()
        {
            EmploymentBatch employmentBatch = new();

            Action action = () => employmentBatch.TryAddBeforeOldest(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void HavingEmptyInstance_WhenTryToAddEmployment_ThenInstanceContainsOnlyThatEmployment()
        {
            EmploymentBatch employmentBatch = new();
            Employment employment = new();

            employmentBatch.TryAddBeforeOldest(employment);

            List<Employment> actual = employmentBatch.ToList();

            actual.Should().HaveCount(1);
            actual.Should().ContainInOrder(employment);
        }

        [Fact]
        public void HavingEmptyInstance_WhenTryToAddEmployment_ThenReturnsTrue()
        {
            EmploymentBatch employmentBatch = new();
            Employment employment = new();

            bool success = employmentBatch.TryAddBeforeOldest(employment);

            success.Should().BeTrue();
        }

        [Fact]
        public void HavingInstanceWithOneFiniteEmployment_WhenTryToAddEmploymentInTheDistantFuture_ThenInstanceContainsOnlyTheInitialEmployment()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 02, 25), new DateTime(2022, 03, 20))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 04, 01))
            };
            EmploymentBatch employmentBatch = new(employment1);

            employmentBatch.TryAddBeforeOldest(employment2);

            employmentBatch.Should().HaveCount(1);
            employmentBatch.Should().ContainInOrder(employment1);
        }

        [Fact]
        public void HavingInstanceWithOneFiniteEmployment_WhenTryToAddEmploymentInTheDistantFuture_ThenReturnsFalse()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 02, 25), new DateTime(2022, 03, 20))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 04, 01))
            };
            EmploymentBatch employmentBatch = new(employment1);

            bool success = employmentBatch.TryAddBeforeOldest(employment2);

            success.Should().BeFalse();
        }

        [Fact]
        public void HavingInstanceWithOneFiniteEmployment_WhenTryToAddEmploymentImmediatelyBeforeExistingOne_ThenInstanceContainsTheTwoEmploymentsInOrder()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 02, 25), new DateTime(2022, 03, 20))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 21), new DateTime(2022, 02, 24))
            };
            EmploymentBatch employmentBatch = new(employment1);

            employmentBatch.TryAddBeforeOldest(employment2);

            employmentBatch.Should().HaveCount(2);
            employmentBatch.Should().ContainInOrder(employment1, employment2);
        }

        [Fact]
        public void HavingInstanceWithOneFiniteEmployment_WhenTryToAddEmploymentImmediatelyBeforeExistingOne_ThenReturnsTrue()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 02, 25), new DateTime(2022, 03, 20))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 21), new DateTime(2022, 02, 24))
            };
            EmploymentBatch employmentBatch = new(employment1);

            bool success = employmentBatch.TryAddBeforeOldest(employment2);

            success.Should().BeTrue();
        }
    }
}