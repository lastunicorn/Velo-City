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

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.EmploymentCollectionTests
{
    public class GetEmploymentForTests
    {
        [Theory]
        [InlineData("2000-03-23")]
        [InlineData("2022-05-06")]
        [InlineData("2501-03-13")]
        public void HavingAnEmptyCollection_WhenRetrievingEmploymentForAnyDate_ThenNullIsReturned(string dateAsString)
        {
            EmploymentCollection employmentCollection = new();

            DateTime date = DateTime.Parse(dateAsString);
            Employment actualEmployment = employmentCollection.GetEmploymentFor(date);

            actualEmployment.Should().BeNull();
        }

        [Theory]
        [InlineData("2000-03-23")]
        [InlineData("2002-05-06")]
        [InlineData("2021-12-31")]
        public void HavingOneEmployment_WhenRetrievingEmploymentForDateBeforeEmployment_ThenNullIsReturned(string dateAsString)
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
            Employment actualEmployment = employmentCollection.GetEmploymentFor(date);

            actualEmployment.Should().BeNull();
        }

        [Theory]
        [InlineData("2022-06-02")]
        [InlineData("2029-05-06")]
        [InlineData("2521-12-31")]
        public void HavingOneEmployment_WhenRetrievingEmploymentForDateAfterEmployment_ThenNullIsReturned(string dateAsString)
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
            Employment actualEmployment = employmentCollection.GetEmploymentFor(date);

            actualEmployment.Should().BeNull();
        }

        [Theory]
        [InlineData("2022-01-02")]
        [InlineData("2022-05-06")]
        [InlineData("2022-06-01")]
        public void HavingOneEmployment_WhenRetrievingEmploymentForDateDuringEmployment_ThenEmploymentIsReturned(string dateAsString)
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
            Employment actualEmployment = employmentCollection.GetEmploymentFor(date);

            actualEmployment.Should().Be(employment1);
        }

        [Fact]
        public void HavingTwoSeparatedEmployments_WhenRetrievingEmploymentForDateBeforeEmployments_ThenNullIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2020, 01, 01));

            actualEmployment.Should().BeNull();
        }

        [Fact]
        public void HavingTwoSeparatedEmployments_WhenRetrievingEmploymentForDateBetweenEmployments_ThenNullIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2022, 10, 01));

            actualEmployment.Should().BeNull();
        }

        [Fact]
        public void HavingTwoSeparatedEmployments_WhenRetrievingEmploymentForDateAfterEmployments_ThenNullIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2025, 01, 01));

            actualEmployment.Should().BeNull();
        }

        [Fact]
        public void HavingTwoSeparatedEmployments_WhenRetrievingEmploymentForDateDuringFirstEmployment_ThenFirstEmploymentIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2022, 03, 01));

            actualEmployment.Should().Be(employment1);
        }

        [Fact]
        public void HavingTwoSeparatedEmployments_WhenRetrievingEmploymentForDateDuringSecondEmployment_ThenSecondEmploymentIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2023, 03, 01));

            actualEmployment.Should().Be(employment2);
        }

        [Fact]
        public void HavingTwoContinuousEmployments_WhenRetrievingEmploymentForDateBeforeEmployments_ThenNullIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2021, 03, 01));

            actualEmployment.Should().BeNull();
        }

        [Fact]
        public void HavingTwoContinuousEmployments_WhenRetrievingEmploymentForDateAfterEmployments_ThenEmptyCollectionIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2023, 03, 01));

            actualEmployment.Should().BeNull();
        }

        [Fact]
        public void HavingTwoContinuousEmployments_WhenRetrievingEmploymentForDateDuringFirstEmployment_ThenFirstEmploymentIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2022, 03, 01));

            actualEmployment.Should().Be(employment1);
        }

        [Fact]
        public void HavingTwoContinuousEmployments_WhenRetrievingEmploymentForDateDuringSecondEmployment_ThenSecondEmploymentIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2022, 07, 01));

            actualEmployment.Should().Be(employment2);
        }

        [Fact]
        public void HavingTwoContinuousEmploymentsOutOfOrder_WhenRetrievingEmploymentForDateDuringSecondEmployment_ThenSecondEmploymentIsReturned()
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

            Employment actualEmployment = employmentCollection.GetEmploymentFor(new DateTime(2022, 07, 01));

            actualEmployment.Should().Be(employment2);
        }
    }
}