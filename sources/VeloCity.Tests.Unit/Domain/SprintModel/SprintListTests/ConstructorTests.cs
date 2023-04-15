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

using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.SprintModel.SprintListTests;

public class ConstructorTests
{
    [Fact]
    public void WhenInstantiatingFromNull_ThenThrows()
    {
        Action action = () => new SprintList(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WhenInstantiatingFromEmptyCollection_ThenListIsEmpty()
    {
        SprintList sprintList = new(Array.Empty<Sprint>());

        sprintList.Should().BeEmpty();
    }

    [Fact]
    public void WhenInstantiatingFromCollectionOfOneSprint_ThenListContainsThatSprint()
    {
        Sprint[] collectionOfSprints = { new() };
        SprintList sprintList = new(collectionOfSprints);

        sprintList.Should().HaveCount(1)
            .And.ContainInOrder(collectionOfSprints);
    }

    [Fact]
    public void WhenInstantiatingFromCollectionOfTwoSprints_ThenListContainsThoseSprints()
    {
        Sprint[] collectionOfSprints = { new(), new() };
        SprintList sprintList = new(collectionOfSprints);

        sprintList.Should().HaveCount(2)
            .And.ContainInOrder(collectionOfSprints);
    }
}