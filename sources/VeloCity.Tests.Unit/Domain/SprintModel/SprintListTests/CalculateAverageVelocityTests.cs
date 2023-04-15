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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.SprintModel.SprintListTests;

public class CalculateAverageVelocityTests
{
    [Fact]
    public void HavingEmptyList_WhenCalculatingAverageVelocity_ThenReturnsNullVelocity()
    {
        SprintList sprintList = new(Array.Empty<Sprint>());

        Velocity velocity = sprintList.CalculateAverageVelocity();

        velocity.Should().Be(Velocity.Empty);
    }

    [Fact]
    public void HavingOneSprintOf2WeeksWith40SPAndOneStandardTeamMember_WhenCalculatingAverageVelocity_ThenReturnsHalfSP()
    {
        Sprint sprint = new()
        {
            ActualStoryPoints = 40,
            DateInterval = new DateInterval(new DateTime(2022, 06, 01), new DateTime(2022, 06, 14))
        };
        TeamMember teamMember = CreateStandardTeamMember();
        sprint.AddSprintMember(teamMember);
        SprintList sprintList = new(new[] { sprint });

        Velocity velocity = sprintList.CalculateAverageVelocity();

        velocity.Should().Be((Velocity)0.5);
    }

    [Fact]
    public void HavingOneSprintOf2WeeksWith80SPAndOneStandardTeamMember_WhenCalculatingAverageVelocity_ThenReturns1SP()
    {
        Sprint sprint = new()
        {
            ActualStoryPoints = 80,
            DateInterval = new DateInterval(new DateTime(2022, 06, 01), new DateTime(2022, 06, 14))
        };
        TeamMember teamMember = CreateStandardTeamMember();
        sprint.AddSprintMember(teamMember);
        SprintList sprintList = new(new[] { sprint });

        Velocity velocity = sprintList.CalculateAverageVelocity();

        velocity.Should().Be((Velocity)1);
    }

    [Fact]
    public void HavingTwoSprintsOf80SPAnd40SPAndOneStandardTeamMember_WhenCalculatingAverageVelocity_ThenReturns3QuartersSP()
    {
        TeamMember teamMember = CreateStandardTeamMember();
        Sprint sprint1 = new()
        {
            ActualStoryPoints = 80,
            DateInterval = new DateInterval(new DateTime(2022, 06, 01), new DateTime(2022, 06, 14))
        };
        sprint1.AddSprintMember(teamMember);
        Sprint sprint2 = new()
        {
            ActualStoryPoints = 40,
            DateInterval = new DateInterval(new DateTime(2022, 06, 15), new DateTime(2022, 06, 28))
        };
        sprint2.AddSprintMember(teamMember);
        SprintList sprintList = new(new[] { sprint1, sprint2 });

        Velocity velocity = sprintList.CalculateAverageVelocity();

        velocity.Should().Be((Velocity)0.75);
    }

    private static TeamMember CreateStandardTeamMember()
    {
        return new TeamMember
        {
            Employments = CreateStandardEmployment()
        };
    }

    private static EmploymentCollection CreateStandardEmployment()
    {
        return new EmploymentCollection
        {
            new()
            {
                EmploymentWeek = EmploymentWeek.NewDefault,
                HoursPerDay = 8
            }
        };
    }
}