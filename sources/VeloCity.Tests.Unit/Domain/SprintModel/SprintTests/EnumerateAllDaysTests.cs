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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.SprintModel.SprintTests;

public class EnumerateAllDaysTests
{
    private readonly Sprint sprint;

    public EnumerateAllDaysTests()
    {
        DateTime startDate = new(2001, 05, 03);
        DateTime endDate = new(2001, 05, 05);
        sprint = new Sprint
        {
            DateInterval = new DateInterval(startDate, endDate)
        };
    }

    [Fact]
    public void HavingASprintOf3Days_WhenEnumeratingAllDays_Then3SprintDaysAreReturned()
    {
        // act
        List<SprintDay> sprintDays = sprint.EnumerateAllDays().ToList();

        // assert
        sprintDays.Count.Should().Be(3);
    }

    [Fact]
    public void HavingASprintOf3Days_WhenEnumeratingAllDays_ThenTheSprintDaysHaveCorrectDateTimes()
    {
        // act
        List<SprintDay> sprintDays = sprint.EnumerateAllDays().ToList();

        // assert
        sprintDays[0].Date.Should().Be(new DateTime(2001, 05, 03));
        sprintDays[1].Date.Should().Be(new DateTime(2001, 05, 04));
        sprintDays[2].Date.Should().Be(new DateTime(2001, 05, 05));
    }
}