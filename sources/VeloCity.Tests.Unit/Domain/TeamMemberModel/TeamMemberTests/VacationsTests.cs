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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.TeamMemberTests;

public class VacationsTests
{
    private readonly VacationCollection vacationCollection;
    private readonly TeamMember teamMember;

    public VacationsTests()
    {
        vacationCollection = new VacationCollection();
        teamMember = new TeamMember
        {
            Vacations = vacationCollection
        };
    }

    [Fact]
    public void HavingAVacationCollectionSet_WhenCollectionIsChanged_ThenRaiseVacationsChangedEvent()
    {
        bool eventWasTriggered = false;
        teamMember.VacationsChanged += (sender, args) => eventWasTriggered = true;

        vacationCollection.Add(new DailyVacation());

        eventWasTriggered.Should().BeTrue();
    }

    [Fact]
    public void HavingVacationCollectionReplacedWithAnotherOne_WhenInitialCollectionIsChanged_ThenDoesNotRaiseVacationsChangedEvent()
    {
        teamMember.Vacations = new VacationCollection();
        bool eventWasTriggered = false;
        teamMember.VacationsChanged += (sender, args) => eventWasTriggered = true;

        vacationCollection.Add(new DailyVacation());

        eventWasTriggered.Should().BeFalse();
    }

    [Fact]
    public void HavingVacationCollectionReplacedWithNull_WhenCollectionIsChanged_ThenDoesNotRaiseVacationsChangedEvent()
    {
        teamMember.Vacations = null;
        bool eventWasTriggered = false;
        teamMember.VacationsChanged += (sender, args) => eventWasTriggered = true;

        vacationCollection.Add(new DailyVacation());

        eventWasTriggered.Should().BeFalse();
    }

    [Fact]
    public void HavingVacationCollectionReplacedWithAnotherOne_WhenTheNewCollectionIsChanged_ThenRaiseVacationsChangedEvent()
    {
        VacationCollection newVacationCollection = new();
        teamMember.Vacations = newVacationCollection;
        bool eventWasTriggered = false;
        teamMember.VacationsChanged += (sender, args) => eventWasTriggered = true;

        newVacationCollection.Add(new DailyVacation());

        eventWasTriggered.Should().BeTrue();
    }
}