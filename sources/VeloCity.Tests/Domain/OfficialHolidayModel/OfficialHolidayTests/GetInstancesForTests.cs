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

using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;

namespace DustInTheWind.VeloCity.Tests.Domain.OfficialHolidayModel.OfficialHolidayTests
{
    public class GetInstancesForTests
    {
        [Fact]
        public void HavingAnOfficialHoliday_WhenEnumeratingInstancesForOneYear_ThenGetInstanceForIsCalledForThatYear()
        {
            Mock<OfficialHoliday> officialHoliday = new();
            officialHoliday
                .Setup(x => x.GetInstanceFor(It.IsAny<int>()))
                .Returns(new OfficialHolidayInstance());

            DateTime startDate = new(2000, 10, 01);
            DateTime endDate = new(2000, 10, 31);
            officialHoliday.Object.GetInstancesFor(startDate, endDate).ToList();

            officialHoliday.Verify(x => x.GetInstanceFor(2000), Times.Once);
        }

        [Fact]
        public void HavingAnOfficialHoliday_WhenEnumeratingInstancesForTwoYears_ThenGetInstanceForIsCalledForEachYear()
        {
            Mock<OfficialHoliday> officialHoliday = new();
            officialHoliday
                .Setup(x => x.GetInstanceFor(It.IsAny<int>()))
                .Returns(new OfficialHolidayInstance());

            DateTime startDate = new(2001, 02, 01);
            DateTime endDate = new(2002, 08, 31);
            officialHoliday.Object.GetInstancesFor(startDate, endDate).ToList();

            officialHoliday.Verify(x => x.GetInstanceFor(2001), Times.Once);
            officialHoliday.Verify(x => x.GetInstanceFor(2002), Times.Once);
        }

        [Fact]
        public void HavingAnOfficialHoliday_WhenEnumeratingInstancesForThreeYears_ThenGetInstanceForIsCalledForEachYear()
        {
            Mock<OfficialHoliday> officialHoliday = new();
            officialHoliday
                .Setup(x => x.GetInstanceFor(It.IsAny<int>()))
                .Returns(new OfficialHolidayInstance());

            DateTime startDate = new(2001, 02, 01);
            DateTime endDate = new(2003, 08, 31);
            officialHoliday.Object.GetInstancesFor(startDate, endDate).ToList();

            officialHoliday.Verify(x => x.GetInstanceFor(2001), Times.Once);
            officialHoliday.Verify(x => x.GetInstanceFor(2002), Times.Once);
            officialHoliday.Verify(x => x.GetInstanceFor(2003), Times.Once);
        }

        [Fact]
        public void HavingAnOfficialHolidayForSpecificDate_WhenEnumeratingInstancesForIntervalInSameYearNotContainingDate_ThenReturnsEmptyCollection()
        {
            Mock<OfficialHoliday> officialHoliday = new();
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
            Mock<OfficialHoliday> officialHoliday = new();
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
}