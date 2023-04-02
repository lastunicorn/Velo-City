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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.EmploymentWeekTests;

public class IsWorkDayTests
{
    [Theory]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    public void HavingDefaultEmploymentWeek_WhenVerifyingMondayToFriday_ThenReturnsTrue(DayOfWeek dayOfWeek)
    {
        EmploymentWeek employmentWeek = new();

        bool actual = employmentWeek.IsWorkDay(dayOfWeek);

        actual.Should().BeTrue();
    }

    [Theory]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public void HavingDefaultEmploymentWeek_WhenVerifyingSaturdayAndSunday_ThenReturnsFalse(DayOfWeek dayOfWeek)
    {
        EmploymentWeek employmentWeek = new();

        bool actual = employmentWeek.IsWorkDay(dayOfWeek);

        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData((DayOfWeek)489)]
    [InlineData((DayOfWeek)34645)]
    [InlineData((DayOfWeek)(-3548))]
    public void HavingDefaultEmploymentWeek_WhenVerifyingNonexistentDays_ThenReturnsFalse(DayOfWeek dayOfWeek)
    {
        EmploymentWeek employmentWeek = new();

        bool actual = employmentWeek.IsWorkDay(dayOfWeek);

        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public void HavingEmploymentWeekWithOneDay_WhenVerifyingThatDay_ThenReturnsTrue(DayOfWeek dayOfWeek)
    {
        EmploymentWeek employmentWeek = new(new[] { dayOfWeek });

        bool actual = employmentWeek.IsWorkDay(dayOfWeek);

        actual.Should().BeTrue();
    }

    [Fact]
    public void HavingEmploymentWeekWithOneDay_WhenVerifyingOtherDay_ThenReturnsFalse()
    {
        EmploymentWeek employmentWeek = new(new[] { DayOfWeek.Wednesday });

        bool actual = employmentWeek.IsWorkDay(DayOfWeek.Monday);

        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData((DayOfWeek)489)]
    [InlineData((DayOfWeek)34645)]
    [InlineData((DayOfWeek)(-3548))]
    public void HavingEmploymentWeekWithOneDay_WhenVerifyingNonexistentDays_ThenReturnsFalse(DayOfWeek dayOfWeek)
    {
        EmploymentWeek employmentWeek = new();

        bool actual = employmentWeek.IsWorkDay(dayOfWeek);

        actual.Should().BeFalse();
    }
}