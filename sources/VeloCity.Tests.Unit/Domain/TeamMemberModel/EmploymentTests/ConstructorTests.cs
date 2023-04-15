// VeloCity
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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.EmploymentTests;

public class ConstructorTests
{
    [Fact]
    public void WhenCreatingANewInstance_ThenStartDateIsNull()
    {
        Employment employment = new();

        employment.StartDate.Should().BeNull();
    }

    [Fact]
    public void WhenCreatingANewInstance_ThenEndDateIsNull()
    {
        Employment employment = new();

        employment.EndDate.Should().BeNull();
    }

    [Fact]
    public void WhenCreatingANewInstance_ThenTimeIntervalIsFullInfinite()
    {
        Employment employment = new();

        employment.TimeInterval.IsFullInfinite.Should().BeTrue();
    }

    [Fact]
    public void WhenCreatingANewInstance_ThenHoursPerDayIsZero()
    {
        Employment employment = new();

        employment.HoursPerDay.Should().Be(HoursValue.Zero);
    }

    [Fact]
    public void WhenCreatingANewInstance_ThenEmploymentWeekIsNull()
    {
        Employment employment = new();

        employment.EmploymentWeek.Should().BeNull();
    }

    [Fact]
    public void WhenCreatingANewInstance_ThenCountryIsNull()
    {
        Employment employment = new();

        employment.Country.Should().BeNull();
    }
}