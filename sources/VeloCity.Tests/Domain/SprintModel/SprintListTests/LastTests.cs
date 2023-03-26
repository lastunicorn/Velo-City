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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.SprintModel.SprintListTests
{
    public class LastTests
    {
        [Fact]
        public void HavingEmptyList_ThenLastIsNull()
        {
            SprintList sprintList = new(Array.Empty<Sprint>());

            sprintList.Last.Should().BeNull();
        }

        [Fact]
        public void HavingListWithOneSprint_ThenLastIsThatSprint()
        {
            Sprint sprint = new();
            Sprint[] sprints = { sprint };
            SprintList sprintList = new(sprints);

            sprintList.Last.Should().BeSameAs(sprint);
        }

        [Fact]
        public void HavingListWithTwoSprintsOutOfOrder_ThenLastIsTheFirstSprint()
        {
            Sprint sprint1 = new()
            {
                DateInterval = new DateInterval(new DateTime(2022, 06, 01), new DateTime(2022, 07, 01))
            };
            Sprint sprint2 = new()
            {
                DateInterval = new DateInterval(new DateTime(1999, 06, 01), new DateTime(1999, 07, 01))
            };
            Sprint[] sprints = { sprint1, sprint2 };
            SprintList sprintList = new(sprints);

            sprintList.Last.Should().BeSameAs(sprint1);
        }

        [Fact]
        public void HavingListWithTwoSprintsInOrder_ThenLastIsTheSecondSprint()
        {
            Sprint sprint1 = new()
            {
                DateInterval = new DateInterval(new DateTime(2022, 06, 01), new DateTime(2022, 07, 01))
            };
            Sprint sprint2 = new()
            {
                DateInterval = new DateInterval(new DateTime(2024, 06, 01), new DateTime(2024, 07, 01))
            };
            Sprint[] sprints = { sprint1, sprint2 };
            SprintList sprintList = new(sprints);

            sprintList.Last.Should().BeSameAs(sprint2);
        }
    }
}