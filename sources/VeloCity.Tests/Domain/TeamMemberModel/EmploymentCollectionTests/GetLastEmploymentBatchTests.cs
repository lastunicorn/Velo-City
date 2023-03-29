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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.EmploymentCollectionTests
{
    public class GetLastEmploymentBatchTests
    {
        [Fact]
        public void HavingAnEmptyCollection_WhenRetrieveLastBatch_ThenEmptyCollectionIsReturned()
        {
            EmploymentCollection employmentCollection = new();

            IEnumerable<Employment> actualEmployments = employmentCollection.GetLastEmploymentBatch();

            actualEmployments.Should().BeEmpty();
        }

        [Fact]
        public void HavingOneInfiniteEmployment_WhenRetrieveLastBatch_ThenEmploymentIsReturned()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
            };
            EmploymentCollection employmentCollection = new()
            {
                employment1
            };

            IEnumerable<Employment> actualEmployments = employmentCollection.GetLastEmploymentBatch();

            actualEmployments.Should().BeEquivalentTo(new[] { employment1 });
        }

        [Fact]
        public void HavingOneFiniteEmployment_WhenRetrieveLastBatch_ThenEmploymentIsReturned()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
            };
            EmploymentCollection employmentCollection = new()
            {
                employment1
            };

            IEnumerable<Employment> actualEmployments = employmentCollection.GetLastEmploymentBatch();

            actualEmployments.Should().BeEquivalentTo(new[] { employment1 });
        }

        [Fact]
        public void HavingTwoSeparateEmployments_WhenRetrieveLastBatch_ThenMostRecentEmploymentIsReturned()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 07, 01), new DateTime(2022, 10, 01))
            };
            EmploymentCollection employmentCollection = new()
            {
                employment1,
                employment2
            };

            IEnumerable<Employment> actualEmployments = employmentCollection.GetLastEmploymentBatch();

            actualEmployments.Should().BeEquivalentTo(new[] { employment2 });
        }

        [Fact]
        public void HavingTwoContinuousEmployments_WhenRetrieveLastBatch_ThenBothEmploymentsAreReturnedMostRecentFirst()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 06, 02), new DateTime(2022, 10, 01))
            };
            EmploymentCollection employmentCollection = new()
            {
                employment1,
                employment2
            };

            IEnumerable<Employment> actualEmployments = employmentCollection.GetLastEmploymentBatch();

            actualEmployments.Should().HaveCount(2)
                .And.ContainInOrder(employment2, employment1);
        }

        [Fact]
        public void HavingOneSeparateAndTwoContinuousEmployments_WhenRetrieveLastBatch_ThenTheTwoContinuousEmploymentsAreReturnedMostRecentFirst()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2021, 01, 01), new DateTime(2021, 06, 01))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
            };
            Employment employment3 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 06, 02), new DateTime(2022, 10, 01))
            };
            EmploymentCollection employmentCollection = new()
            {
                employment1,
                employment2,
                employment3
            };

            IEnumerable<Employment> actualEmployments = employmentCollection.GetLastEmploymentBatch();

            actualEmployments.Should().HaveCount(2)
                .And.ContainInOrder(employment3, employment2);
        }

        [Fact]
        public void HavingTwoContinuousAndOneSeparateEmployments_WhenRetrieveLastBatch_ThenTheLastEmploymentsIsReturned()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 06, 02), new DateTime(2022, 10, 01))
            };
            Employment employment3 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2023, 01, 01), new DateTime(2023, 06, 01))
            };
            EmploymentCollection employmentCollection = new()
            {
                employment1,
                employment2,
                employment3
            };

            IEnumerable<Employment> actualEmployments = employmentCollection.GetLastEmploymentBatch();

            actualEmployments.Should().HaveCount(1)
                .And.ContainInOrder(employment3);
        }
    }
}