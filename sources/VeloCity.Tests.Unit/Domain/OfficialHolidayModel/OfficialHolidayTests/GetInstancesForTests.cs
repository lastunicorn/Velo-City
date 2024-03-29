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

using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.OfficialHolidayModel.OfficialHolidayTests;

public class GetInstancesForTests
{
    private readonly Mock<OfficialHoliday> officialHoliday;

    public GetInstancesForTests()
    {
        officialHoliday = new Mock<OfficialHoliday>();
    }

    [Fact]
    public void HavingAnOfficialHoliday_WhenEnumeratingInstancesForOneYear_ThenGetInstanceForIsCalledForThatYear()
    {
        officialHoliday
            .Setup(x => x.GetInstanceFor(It.IsAny<int>()))
            .Returns(new OfficialHolidayInstance());

        DateTime startDate = new(2000, 10, 01);
        DateTime endDate = new(2000, 10, 31);
        _ = officialHoliday.Object.GetInstancesFor(startDate, endDate).ToList();

        officialHoliday.Verify(x => x.GetInstanceFor(2000), Times.Once);
    }

    [Fact]
    public void HavingAnOfficialHoliday_WhenEnumeratingInstancesForTwoYears_ThenGetInstanceForIsCalledForEachYear()
    {
        officialHoliday
            .Setup(x => x.GetInstanceFor(It.IsAny<int>()))
            .Returns(new OfficialHolidayInstance());

        DateTime startDate = new(2001, 02, 01);
        DateTime endDate = new(2002, 08, 31);
        _ = officialHoliday.Object.GetInstancesFor(startDate, endDate).ToList();

        officialHoliday.Verify(x => x.GetInstanceFor(2001), Times.Once);
        officialHoliday.Verify(x => x.GetInstanceFor(2002), Times.Once);
    }

    [Fact]
    public void HavingAnOfficialHoliday_WhenEnumeratingInstancesForThreeYears_ThenGetInstanceForIsCalledForEachYear()
    {
        officialHoliday
            .Setup(x => x.GetInstanceFor(It.IsAny<int>()))
            .Returns(new OfficialHolidayInstance());

        DateTime startDate = new(2001, 02, 01);
        DateTime endDate = new(2003, 08, 31);
        _ = officialHoliday.Object.GetInstancesFor(startDate, endDate).ToList();

        officialHoliday.Verify(x => x.GetInstanceFor(2001), Times.Once);
        officialHoliday.Verify(x => x.GetInstanceFor(2002), Times.Once);
        officialHoliday.Verify(x => x.GetInstanceFor(2003), Times.Once);
    }

    [Fact]
    public void HavingAnOfficialHolidayForSpecificDate_WhenEnumeratingInstancesForIntervalInSameYearNotContainingDate_ThenReturnsEmptyCollection()
    {
        officialHoliday
            .Setup(x => x.GetInstanceFor(It.IsAny<int>()))
            .Returns(new OfficialHolidayInstance());
        officialHoliday.Object.Date = new DateTime(100, 06, 09);

        DateTime startDate = new(2000, 07, 01);
        DateTime endDate = new(2000, 10, 31);
        List<OfficialHolidayInstance> actual = officialHoliday.Object.GetInstancesFor(startDate, endDate)
            .ToList();

        actual.Should().HaveCount(0);
    }

    [Fact]
    public void HavingAnOfficialHolidayForSpecificDate_WhenEnumeratingInstancesForIntervalInSameYearContainingDate_ThenReturnsInstanceForSpecifiedYear()
    {
        officialHoliday
            .Setup(x => x.GetInstanceFor(2000))
            .Returns(new OfficialHolidayInstance
            {
                Date = new DateTime(2000, 06, 09)
            });
        officialHoliday.Object.Date = new DateTime(100, 06, 09);

        DateTime startDate = new(2000, 05, 01);
        DateTime endDate = new(2000, 10, 31);
        List<OfficialHolidayInstance> actual = officialHoliday.Object.GetInstancesFor(startDate, endDate)
            .ToList();

        actual.Should().HaveCount(1);
        actual[0].Date.Should().Be(new DateTime(2000, 06, 09));
    }
}