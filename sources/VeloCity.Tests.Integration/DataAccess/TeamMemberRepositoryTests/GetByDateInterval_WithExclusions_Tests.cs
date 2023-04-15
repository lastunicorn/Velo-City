﻿// VeloCity
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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.TeamMemberRepositoryTests;

public class GetByDateInterval_WithExclusions_Tests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\TeamMemberRepositoryTests";

    [Fact]
    public async Task HavingThreeTeamMembersAndOneExclusionThatMatchesTwoOfThem_WhenGetByDateInterval_ThenReturnsOneTeamMember()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date-interval.with-exclusions.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.DbContext);

                DateInterval dateInterval = DateInterval.FullInfinite;
                string[] excludedNames = { "mari" };
                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.GetByDateInterval(dateInterval, excludedNames);

                int[] expectedIds = { 1 };
                teamMembers.Select(x => x.Id).Should().Equal(expectedIds);
            });
    }

    [Fact]
    public async Task HavingThreeTeamMembersAndTwoExclusionThatMatchesTwoOfThem_WhenGetByDateInterval_ThenReturnsOneTeamMember()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date-interval.with-exclusions.json")
            .Execute(async context =>
            {
                TeamMemberRepository teamMemberRepository = new(context.DbContext);

                DateInterval dateInterval = DateInterval.FullInfinite;
                string[] excludedNames = { "valentin", "marius" };
                IEnumerable<TeamMember> teamMembers = await teamMemberRepository.GetByDateInterval(dateInterval, excludedNames);

                int[] expectedIds = { 2 };
                teamMembers.Select(x => x.Id).Should().Equal(expectedIds);
            });
    }
}