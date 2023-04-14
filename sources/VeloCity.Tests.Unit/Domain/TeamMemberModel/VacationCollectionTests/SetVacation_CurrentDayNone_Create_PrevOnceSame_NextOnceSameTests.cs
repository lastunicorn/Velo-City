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

// --- --- --- --- --- --- --- --- ---
//             (=)     (=)
//             (=) (=) (=)
//             [=========]

public class SetVacation_CurrentDayNone_Create_PrevOnceSame_NextOnceSameTests
{
    private readonly VacationCollection vacationCollection;
    private readonly DateTime currentDate;
    private readonly DateTime previousDate;
    private readonly DateTime nextDate;
    private readonly SingleDayVacation previousVacation;
    private readonly SingleDayVacation nextVacation;

    public SetVacation_CurrentDayNone_Create_PrevOnceSame_NextOnceSameTests()
    {
        currentDate = new DateTime(2023, 03, 27);
        previousDate = new DateTime(2023, 03, 26);
        nextDate = new DateTime(2023, 03, 28);

        vacationCollection = new VacationCollection();

        previousVacation = new SingleDayVacation
        {
            Date = previousDate,
            HourCount = 8
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
    public void HavingPrevOnceSameNextOnceSame_WhenSettingVacationForCurrentDay_ThenCurrentDayVacationSpansFromPrevToNext()
    {
        vacationCollection.SetVacation(currentDate, 8);

        DailyVacation actualVacation = vacationCollection.GetVacationsFor(currentDate).Single() as DailyVacation;

        DateInterval expectedDateInterval = new(previousDate, nextDate);
        actualVacation.DateInterval.Should().Be(expectedDateInterval);
    }

    [Fact]
    public void HavingPrevOnceSameNextOnceSame_WhenSettingVacationForCurrentDay_ThenCurrentDayVacationHasCorrectHours()
    {
        vacationCollection.SetVacation(currentDate, 8);

        Vacation actualVacation = vacationCollection.GetVacationsFor(currentDate).Single();

        actualVacation.HourCount.Should().Be(8);
    }

    [Fact]
    public void HavingPrevOnceSameNextOnceSame_WhenSettingVacationForCurrentDay_ThenOldPreviousDayVacationIsRemoved()
    {
        vacationCollection.SetVacation(currentDate, 8);

        List<Vacation> actualVacations = vacationCollection.GetVacationsFor(previousDate).ToList();

        actualVacations.Count.Should().Be(1);
        actualVacations[0].Should().BeOfType<DailyVacation>();
    }

    [Fact]
    public void HavingPrevOnceSameNextOnceSame_WhenSettingVacationForCurrentDay_ThenPreviousVacationDoesNotTriggerChangedEventAnymore()
    {
        vacationCollection.SetVacation(currentDate, 8);

        bool wasEventTriggered = false;
        vacationCollection.Changed += (sender, args) => wasEventTriggered = true;

        previousVacation.HourCount = 100;

        wasEventTriggered.Should().BeFalse();
    }

    [Fact]
    public void HavingPrevOnceSameNextOnceSame_WhenSettingVacationForCurrentDay_ThenNextVacationDoesNotTriggerChangedEventAnymore()
    {
        vacationCollection.SetVacation(currentDate, 8);

        bool wasEventTriggered = false;
        vacationCollection.Changed += (sender, args) => wasEventTriggered = true;

        nextVacation.HourCount = 100;

        wasEventTriggered.Should().BeFalse();
    }

    [Fact]
    public void HavingPrevOnceSameNextOnceSame_WhenSettingVacationForCurrentDay_ThenCurrentVacationTriggersChangedEvent()
    {
        vacationCollection.SetVacation(currentDate, 8);

        bool wasEventTriggered = false;
        vacationCollection.Changed += (sender, args) => wasEventTriggered = true;

        Vacation currentVacation = vacationCollection.GetVacationsFor(previousDate).Single();
        currentVacation.HourCount = 100;

        wasEventTriggered.Should().BeTrue();
    }
}