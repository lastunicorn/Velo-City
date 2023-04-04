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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.JsonFiles;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.SprintRepositoryTests;

public class AddTests : DatabaseTestsBase
{
    private const string DatabaseFilePath = @"TestData\DataAccess\SprintRepositoryTests\db-update.json";

    private readonly SprintRepository sprintRepository;

    public AddTests()
        : base(DatabaseFilePath)
    {
        OpenDatabase();
        sprintRepository = new SprintRepository(VeloCityDbContext);
    }

    [Fact]
    public async Task WhenAddingNullSprint_ThenThrows()
    {
        Sprint sprint = new()
        {
            Id = 5,
            Number = 5
        };
        sprintRepository.Add(sprint);

        await VeloCityDbContext.SaveChanges();

        await AssertExistsSprint(5);
    }

    [Fact]
    public async Task WhenAddingSprintWithIdAndSave_ThenSprintExistsInDatabase()
    {
        Sprint sprint = new()
        {
            Id = 5,
            Number = 5
        };
        sprintRepository.Add(sprint);

        await VeloCityDbContext.SaveChanges();

        await AssertExistsSprint(5);
    }

    [Fact]
    public async Task WhenAddingSprintWithoutIdAndSave_ThenSprintIdIsAddedAndSprintExistsInDatabase()
    {
        Sprint sprint = new()
        {
            Number = 5
        };
        sprintRepository.Add(sprint);

        await VeloCityDbContext.SaveChanges();

        sprint.Id.Should().NotBe(0);
        await AssertExistsSprint(sprint.Id);
    }

    [Fact]
    public async Task WhenAddingSprintWithIdAndNotSave_ThenSprintDoesNotExistInDatabase()
    {
        Sprint sprint = new()
        {
            Id = 5,
            Number = 5
        };
        sprintRepository.Add(sprint);

        await AssertNotExistsSprint(5);
    }

    private static async Task AssertExistsSprint(int id)
    {
        JsonDatabase jsonDatabase = new()
        {
            PersistenceLocation = DatabaseFilePath
        };
        jsonDatabase.Open();
        VeloCityDbContext veloCityDbContext = new(jsonDatabase);
        SprintRepository sprintRepository = new(veloCityDbContext);
        Sprint sprint = await sprintRepository.Get(id);

        sprint.Should().NotBeNull();
    }

    private static async Task AssertNotExistsSprint(int id)
    {
        JsonDatabase jsonDatabase = new()
        {
            PersistenceLocation = DatabaseFilePath
        };
        jsonDatabase.Open();
        VeloCityDbContext veloCityDbContext = new(jsonDatabase);
        SprintRepository sprintRepository = new(veloCityDbContext);
        Sprint sprint = await sprintRepository.Get(id);

        sprint.Should().BeNull();
    }
}