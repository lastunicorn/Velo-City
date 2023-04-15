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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.TeamMemberTests;

public class HasActiveEmploymentTests
{
    [Fact]
    public void HavingInstanceWithUninitializedEmployments_ThenDoesNotHaveActiveEmployment()
    {
        TeamMember teamMember = new();

        teamMember.HasActiveEmployment.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithNullEmployments_ThenDoesNotHaveActiveEmployment()
    {
        TeamMember teamMember = new()
        {
            Employments = null
        };

        teamMember.HasActiveEmployment.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithEmptyEmployments_ThenDoesNotHaveActiveEmployment()
    {
        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection()
        };

        teamMember.HasActiveEmployment.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithOneEmploymentInThePastThatEnded_ThenDoesNotHaveActiveEmployment()
    {
        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    EndDate = DateTime.Today.AddDays(-1)
                }
            }
        };

        teamMember.HasActiveEmployment.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithOneEmploymentInTheFutureThatEnded_ThenDoesNotHaveActiveEmployment()
    {
        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(2)
                }
            }
        };

        teamMember.HasActiveEmployment.Should().BeFalse();
    }

    [Fact]
    public void HavingInstanceWithOneEmploymentInTheFutureNotEnded_ThenHasActiveEmployment()
    {
        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = DateTime.Today.AddDays(1)
                }
            }
        };

        teamMember.HasActiveEmployment.Should().BeTrue();
    }
}