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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.EmploymentBatchTests;

public class ConstructorTests
{
    [Fact]
    public void WhenCreatingNewEmptyInstance_ThenInstanceContainsNoItems()
    {
        EmploymentBatch employmentBatch = new();

        bool existsItems = employmentBatch.Any();

        existsItems.Should().BeFalse();
    }

    [Fact]
    public void WhenCreatingNewInstanceWithOneEmployment_ThenInstanceContainsOnlyThatEmployment()
    {
        Employment employment = new();
        EmploymentBatch employmentBatch = new(employment);

        List<Employment> actual = employmentBatch.ToList();

        actual.Should().HaveCount(1);
        actual.Should().ContainInOrder(employment);
    }

    [Fact]
    public void WhenCreatingNewInstanceWithNullEmployment_ThenThrows()
    {
        Action action = () => new EmploymentBatch(null);

        action.Should().Throw<ArgumentNullException>();
    }
}