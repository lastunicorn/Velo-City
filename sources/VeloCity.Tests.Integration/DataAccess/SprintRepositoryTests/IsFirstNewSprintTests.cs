using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.SprintRepositoryTests;

public class IsFirstNewSprintTests : DatabaseTestsBase
{
    private const string DatabaseFilePath = @"TestData\DataAccess\SprintRepositoryTests\db-is-first-new-sprint.json";

    private readonly SprintRepository sprintRepository;

    public IsFirstNewSprintTests()
        : base(DatabaseFilePath)
    {
        OpenDatabase();
        sprintRepository = new SprintRepository(VeloCityDbContext);
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
}