using DustInTheWind.VeloCity.SystemAccess;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.SystemAccess
{
    public class SystemClockTests
    {
        [Fact]
        public void Today()
        {
            SystemClock systemClock = new();

            DateTime expectedDate = DateTime.Today;
            systemClock.Today.Should().Be(expectedDate);
        }
    }
}