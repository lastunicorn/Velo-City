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

using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.TeamMemberRepositoryTests;

public class GetByDate_TwoTeamMembersTests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\TeamMemberRepositoryTests";

    [Fact]
    public async Task HavingDateBeforeBothEmployments_WhenGetByDate_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date.two.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.GetByDate(new DateTime(2000, 01, 15));

                teamMembers.Should().BeEmpty();
            });
    }

    [Fact]
    public async Task HavingDateDuringFirstTeamMemberEmployment_WhenGetByDate_ThenReturnsFirstTeamMember()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date.two.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.GetByDate(new DateTime(2022, 03, 15));

                int[] expectedIds = { 1 };
                teamMembers.Select(x => x.Id).Should().Equal(expectedIds);
            });
    }

    [Fact]
    public async Task HavingDateDuringBothTeamMemberEmployments_WhenGetByDate_ThenReturnsBothTeamMembers()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date.two.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.GetByDate(new DateTime(2022, 06, 15));

                int[] expectedIds = { 1, 2 };
                teamMembers.Select(x => x.Id).Should().Equal(expectedIds);
            });
    }

    [Fact]
    public async Task HavingDateDuringSecondTeamMemberEmployment_WhenGetByDate_ThenReturnsSecondTeamMember()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date.two.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.GetByDate(new DateTime(2022, 08, 15));

                int[] expectedIds = { 2 };
                teamMembers.Select(x => x.Id).Should().Equal(expectedIds);
            });
    }

    [Fact]
    public async Task HavingDateAfterBothTeamMemberEmployment_WhenGetByDate_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date.two.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.GetByDate(new DateTime(2022, 11, 15));

                teamMembers.Should().BeEmpty();
            });
    }
}