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

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.EmploymentCollectionTests;

public class GetFirstEmploymentTests
{
    [Fact]
    public void HavingEmptyCollection_WhenRequestingFirstEmployment_ThenReturnsNull()
    {
        EmploymentCollection employmentCollection = new();

        Employment actual = employmentCollection.GetFirstEmployment();

        actual.Should().BeNull();
    }

    [Fact]
    public void HavingCollectionWithOneFiniteEmployment_WhenRequestingFirstEmployment_ThenReturnsEmployment()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1
        };

        Employment actual = employmentCollection.GetFirstEmployment();

        actual.Should().Be(employment1);
    }

    [Fact]
    public void HavingCollectionWithOneInfiniteStartingEmployment_WhenRequestingFirstEmployment_ThenReturnsEmployment()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(null, new DateTime(2022, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1
        };

        Employment actual = employmentCollection.GetFirstEmployment();

        actual.Should().Be(employment1);
    }

    [Fact]
    public void HavingCollectionWithTwoFiniteEmployments_WhenRequestingFirstEmployment_ThenReturnsOldestEmployment()
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

        Employment actual = employmentCollection.GetFirstEmployment();

        actual.Should().Be(employment1);
    }

    [Fact]
    public void HavingCollectionWithOneInfiniteAndOneFiniteEmployments_WhenRequestingFirstEmployment_ThenReturnsTheInfiniteEmployment()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(null, new DateTime(2022, 06, 01))
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

        Employment actual = employmentCollection.GetFirstEmployment();

        actual.Should().Be(employment1);
    }
}