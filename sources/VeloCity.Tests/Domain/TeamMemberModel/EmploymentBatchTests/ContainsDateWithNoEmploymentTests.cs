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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.EmploymentBatchTests
{
    public class ContainsDateWithNoEmploymentTests
    {
        [Theory]
        [InlineData("2022-05-13")]
        [InlineData("200-01-20")]
        [InlineData("5430-07-10")]
        public void HavingEmptyBatch_WhenCheckingIfContainsAnyDate_ThenReturnsFalse(string dateAsString)
        {
            EmploymentBatch employmentBatch = new();

            DateTime date = DateTime.Parse(dateAsString);
            bool actual = employmentBatch.ContainsDate(date);

            actual.Should().BeFalse();
        }
    }
}