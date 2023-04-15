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

public class FindTests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\TeamMemberRepositoryTests";

    [Fact]
    public async Task HavingEmptyDatabase_WhenFind_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-find.empty.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.Find("anything");

                teamMembers.Should().BeEmpty();
            });
    }

    [Fact]
    public async Task HavingDatabaseWithTeamMembers_WhenFindNonExistingName_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-find.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.Find("blabla");

                teamMembers.Should().BeEmpty();
            });
    }

    [Fact]
    public async Task HavingDatabaseWithTeamMembers_WhenFindNameMatchingTwoTeamMembers_ThenReturnsTwoTeamMembers()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-find.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.Find("Vale");

                int[] expectedIds = { 1, 2 };
                teamMembers.Select(x => x.Id).Should().Equal(expectedIds);
            });
    }

    [Fact]
    public async Task HavingDatabaseWithTeamMembers_WhenFindNameMatchingTwoTeamMembersCaseInsensitive_ThenReturnsTwoTeamMembers()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-find.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.Find("vale");

                int[] expectedIds = { 1, 2 };
                teamMembers.Select(x => x.Id).Should().Equal(expectedIds);
            });
    }

    [Fact]
    public async Task HavingDatabaseWithTeamMembers_WhenFindNameMatchingOneTeamMembersCaseInsensitive_ThenReturnsTheTeamMember()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-find.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.Find("mar");

                int[] expectedIds = { 2 };
                teamMembers.Select(x => x.Id).Should().Equal(expectedIds);
            });
    }
}