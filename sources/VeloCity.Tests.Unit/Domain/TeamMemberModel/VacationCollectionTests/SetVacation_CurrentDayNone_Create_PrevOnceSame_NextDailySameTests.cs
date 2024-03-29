﻿// VeloCity
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
//             (=)     [=============]
//             (=) (=) [=============]
//             [=====================]

public class SetVacation_CurrentDayNone_Create_PrevOnceSame_NextDailySameTests
{
    private readonly VacationCollection vacationCollection;
    private readonly DateTime currentDate;
    private readonly DateTime previousDate;
    private readonly SingleDayVacation previousVacation;
    private readonly DailyVacation nextVacation;
    private readonly DateTime maxDate;

    public SetVacation_CurrentDayNone_Create_PrevOnceSame_NextDailySameTests()
    {
        currentDate = new DateTime(2023, 03, 27);
        previousDate = new DateTime(2023, 03, 26);
        DateTime nextDate = new(2023, 03, 28);
        maxDate = new DateTime(3000, 01, 01);

        vacationCollection = new VacationCollection();

        previousVacation = new SingleDayVacation
        {
            Date = previousDate,
            HourCount = 8
        };
        vacationCollection.Add(previousVacation);

        nextVacation = new DailyVacation
        {
            DateInterval = new DateInterval(nextDate, maxDate),
            HourCount = 8
        };
        vacationCollection.Add(nextVacation);
    }

    [Fact]
    public void HavingPrevOnceSameNextDailySame_WhenSettingVacationForCurrentDay_ThenCurrentDayVacationSpansFromPrevToNextEnd()
    {
        vacationCollection.SetVacation(currentDate, 8);

        DailyVacation actualVacation = vacationCollection.GetVacationsFor(currentDate).Single() as DailyVacation;

        DateInterval expectedDateInterval = new(previousDate, maxDate);
        actualVacation.DateInterval.Should().Be(expectedDateInterval);
    }

    [Fact]
    public void HavingPrevOnceSameNextDailySame_WhenSettingVacationForCurrentDay_ThenCurrentDayVacationHasCorrectHours()
    {
        vacationCollection.SetVacation(currentDate, 8);

        Vacation actualVacation = vacationCollection.GetVacationsFor(currentDate).Single();

        actualVacation.HourCount.Should().Be(8);
    }

    [Fact]
    public void HavingPrevOnceSameNextDailySame_WhenSettingVacationForCurrentDay_ThenOldPreviousDayVacationIsRemoved()
    {
        vacationCollection.SetVacation(currentDate, 8);

        List<Vacation> actualVacations = vacationCollection.GetVacationsFor(previousDate).ToList();

        actualVacations.Count.Should().Be(1);
        actualVacations[0].Should().BeOfType<DailyVacation>();
    }

    [Fact]
    public void HavingPrevOnceSameNextDailySame_WhenSettingVacationForCurrentDay_ThenPreviousVacationDoesNotTriggerChangedEventAnymore()
    {
        vacationCollection.SetVacation(currentDate, 8);

        bool wasEventTriggered = false;
        vacationCollection.Changed += (sender, args) => wasEventTriggered = true;

        previousVacation.HourCount = 100;

        wasEventTriggered.Should().BeFalse();
    }

    [Fact]
    public void HavingPrevOnceSameNextDailySame_WhenSettingVacationForCurrentDay_ThenNextVacationStillTriggersChangedEvent()
    {
        vacationCollection.SetVacation(currentDate, 8);

        bool wasEventTriggered = false;
        vacationCollection.Changed += (sender, args) => wasEventTriggered = true;

        nextVacation.HourCount = 100;

        wasEventTriggered.Should().BeTrue();
    }

    [Fact]
    public void HavingPrevOnceSameNextDailySame_WhenSettingVacationForCurrentDay_ThenCurrentVacationTriggersChangedEvent()
    {
        vacationCollection.SetVacation(currentDate, 8);

        bool wasEventTriggered = false;
        vacationCollection.Changed += (sender, args) => wasEventTriggered = true;

        Vacation currentVacation = vacationCollection.GetVacationsFor(previousDate).Single();
        currentVacation.HourCount = 100;

        wasEventTriggered.Should().BeTrue();
    }
}