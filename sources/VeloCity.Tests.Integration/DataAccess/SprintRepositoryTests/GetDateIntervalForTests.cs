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