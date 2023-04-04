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

using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.TeamMemberRepositoryTests;

public class GetTests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\TeamMemberRepositoryTests";

    [Fact]
    public async Task HavingDatabaseWithTeamMembers_WhenGetExistingId_ThenReturnsTeamMemberWithSpecifiedId()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                TeamMember teamMember = await teamMemberRepository.Get(2);

                teamMember.Id.Should().Be(2);
            });
    }

    [Fact]
    public async Task HavingDatabaseWithTeamMembers_WhenGetNonExistingId_ThenReturnsNull()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                TeamMember teamMember = await teamMemberRepository.Get(45521);

                teamMember.Should().BeNull();
            });
    }

    [Fact]
    public async Task HavingEmptyDatabase_WhenGetNonExistingId_ThenReturnsNull()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get.empty.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);

                TeamMember teamMember = await teamMemberRepository.Get(1);

                teamMember.Should().BeNull();
            });
    }
}