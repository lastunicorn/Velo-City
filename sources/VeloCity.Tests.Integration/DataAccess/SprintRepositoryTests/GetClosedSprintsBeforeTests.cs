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