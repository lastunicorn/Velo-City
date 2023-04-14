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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.DailyVacationTests;

public class DateIntervalTests
{
    private readonly DailyVacation dailyVacation;

    public DateIntervalTests()
    {
        dailyVacation = new DailyVacation();
    }

    [Fact]
    public void HavingVacationInstance_WhenSettingIntervalAsFullInfinite_ThenStartDateIsNull()
    {
        dailyVacation.DateInterval = DateInterval.FullInfinite;

        dailyVacation.StartDate.Should().BeNull();
    }

    [Fact]
    public void HavingVacationInstance_WhenSettingIntervalAsFullInfinite_ThenEndDateIsNull()
    {
        dailyVacation.DateInterval = DateInterval.FullInfinite;

        dailyVacation.EndDate.Should().BeNull();
    }

    [Fact]
    public void HavingVacationInstance_WhenSettingIntervalAsStartInfinite_ThenStartDateIsNull()
    {
        dailyVacation.DateInterval = new DateInterval(null, new DateTime(2023, 04, 14));

        dailyVacation.StartDate.Should().BeNull();
    }

    [Fact]
    public void HavingVacationInstance_WhenSettingIntervalAsStartInfinite_ThenEndDateIsTheEndDateFromInterval()
    {
        dailyVacation.DateInterval = new DateInterval(null, new DateTime(2023, 04, 14));

        dailyVacation.EndDate.Should().Be(new DateTime(2023, 04, 14));
    }

    [Fact]
    public void HavingVacationInstance_WhenSettingIntervalAsEndInfinite_ThenStartDateIsTheStartDateFromInterval()
    {
        dailyVacation.DateInterval = new DateInterval(new DateTime(2023, 04, 14));

        dailyVacation.StartDate.Should().Be(new DateTime(2023, 04, 14));
    }

    [Fact]
    public void HavingVacationInstance_WhenSettingIntervalAsEndInfinite_ThenEndDateIsNull()
    {
        dailyVacation.DateInterval = new DateInterval(new DateTime(2023, 04, 14));

        dailyVacation.EndDate.Should().BeNull();
    }

    [Fact]
    public void HavingVacationInstance_WhenSettingIntervalAsFinite_ThenStartDateIsTheStartDateFromInterval()
    {
        dailyVacation.DateInterval = new DateInterval(new DateTime(2023, 04, 14), new DateTime(2023, 04, 20));

        dailyVacation.StartDate.Should().Be(new DateTime(2023, 04, 14));
    }

    [Fact]
    public void HavingVacationInstance_WhenSettingIntervalAsFinite_ThenEndDateIsTheEndDateFromInterval()
    {
        dailyVacation.DateInterval = new DateInterval(new DateTime(2023, 04, 14), new DateTime(2023, 04, 20));

        dailyVacation.EndDate.Should().Be(new DateTime(2023, 04, 20));
    }
}