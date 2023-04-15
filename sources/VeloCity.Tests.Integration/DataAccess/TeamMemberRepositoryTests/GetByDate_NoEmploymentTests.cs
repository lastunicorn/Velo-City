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

public class GetByDate_NoEmploymentTests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\TeamMemberRepositoryTests";

    [Fact]
    public async Task HavingDatabaseWithTeamMembersWithoutEmployment_WhenGetByDate_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date.no-employment.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.VeloCityDbContext);
                
                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.GetByDate(new DateTime(2022, 06, 15));

                teamMembers.Should().BeEmpty();
            });
    }
}