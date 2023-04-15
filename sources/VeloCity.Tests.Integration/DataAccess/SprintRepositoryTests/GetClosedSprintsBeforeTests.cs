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

public class GetClosedSprintsBeforeTests : DatabaseTestsBase
{
    private const string DatabaseFilePath = @"TestData\DataAccess\SprintRepositoryTests\db-get-closed-sprints-before.json";

    private readonly SprintRepository sprintRepository;

    public GetClosedSprintsBeforeTests()
        : base(DatabaseFilePath)
    {
        OpenDatabase();
        sprintRepository = new SprintRepository(VeloCityDbContext);
    }

    [Fact]
    public async Task GetClosedSprintsBefore()
    {
        IEnumerable<Sprint> closedSprints = await sprintRepository.GetClosedSprintsBefore(9, 3);

        int[] expectedIds = { 5, 4, 3 };
        closedSprints.Select(x => x.Id).Should().Equal(expectedIds);
    }

    [Fact]
    public async Task GetClosedSprintsBeforeWithExcluding()
    {
        IEnumerable<Sprint> closedSprints = await sprintRepository.GetClosedSprintsBefore(9, 3, new[] { 4 });

        int[] expectedIds = { 5, 3, 2 };
        closedSprints.Select(x => x.Id).Should().Equal(expectedIds);
    }
}