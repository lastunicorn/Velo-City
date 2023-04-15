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

public class GetLastInProgressTests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\SprintRepositoryTests";

    [Fact]
    public async Task HavingDatabaseWithMultipleSprintsAndLastOneInProgress_WhenCallGetLastInProgress_ThenReturnsTheLastOne()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-last-in-progress.last.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.VeloCityDbContext);
                Sprint sprint = await sprintRepository.GetLastInProgress();

                sprint.Id.Should().Be(3);
            });
    }

    [Fact]
    public async Task HavingDatabaseWithSprintsAfterTheOneInProgress_WhenCallGetLastInProgress_ThenReturnsTheOneInProgress()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-last-in-progress.not-last.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.VeloCityDbContext);
                Sprint sprint = await sprintRepository.GetLastInProgress();

                sprint.Id.Should().Be(3);
            });
    }

    [Fact]
    public async Task HavingDatabaseWithMultipleSprintsButNoneInProgress_WhenCallGetLastInProgress_ThenReturnsNull()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-last-in-progress.none.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.VeloCityDbContext);
                Sprint sprint = await sprintRepository.GetLastInProgress();

                sprint.Should().BeNull();
            });
    }

    [Fact]
    public async Task HavingEmptyDatabase_WhenCallGetLastInProgress_ThenReturnsNull()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-last-in-progress.empty.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.VeloCityDbContext);
                Sprint sprint = await sprintRepository.GetLastInProgress();

                sprint.Should().BeNull();
            });
    }
}