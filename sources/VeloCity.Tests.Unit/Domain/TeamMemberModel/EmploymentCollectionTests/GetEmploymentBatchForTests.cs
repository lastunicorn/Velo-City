// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.EmploymentCollectionTests;

public class GetEmploymentBatchForTests
{
    [Theory]
    [InlineData("2000-03-23")]
    [InlineData("2022-05-06")]
    [InlineData("2501-03-13")]
    public void HavingAnEmptyCollection_WhenRetrieveBatchForAnyDate_ThenEmptyCollectionIsReturned(string dateAsString)
    {
        EmploymentCollection employmentCollection = new();

        DateTime date = DateTime.Parse(dateAsString);
        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(date);

        actualEmployments.Should().BeEmpty();
    }

    [Theory]
    [InlineData("2000-03-23")]
    [InlineData("2002-05-06")]
    [InlineData("2021-12-31")]
    public void HavingOneEmployment_WhenRetrieveBatchForDateBeforeEmployment_ThenEmptyCollectionIsReturned(string dateAsString)
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1
        };

        DateTime date = DateTime.Parse(dateAsString);
        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(date);

        actualEmployments.Should().BeEmpty();
    }

    [Theory]
    [InlineData("2022-06-02")]
    [InlineData("2029-05-06")]
    [InlineData("2521-12-31")]
    public void HavingOneEmployment_WhenRetrieveBatchForDateAfterEmployment_ThenEmptyCollectionIsReturned(string dateAsString)
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1
        };

        DateTime date = DateTime.Parse(dateAsString);
        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(date);

        actualEmployments.Should().BeEmpty();
    }

    [Theory]
    [InlineData("2022-01-01")]
    [InlineData("2022-05-06")]
    [InlineData("2022-06-01")]
    public void HavingOneEmployment_WhenRetrieveBatchForDateDuringEmployment_ThenEmploymentIsReturned(string dateAsString)
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1
        };

        DateTime date = DateTime.Parse(dateAsString);
        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(date);

        actualEmployments.Should().BeEquivalentTo(new[] { employment1 });
    }

    [Fact]
    public void HavingTwoSeparatedEmployments_WhenRetrieveBatchForDateBeforeEmployments_ThenEmptyCollectionIsReturned()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2023, 01, 01), new DateTime(2023, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1,
            employment2
        };

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2020, 01, 01));

        actualEmployments.Should().BeEmpty();
    }

    [Fact]
    public void HavingTwoSeparatedEmployments_WhenRetrieveBatchForDateBetweenEmployments_ThenEmptyCollectionIsReturned()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2023, 01, 01), new DateTime(2023, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1,
            employment2
        };

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2022, 10, 01));

        actualEmployments.Should().BeEmpty();
    }

    [Fact]
    public void HavingTwoSeparatedEmployments_WhenRetrieveBatchForDateAfterEmployments_ThenEmptyCollectionIsReturned()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2023, 01, 01), new DateTime(2023, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1,
            employment2
        };

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2025, 01, 01));

        actualEmployments.Should().BeEmpty();
    }

    [Fact]
    public void HavingTwoSeparatedEmployments_WhenRetrieveBatchForDateDuringFirstEmployment_ThenFirstEmploymentIsReturned()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2023, 01, 01), new DateTime(2023, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1,
            employment2
        };

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2022, 03, 01));

        actualEmployments.Should().BeEquivalentTo(new[] { employment1 });
    }

    [Fact]
    public void HavingTwoSeparatedEmployments_WhenRetrieveBatchForDateDuringSecondEmployment_ThenSecondEmploymentIsReturned()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2023, 01, 01), new DateTime(2023, 06, 01))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1,
            employment2
        };

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2023, 03, 01));

        actualEmployments.Should().BeEquivalentTo(new[] { employment2 });
    }

    [Fact]
    public void HavingTwoContinuousEmployments_WhenRetrieveBatchForDateBeforeEmployments_ThenEmptyCollectionIsReturned()
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

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2021, 03, 01));

        actualEmployments.Should().BeEmpty();
    }

    [Fact]
    public void HavingTwoContinuousEmployments_WhenRetrieveBatchForDateAfterEmployments_ThenEmptyCollectionIsReturned()
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

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2023, 03, 01));

        actualEmployments.Should().BeEmpty();
    }

    [Fact]
    public void HavingTwoContinuousEmployments_WhenRetrieveBatchForDateDuringFirstEmployment_ThenBothEmploymentsAreReturned()
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

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2022, 03, 01));

        actualEmployments.Should().HaveCount(2)
            .And.ContainInOrder(employment2, employment1);
    }

    [Fact]
    public void HavingTwoContinuousEmploymentsOutOfOrder_WhenRetrieveBatchForDateDuringSecondEmployment_ThenBothEmploymentsAreReturned()
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
            employment2,
            employment1
        };

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2022, 07, 01));

        actualEmployments.Should().HaveCount(2)
            .And.ContainInOrder(employment2, employment1);
    }

    [Fact]
    public void HavingTwoContinuousEmployments_WhenRetrieveBatchForDateDuringSecondEmployment_ThenBothEmploymentsAreReturned()
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

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2022, 07, 01));

        actualEmployments.Should().HaveCount(2)
            .And.ContainInOrder(employment2, employment1);
    }

    [Fact]
    public void HavingTwoContinuousEmploymentsWithSecondOneBingInfinite_WhenRetrieveBatchForDateDuringSecondEmployment_ThenBothEmploymentsAreReturned()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 01, 01), new DateTime(2022, 06, 01))
        };
        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2022, 06, 02))
        };
        EmploymentCollection employmentCollection = new()
        {
            employment1,
            employment2
        };

        IEnumerable<Employment> actualEmployments = employmentCollection.GetEmploymentBatchFor(new DateTime(2022, 07, 01));

        actualEmployments.Should().HaveCount(2)
            .And.ContainInOrder(employment2, employment1);
    }
}