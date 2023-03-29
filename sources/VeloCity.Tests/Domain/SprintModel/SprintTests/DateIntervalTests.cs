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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Tests.Domain.SprintModel.SprintTests;

public class DateIntervalTests
{
    [Fact]
    public void WhenCreatingASprintInstance_ThenStartDateIsToday()
    {
        // act
        Sprint sprint = new();

        // assert
        sprint.StartDate.Should().Be(DateTime.Today);
    }

    [Fact]
    public void WhenCreatingASprintInstance_ThenEndDateIsToday()
    {
        // act
        Sprint sprint = new();

        // assert
        sprint.EndDate.Should().Be(DateTime.Today);
    }

    [Fact]
    public void HavingANewSprintInstance_WhenDateIntervalIsSet_ThenStartDateIsTheStartDateOdTheDateInterval()
    {
        // arrange
        Sprint sprint = new();
        DateTime startDate = new(2000, 02, 04);
        DateTime endDate = new(2000, 02, 24);
        DateInterval dateInterval = new(startDate, endDate);

        // act
        sprint.DateInterval = dateInterval;

        // assert
        sprint.StartDate.Should().Be(startDate);
    }

    [Fact]
    public void HavingANewSprintInstance_WhenDateIntervalIsSet_ThenEndDateIsTheEndDateOdTheDateInterval()
    {
        // arrange
        Sprint sprint = new();
        DateTime startDate = new(2000, 02, 04);
        DateTime endDate = new(2000, 02, 24);
        DateInterval dateInterval = new(startDate, endDate);

        // act
        sprint.DateInterval = dateInterval;

        // assert
        sprint.EndDate.Should().Be(endDate);
    }

    [Fact]
    public void HavingANewSprintInstance_WhenNegativeInfiniteDateIntervalIsSet_ThenThrows()
    {
        // arrange
        Sprint sprint = new();
        DateTime endDate = new(2000, 02, 24);
        DateInterval dateInterval = new(null, endDate);

        // assert
        Assert.Throws<ArgumentException>(() =>
        {
            // act
            sprint.DateInterval = dateInterval;
        });
    }

    [Fact]
    public void HavingANewSprintInstance_WhenPositiveInfiniteDateIntervalIsSet_ThenThrows()
    {
        // arrange
        Sprint sprint = new();
        DateTime startDate = new(2000, 02, 04);
        DateInterval dateInterval = new(startDate, null);

        // assert
        Assert.Throws<ArgumentException>(() =>
        {
            // act
            sprint.DateInterval = dateInterval;
        });
    }
}