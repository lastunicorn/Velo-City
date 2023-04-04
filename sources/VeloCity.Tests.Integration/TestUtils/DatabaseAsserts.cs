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
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.TestUtils;

public class DatabaseAsserts
{
    private readonly string databaseFilePath;

    public DatabaseAsserts(string databaseFilePath)
    {
        this.databaseFilePath = databaseFilePath ?? throw new ArgumentNullException(nameof(databaseFilePath));
    }

    public async Task AssertExistsSprint(int id)
    {
        JsonDatabase jsonDatabase = new()
        {
            PersistenceLocation = databaseFilePath
        };
        jsonDatabase.Open();
        VeloCityDbContext veloCityDbContext = new(jsonDatabase);
        SprintRepository sprintRepository = new(veloCityDbContext);
        Sprint sprint = await sprintRepository.Get(id);

        sprint.Should().NotBeNull();
    }

    public async Task AssertNotExistsSprint(int id)
    {
        JsonDatabase jsonDatabase = new()
        {
            PersistenceLocation = databaseFilePath
        };
        jsonDatabase.Open();
        VeloCityDbContext veloCityDbContext = new(jsonDatabase);
        SprintRepository sprintRepository = new(veloCityDbContext);
        Sprint sprint = await sprintRepository.Get(id);

        sprint.Should().BeNull();
    }
}