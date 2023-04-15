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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.SprintRepositoryTests;

public class GetLastTests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\SprintRepositoryTests";

    [Fact]
    public async Task HavingDatabaseWithTwoSprints_WhenGetLast_ThenReturnsTheSecondSprint()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-last.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.DbContext);

                Sprint lastSprint = await sprintRepository.GetLast();

                lastSprint.Id.Should().Be(7);
            });
    }

    [Fact]
    public async Task HavingEmptyDatabase_WhenGetLast_ThenReturnsNull()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-last.empty.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.DbContext);

                Sprint lastSprint = await sprintRepository.GetLast();

                lastSprint.Should().BeNull();
            });
    }

    [Fact]
    public async Task GetLastMany()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-last.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.DbContext);

                IEnumerable<Sprint> lastSprints = await sprintRepository.GetLast(3);

                int[] expectedIds = { 6, 5, 4 };
                lastSprints.Select(x => x.Id).Should().Equal(expectedIds);
            });
    }
}