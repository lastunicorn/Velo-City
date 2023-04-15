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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.EmploymentBatchTests;

public class StartDateTests
{
    [Fact]
    public void HavingAnEmptyBatch_ThenStartDateIsNull()
    {
        EmploymentBatch employmentBatch = new();

        employmentBatch.StartDate.Should().BeNull();
    }

    [Fact]
    public void HavingABatchWithASingleEmployment_ThenStartDateIsTheStartDateOfEmployment()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2020, 03, 12), new DateTime(2020, 05, 07))
        };
        EmploymentBatch employmentBatch = new(employment);

        employmentBatch.StartDate.Should().Be(new DateTime(2020, 03, 12));
    }

    [Fact]
    public void HavingABatchWithTwoEmployments_ThenStartDateIsTheStartDateOfTheEarliestEmployment()
    {
        Employment employment1 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2020, 03, 12), new DateTime(2020, 05, 07))
        };
        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2020, 05, 08), new DateTime(2020, 10, 13))
        };
        EmploymentBatch employmentBatch = new(employment2);
        employmentBatch.TryAddBeforeOldest(employment1);

        employmentBatch.StartDate.Should().Be(new DateTime(2020, 03, 12));
    }
}