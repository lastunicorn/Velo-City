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

public class AddTests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\SprintRepositoryTests";

    [Fact]
    public async Task WhenAddingSprintWithoutId_ThenSprintIdIs1AndSprintExistsInDatabase()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-add.empty.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.VeloCityDbContext);

                Sprint sprint = new()
                {
                    Number = 5
                };
                sprintRepository.Add(sprint);

                await context.VeloCityDbContext.SaveChanges();

                sprint.Id.Should().NotBe(0);
                await context.Asserts.ExistsSprint(sprint.Id);
            });
    }

    [Fact]
    public void WhenAddingNullSprint_ThenThrows()
    {
        DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-add.json")
            .Execute(context =>
            {
                SprintRepository sprintRepository = new(context.VeloCityDbContext);

                Action action = () =>
                {
                    sprintRepository.Add(null);
                };

                action.Should().Throw<ArgumentNullException>();
            });
    }

    [Fact]
    public async Task WhenAddingSprintWithIdAndSave_ThenSprintExistsInDatabase()
    {
        await DatabaseTestContext
             .WithDatabase(DatabaseDirectoryPath, "db-add.json")
             .Execute(async context =>
             {
                 SprintRepository sprintRepository = new(context.VeloCityDbContext);

                 Sprint sprint = new()
                 {
                     Id = 5,
                     Number = 5
                 };
                 sprintRepository.Add(sprint);

                 await context.VeloCityDbContext.SaveChanges();

                 await context.Asserts.ExistsSprint(5);
             });
    }

    [Fact]
    public async Task WhenAddingSprintWithoutIdAndSave_ThenSprintIdIsAddedAndSprintExistsInDatabase()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-add.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.VeloCityDbContext);

                Sprint sprint = new()
                {
                    Number = 5
                };
                sprintRepository.Add(sprint);

                await context.VeloCityDbContext.SaveChanges();

                sprint.Id.Should().NotBe(0);
                await context.Asserts.ExistsSprint(sprint.Id);
            });
    }

    [Fact]
    public async Task WhenAddingSprintWithIdAndNotSave_ThenSprintDoesNotExistInDatabase()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-add.json")
            .Execute(async context =>
            {
                SprintRepository sprintRepository = new(context.VeloCityDbContext);

                Sprint sprint = new()
                {
                    Id = 5,
                    Number = 5
                };
                sprintRepository.Add(sprint);

                await context.Asserts.NotExistsSprint(5);
            });
    }
}