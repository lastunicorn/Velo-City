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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.SprintRepositoryTests;

public class GetDateIntervalForTests : DatabaseTestsBase
{
    private const string DatabaseFilePath = @"TestData\DataAccess\SprintRepositoryTests\db-get-date-interval-for.json";

    private readonly SprintRepository sprintRepository;

    public GetDateIntervalForTests()
        : base(DatabaseFilePath)
    {
        OpenDatabase();
        sprintRepository = new SprintRepository(VeloCityDbContext);
    }

    [Fact]
    public async Task GetDateIntervalForExistingSprint()
    {
        DateInterval? dateInterval = await sprintRepository.GetDateIntervalFor(5);

        DateInterval expected = new(new DateTime(2022, 04, 18), new DateTime(2022, 05, 01));
        dateInterval.Should().Be(expected);
    }

    [Fact]
    public async Task GetDateIntervalForNonExistingSprint()
    {
        DateInterval? dateInterval = await sprintRepository.GetDateIntervalFor(50);

        dateInterval.Should().BeNull();
    }
}