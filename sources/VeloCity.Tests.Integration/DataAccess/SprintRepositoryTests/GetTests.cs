using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.SprintRepositoryTests;

public class GetTests : DatabaseTestsBase
{
    private const string DatabaseFilePath = @"TestData\DataAccess\SprintRepositoryTests\db-get.json";

    private readonly SprintRepository sprintRepository;

    public GetTests()
        : base(DatabaseFilePath)
    {
        OpenDatabase();
        sprintRepository = new SprintRepository(VeloCityDbContext);
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
}