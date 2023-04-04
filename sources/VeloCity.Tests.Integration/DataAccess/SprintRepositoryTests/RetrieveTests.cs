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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.SprintRepositoryTests;

public class RetrieveTests : DatabaseTestsBase
{
    private const string DatabaseFilePath = @"TestData\DataAccess\SprintRepositoryTests\db-retrieve.json";

    private readonly SprintRepository sprintRepository;

    public RetrieveTests()
    : base(DatabaseFilePath)
    {
        OpenDatabase();
        sprintRepository = new SprintRepository(VeloCityDbContext);
    }

    [Fact]
    public async Task GetAll()
    {
        IEnumerable<Sprint> sprints = await sprintRepository.GetAll();

        sprints.Should().HaveCount(13);
    }

    [Fact]
    public async Task Get()
    {
        Sprint sprint = await sprintRepository.Get(3);

        sprint.Id.Should().Be(3);
    }

    [Fact]
    public async Task GetByNonExistentId()
    {
        Sprint sprint = await sprintRepository.Get(234235);

        sprint.Should().BeNull();
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

    [Fact]
    public async Task IsAnyInProgress()
    {
        bool isAnyInProgress = await sprintRepository.IsAnyInProgress();

        isAnyInProgress.Should().BeTrue();
    }

    [Fact]
    public async Task IsFirstNewSprint7_ShouldReturnTrue()
    {
        bool isFirstNewSprint = await sprintRepository.IsFirstNewSprint(7);

        isFirstNewSprint.Should().BeTrue();
    }

    [Fact]
    public async Task IsFirstNewSprint8_ShouldReturnFalse()
    {
        bool isFirstNewSprint = await sprintRepository.IsFirstNewSprint(8);

        isFirstNewSprint.Should().BeFalse();
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

    [Fact]
    public async Task GetLast()
    {
        Sprint lastSprint = await sprintRepository.GetLast();

        lastSprint.Id.Should().Be(13);
    }

    [Fact]
    public async Task GetLastMany()
    {
        IEnumerable<Sprint> lastSprints = await sprintRepository.GetLast(3);

        int[] expectedIds = { 6, 5, 4 };
        lastSprints.Select(x => x.Id).Should().Equal(expectedIds);
    }

    [Fact]
    public async Task GetLastInProgress()
    {
        Sprint lastSprint = await sprintRepository.GetLastInProgress();

        lastSprint.Id.Should().Be(6);
    }

    [Fact]
    public async Task GetLastClosed()
    {
        Sprint lastSprint = await sprintRepository.GetLastClosed();

        lastSprint.Id.Should().Be(5);
    }

    [Fact]
    public async Task GetLastClosedMany()
    {
        IEnumerable<Sprint> lastSprints = await sprintRepository.GetLastClosed(7);

        int[] expectedIds = { 5, 4, 3, 2, 1 };
        lastSprints.Select(x => x.Id).Should().Equal(expectedIds);
    }

    [Fact]
    public async Task GetBetweenDates()
    {
        IEnumerable<Sprint> sprints = await sprintRepository.Get(new DateTime(2022, 04, 18), new DateTime(2022, 05, 29));

        int[] expectedIds = { 7, 6, 5 };
        sprints.Select(x => x.Id).Should().Equal(expectedIds);
    }
}