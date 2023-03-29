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

using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Tests.Domain.SprintModel.SprintListExtensionsTests
{
    public class ToSprintList
    {
        [Fact]
        public void HavingNullEnumeration_WhenConvertedToSprintList_ThenThrows()
        {
            IEnumerable<Sprint> sprints = null;

            Action action = () =>
            {
                sprints.ToSprintList();
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void HavingEmptyEnumeration_WhenConvertedToSprintList_ThenReturnsEmptySprintList()
        {
            IEnumerable<Sprint> sprints = Enumerable.Empty<Sprint>();

            SprintList sprintList = sprints.ToSprintList();

            sprintList.Should().BeEmpty();
        }

        [Fact]
        public void HavingEnumerationWithOneSprint_WhenConvertedToSprintList_ThenSprintListContainsThatSprintInstance()
        {
            Sprint sprint = new();
            IEnumerable<Sprint> sprints = new[] { sprint };

            SprintList sprintList = sprints.ToSprintList();

            Sprint[] expectedCollection = { sprint };
            sprintList.Should().Equal(expectedCollection);
        }

        [Fact]
        public void HavingEnumerationWithThreeSprints_WhenConvertedToSprintList_ThenSprintListContainsTheSprintInSameOrder()
        {
            Sprint sprint1 = new();
            Sprint sprint2 = new();
            Sprint sprint3 = new();
            IEnumerable<Sprint> sprints = new[] { sprint1, sprint2, sprint3 };

            SprintList sprintList = sprints.ToSprintList();

            Sprint[] expectedCollection = { sprint1, sprint2, sprint3 };
            sprintList.Should().Equal(expectedCollection);
        }
    }
}