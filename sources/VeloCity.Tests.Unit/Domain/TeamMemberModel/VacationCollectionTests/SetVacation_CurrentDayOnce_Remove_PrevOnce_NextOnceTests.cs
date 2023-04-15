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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.VacationCollectionTests;

// --- --- --- --- --- --- --- --- ---
//             (=) (*) (=)
//             (=)  x  (=)
//             (=)     (=)

public class SetVacation_CurrentDayOnce_Remove_PrevOnce_NextOnceTests
{
    private readonly VacationCollection vacationCollection;
    private readonly DateTime currentDate;
    private readonly DateTime previousDate;
    private readonly DateTime nextDate;
    private readonly SingleDayVacation currentVacation;
    private readonly SingleDayVacation previousVacation;
    private readonly SingleDayVacation nextVacation;

    public SetVacation_CurrentDayOnce_Remove_PrevOnce_NextOnceTests()
    {
        currentDate = new DateTime(2023, 03, 27);
        previousDate = new DateTime(2023, 03, 26);
        nextDate = new DateTime(2023, 03, 28);

        vacationCollection = new VacationCollection();

        currentVacation = new SingleDayVacation
        {
            Date = currentDate,
            HourCount = 8
        };
        vacationCollection.Add(currentVacation);

        previousVacation = new SingleDayVacation
        {
            Date = previousDate,
            HourCount = 4
        };
        vacationCollection.Add(previousVacation);

        nextVacation = new SingleDayVacation
        {
            Date = nextDate,
            HourCount = 8
        };
        vacationCollection.Add(nextVacation);
    }

    [Fact]
    public void HavingPrevNoneNextNone_WhenSettingZeroForCurrentDay_ThenCurrentDayHasNoVacation()
    {
        vacationCollection.SetVacation(currentDate, HoursValue.Zero);

        List<Vacation> actualVacations = vacationCollection.GetVacationsFor(currentDate).ToList();
        actualVacations.Should().BeEmpty();
    }

    [Fact]
    public void HavingPrevNoneNextNone_WhenSettingZeroForCurrentDay_ThenOldCurrentDayVacationDoesNotTriggerChangeEventAnymore()
    {
        vacationCollection.SetVacation(currentDate, HoursValue.Zero);

        bool wasEventTriggered = false;
        vacationCollection.Changed += (sender, args) => wasEventTriggered = true;

        currentVacation.HourCount = 100;

        wasEventTriggered.Should().BeFalse();
    }

    [Fact]
    public void HavingPrevNoneNextNone_WhenSettingZeroForCurrentDay_ThenPreviousDayHasSameVacation()
    {
        vacationCollection.SetVacation(currentDate, HoursValue.Zero);

        Vacation actualVacation = vacationCollection.GetVacationsFor(previousDate).Single();
        actualVacation.Should().BeSameAs(previousVacation);
    }

    [Fact]
    public void HavingPrevNoneNextNone_WhenSettingZeroForCurrentDay_ThenNextDayHasSameVacation()
    {
        vacationCollection.SetVacation(currentDate, HoursValue.Zero);

        Vacation actualVacation = vacationCollection.GetVacationsFor(nextDate).Single();
        actualVacation.Should().BeSameAs(nextVacation);
    }

    [Fact]
    public void HavingPrevNoneNextNone_WhenSettingNegativeForCurrentDay_ThenCurrentDayHasNoVacation()
    {
        vacationCollection.SetVacation(currentDate, -123);

        List<Vacation> actualVacations = vacationCollection.GetVacationsFor(currentDate).ToList();
        actualVacations.Should().BeEmpty();
    }
}