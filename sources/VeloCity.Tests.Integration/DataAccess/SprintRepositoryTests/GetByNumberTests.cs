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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.SprintRepositoryTests;

public class GetByNumberTests : DatabaseTestsBase
{
    private const string DatabaseFilePath = @"TestData\DataAccess\SprintRepositoryTests\db-get-by-number.json";

    private readonly SprintRepository sprintRepository;

    public GetByNumberTests()
        : base(DatabaseFilePath)
    {
        OpenDatabase();
        sprintRepository = new SprintRepository(VeloCityDbContext);
    }

    [Fact]
    public async Task GetByNumber()
    {
        Sprint sprint = await sprintRepository.GetByNumber(5);

        sprint.Number.Should().Be(5);
    }

    [Fact]
    public async Task GetByNonExistentNumber()
    {
        Sprint sprint = await sprintRepository.GetByNumber(39247869);

        sprint.Should().BeNull();
    }
}