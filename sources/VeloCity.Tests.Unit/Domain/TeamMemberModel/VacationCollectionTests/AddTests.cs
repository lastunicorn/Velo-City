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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.VacationCollectionTests;

public class AddTests
{
    private readonly VacationCollection vacationCollection;

    public AddTests()
    {
        vacationCollection = new VacationCollection();
    }

    [Fact]
    public void HavingVacationCollection_WhenAddingOneVacation_ThenCollectionContainsThatVacation()
    {
        SingleDayVacation singleDayVacation = new();
        vacationCollection.Add(singleDayVacation);

        vacationCollection.Single().Should().BeSameAs(singleDayVacation);
    }

    [Fact]
    public void HavingVacationCollection_WhenAddingTwoVacations_ThenCollectionContainsThoseTwoVacations()
    {
        SingleDayVacation vacation1 = new();
        vacationCollection.Add(vacation1);

        SingleDayVacation vacation2 = new();
        vacationCollection.Add(vacation2);

        vacationCollection.Should().BeEquivalentTo(new[] { vacation1, vacation2 });
    }

    [Fact]
    public void HavingVacationAddedToCollection_WhenVacationIsChanged_ThenCollectionRaiseChangeEvent()
    {
        bool wasEventRaised = false;
        vacationCollection.Changed += (sender, args) => wasEventRaised = true;

        SingleDayVacation singleDayVacation = new();
        vacationCollection.Add(singleDayVacation);

        singleDayVacation.Date = new DateTime(2023, 03, 27);

        wasEventRaised.Should().BeTrue();
    }
}