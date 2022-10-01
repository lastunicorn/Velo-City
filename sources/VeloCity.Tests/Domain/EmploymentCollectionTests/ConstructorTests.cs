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
using System.Linq;
using DustInTheWind.VeloCity.Domain;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.EmploymentCollectionTests
{
    public class ConstructorTests
    {
        [Fact]
        public void WhenInstantiateCollectionWithNoEmployment_ThenCollectionIsEmpty()
        {
            EmploymentCollection employmentCollection = new();

            employmentCollection.Should().BeEmpty();
        }

        [Fact]
        public void WhenInstantiateCollectionWithEmptyEmploymentCollection_ThenCollectionIsEmpty()
        {
            EmploymentCollection employmentCollection = new(Enumerable.Empty<Employment>());

            employmentCollection.Should().BeEmpty();
        }

        [Fact]
        public void WhenInstantiateCollectionWithOneEmployment_ThenCollectionContainsEmployment()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
            };
            EmploymentCollection employmentCollection = new(new[] { employment1 });

            employmentCollection.Should().HaveCount(1)
                .And.ContainInOrder(employment1);
        }

        [Fact]
        public void WhenInstantiateCollectionWithTwoEmploymentsInChronologicalOrder_ThenCollectionContainsEmploymentsInReverseChronologicalOrder()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 06, 02), new DateTime(2022, 10, 01))
            };
            EmploymentCollection employmentCollection = new(new[] { employment1, employment2 });

            employmentCollection.Should().HaveCount(2)
                .And.ContainInOrder(employment2, employment1);
        }

        [Fact]
        public void WhenInstantiateCollectionWithTwoEmploymentsInReverseChronologicalOrder_ThenCollectionContainsEmploymentsInReverseChronologicalOrder()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 06, 02), new DateTime(2022, 10, 01))
            };
            EmploymentCollection employmentCollection = new(new[] { employment2, employment1 });

            employmentCollection.Should().HaveCount(2)
                .And.ContainInOrder(employment2, employment1);
        }
    }
}