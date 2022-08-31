using System;
using DustInTheWind.VeloCity.Domain;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.SprintListTests
{
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
                    EmploymentWeek = new EmploymentWeek(),
                    HoursPerDay = 8
                }
            };
        }
    }
}