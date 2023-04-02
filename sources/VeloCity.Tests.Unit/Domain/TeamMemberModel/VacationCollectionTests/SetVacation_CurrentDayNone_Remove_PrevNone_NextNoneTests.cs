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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.VacationCollectionTests;

public class SetVacation_CurrentDayNone_Remove_PrevNone_NextNoneTests
{
    private readonly VacationCollection vacationCollection;
    private readonly DateTime currentDate;
    private readonly DateTime previousDate;
    private readonly DateTime nextDate;

    public SetVacation_CurrentDayNone_Remove_PrevNone_NextNoneTests()
    {
        currentDate = new DateTime(2023, 03, 27);
        previousDate = new DateTime(2023, 03, 26);
        nextDate = new DateTime(2023, 03, 28);

        vacationCollection = new VacationCollection();
    }

    [Fact]
    public void WhenSettingZeroForCurrentDay_ThenCurrentDayHasNoVacation()
    {
        vacationCollection.SetVacation(currentDate, HoursValue.Zero);

        List<Vacation> actualVacations = vacationCollection.GetVacationsFor(currentDate).ToList();
        actualVacations.Should().BeEmpty();
    }

    [Fact]
    public void WhenSettingZeroForCurrentDay_ThenPreviousDayHasNoVacation()
    {
        vacationCollection.SetVacation(currentDate, HoursValue.Zero);

        List<Vacation> actualVacations = vacationCollection.GetVacationsFor(previousDate).ToList();
        actualVacations.Should().BeEmpty();
    }

    [Fact]
    public void WhenSettingZeroForCurrentDay_ThenNextDayHasNoVacation()
    {
        vacationCollection.SetVacation(currentDate, HoursValue.Zero);

        List<Vacation> actualVacations = vacationCollection.GetVacationsFor(nextDate).ToList();
        actualVacations.Should().BeEmpty();
    }

    [Fact]
    public void WhenSettingNegativeForCurrentDay_ThenCurrentDayHasNoVacation()
    {
        vacationCollection.SetVacation(currentDate, -123);

        List<Vacation> actualVacations = vacationCollection.GetVacationsFor(currentDate).ToList();
        actualVacations.Should().BeEmpty();
    }
}