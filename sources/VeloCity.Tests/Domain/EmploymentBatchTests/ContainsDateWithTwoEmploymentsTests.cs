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

namespace DustInTheWind.VeloCity.Tests.Domain.EmploymentBatchTests
{
    public class ContainsDateWithTwoEmploymentsTests
    {
        private readonly EmploymentBatch employmentBatch;

        public ContainsDateWithTwoEmploymentsTests()
        {
            Employment employment1 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 03, 15), new DateTime(2022, 05, 27))
            };
            Employment employment2 = new()
            {
                TimeInterval = new DateInterval(new DateTime(2022, 05, 28), new DateTime(2022, 07, 16))
            };
            employmentBatch = new(employment2);
            employmentBatch.TryAddBeforeOldest(employment1);
        }

        [Theory]
        [InlineData("101-07-09")]
        [InlineData("2000-02-23")]
        [InlineData("2022-03-14")]
        public void HavingBatchWithTwoEmployments_WhenCheckingIfContainsDateBeforeFirstEmployment_ThenReturnsFalse(string dateAsString)
        {
            DateTime date = DateTime.Parse(dateAsString);
            bool actual = employmentBatch.ContainsDate(date);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("2022-07-17")]
        [InlineData("5072-01-19")]
        public void HavingBatchWithTwoEmployments_WhenCheckingIfContainsDateAfterLastEmployment_ThenReturnsFalse(string dateAsString)
        {
            DateTime date = DateTime.Parse(dateAsString);
            bool actual = employmentBatch.ContainsDate(date);

            actual.Should().BeFalse();
        }

        [Theory]
        [InlineData("2022-03-15")]
        [InlineData("2022-03-25")]
        [InlineData("2022-05-27")]
        public void HavingBatchWithTwoEmployments_WhenCheckingIfContainsDateDuringFirstEmployment_ThenReturnsTrue(string dateAsString)
        {
            DateTime date = DateTime.Parse(dateAsString);
            bool actual = employmentBatch.ContainsDate(date);

            actual.Should().BeTrue();
        }

        [Theory]
        [InlineData("2022-05-28")]
        [InlineData("2022-06-21")]
        [InlineData("2022-07-16")]
        public void HavingBatchWithTwoEmployments_WhenCheckingIfContainsDateDuringSecondEmployment_ThenReturnsTrue(string dateAsString)
        {
            DateTime date = DateTime.Parse(dateAsString);
            bool actual = employmentBatch.ContainsDate(date);

            actual.Should().BeTrue();
        }
    }
}