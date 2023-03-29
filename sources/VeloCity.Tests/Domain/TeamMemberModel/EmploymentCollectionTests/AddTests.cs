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

public class AddTests
{
    [Fact]
    public void HavingEmptyCollection_WhenAddingNullEmployment_ThenThrows()
    {
        EmploymentCollection employmentCollection = new();

        Action action = () => employmentCollection.Add(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingEmptyCollection_WhenAddingEmployment_ThenCollectionContainsEmployment()
    {
        EmploymentCollection employmentCollection = new();

        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        employmentCollection.Add(employment1);

        employmentCollection.Should().HaveCount(1)
            .And.ContainInOrder(employment1);
    }

    [Fact]
    public void HavingCollectionWithOneEmployment_WhenAddingEmploymentChronologicallyAfterExistingEmployment_ThenEmploymentIsAddedBeforeExistingEmployment()
    {
        Employment existingEmployment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        EmploymentCollection employmentCollection = new(new[] { existingEmployment });

        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2023, 01, 01), new DateTime(2023, 06, 01))
        };
        employmentCollection.Add(employment1);

        employmentCollection.Should().HaveCount(2)
            .And.ContainInOrder(employment1, existingEmployment);
    }

    [Fact]
    public void HavingCollectionWithOneEmployment_WhenAddingEmploymentChronologicallyBeforeExistingEmployment_ThenEmploymentIsAddedAfterExistingEmployment()
    {
        Employment existingEmployment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        EmploymentCollection employmentCollection = new(new[] { existingEmployment });

        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2021, 01, 01), new DateTime(2021, 06, 01))
        };
        employmentCollection.Add(employment1);

        employmentCollection.Should().HaveCount(2)
            .And.ContainInOrder(existingEmployment, employment1);
    }
}