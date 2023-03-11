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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprints;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprints.PresentSprintsUseCaseTests
{
    public class HandleTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<ISprintRepository> sprintRepository;
        private readonly ApplicationState applicationState;
        private readonly PresentSprintsUseCase useCase;
        private readonly List<Sprint> sprintsFromRepository;

        public HandleTests()
        {
            unitOfWork = new();
            sprintRepository = new();

            sprintsFromRepository = new List<Sprint>();

            sprintRepository
                .Setup(x => x.GetAll())
                .Returns(sprintsFromRepository);

            unitOfWork
                .SetupGet(x => x.SprintRepository)
                .Returns(sprintRepository.Object);

            applicationState = new();

            useCase = new(unitOfWork.Object, applicationState);
        }

        [Fact]
        public async Task HavingASprintIdInApplicationState_WhenUseCaseIsExecuted_ThenReturnsThatSprintAsCurrentSprint()
        {
            applicationState.SelectedSprintNumber = 659;
            PresentSprintsRequest request = new();

            PresentSprintsResponse response = await useCase.Handle(request, CancellationToken.None);

            response.CurrentSprintId.Should().Be(659);
        }

        [Fact]
        public async Task HavingSprintRepositoryReturnTwoSprintsInOrderByStartDate_WhenUseCaseIsExecuted_ThenReturnsTwoSprintsInInverseOrderByStartDate()
        {
            sprintsFromRepository.AddRange(new[]
            {
                new Sprint
                {
                    Id = 1,
                    DateInterval = new(new DateTime(2022, 05, 01), new DateTime(2022, 05, 31))
                },
                new Sprint
                {
                    Id = 2,
                    DateInterval = new(new DateTime(2022, 06, 01), new DateTime(2022, 06, 30))
                }
            });

            PresentSprintsRequest request = new();

            PresentSprintsResponse response = await useCase.Handle(request, CancellationToken.None);

            AssertSprintIds(response.Sprints, new[] { 2, 1 });
        }

        [Fact]
        public async Task HavingSprintRepositoryReturnTwoSprintsOutOfOrderByStartDate_WhenUseCaseIsExecuted_ThenReturnsTwoSprintsInInverseOrderByStartDate()
        {
            sprintsFromRepository.AddRange(new[]
            {
                new Sprint
                {
                    Id = 1,
                    DateInterval = new(new DateTime(2022, 06, 01), new DateTime(2022, 06, 30))
                },
                new Sprint
                {
                    Id = 2,
                    DateInterval = new(new DateTime(2022, 05, 01), new DateTime(2022, 05, 31))
                }
            });

            PresentSprintsRequest request = new();

            PresentSprintsResponse response = await useCase.Handle(request, CancellationToken.None);

            AssertSprintIds(response.Sprints, new[] { 1, 2 });
        }

        private static void AssertSprintIds(IEnumerable<SprintInfo> actualSprints, int[] expectedSprintIds)
        {
            int[] actualSprintIds = actualSprints
                .Select(x => x.Id)
                .ToArray();

            actualSprintIds.Should().Equal(expectedSprintIds);
        }
    }
}
