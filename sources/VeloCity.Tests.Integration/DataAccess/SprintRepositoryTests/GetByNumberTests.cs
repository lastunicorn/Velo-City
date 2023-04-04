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