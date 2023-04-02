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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.EmploymentTests;

public class IsWorkDayTests
{
    [Theory]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public void HavingEmploymentWithNoEmploymentWeek_WhenCheckingAnyDayIfTheyAreWorkDays_ThenReturnsFalse(DayOfWeek dayOfWeek)
    {
        Employment employment = new();

        bool actual = employment.IsWorkDay(dayOfWeek);

        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData((DayOfWeek)100)]
    [InlineData((DayOfWeek)34645)]
    [InlineData((DayOfWeek)(-613))]
    public void HavingEmploymentWithNoEmploymentWeek_WhenNonexistentDaysIfTheyAreWorkDays_ThenReturnsFalse(DayOfWeek dayOfWeek)
    {
        Employment employment = new();

        bool actual = employment.IsWorkDay(dayOfWeek);

        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    public void HavingEmploymentWithDefaultWeek_WhenCheckingMondayToFridayIfTheyAreWorkDays_ThenReturnsTrue(DayOfWeek dayOfWeek)
    {
        Employment employment = new()
        {
            EmploymentWeek = new EmploymentWeek()
        };

        bool actual = employment.IsWorkDay(dayOfWeek);

        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public void HavingEmploymentWithDefaultWeek_WhenCheckingWeekendDaysIfTheyAreWorkDays_ThenReturnsFalse(DayOfWeek dayOfWeek)
    {
        Employment employment = new()
        {
            EmploymentWeek = new EmploymentWeek()
        };

        bool actual = employment.IsWorkDay(dayOfWeek);

        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData((DayOfWeek)100)]
    [InlineData((DayOfWeek)34645)]
    [InlineData((DayOfWeek)(-613))]
    public void HavingEmploymentWithDefaultWeek_WhenNonexistentDaysIfTheyAreWorkDays_ThenReturnsFalse(DayOfWeek dayOfWeek)
    {
        Employment employment = new()
        {
            EmploymentWeek = new EmploymentWeek()
        };

        bool actual = employment.IsWorkDay(dayOfWeek);

        actual.Should().BeFalse();
    }
}