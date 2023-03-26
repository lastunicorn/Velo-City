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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintsCapacity;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprintsCapacity.PresentSprintsCapacityUseCaseTests;

public class HandleTests
{
    private readonly PresentSprintsCapacityUseCase useCase;
    private readonly Mock<ISprintRepository> sprintRepository;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        useCase = new PresentSprintsCapacityUseCase(unitOfWork.Object);
    }

    [Fact]
    public async Task HavingSprintCountSpecifiedInRequest_WhenUseCaseIsExecuted_ThenRetrieveFromRepositoryTheSpecifiedCountOfClosedSprints()
    {
        PresentSprintsCapacityRequest request = new()
        {
            SprintCount = 49
        };
        await useCase.Handle(request, CancellationToken.None);

        sprintRepository.Verify(x => x.GetLastClosed(49), Times.Once);
    }

    [Fact]
    public async Task HavingSprintCountSpecifiedInRequest_WhenUseCaseIsExecuted_ThenThatSprintCountValueIsReturnedInTheResponse()
    {
        PresentSprintsCapacityRequest request = new()
        {
            SprintCount = 86
        };
        PresentSprintsCapacityResponse response = await useCase.Handle(request, CancellationToken.None);

        response.RequestedSprintCount.Should().Be(86);
    }

    [Fact]
    public async Task HavingNoSprintCountSpecifiedInRequest_WhenUseCaseIsExecuted_ThenRetrieveFromRepository10ClosedSprints()
    {
        PresentSprintsCapacityRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintRepository.Verify(x => x.GetLastClosed(10), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintCountSpecifiedInRequest_WhenUseCaseIsExecuted_ThenSprintCount10IsReturnedInTheResponse()
    {
        PresentSprintsCapacityRequest request = new();
        PresentSprintsCapacityResponse response = await useCase.Handle(request, CancellationToken.None);

        response.RequestedSprintCount.Should().Be(10);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenOneSprintIsReturnedInTheResponse()
    {
        List<Sprint> sprintsFromRepository = new()
        {
            new Sprint()
        };

        sprintRepository
            .Setup(x => x.GetLastClosed(It.IsAny<uint>()))
            .Returns(sprintsFromRepository);

        PresentSprintsCapacityRequest request = new();
        PresentSprintsCapacityResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintCapacities.Count.Should().Be(1);
    }

    [Fact]
    public async Task HavingTwoSprintsInRepository_WhenUseCaseIsExecuted_ThenTwoSprintsAreReturnedInTheResponse()
    {
        List<Sprint> sprintsFromRepository = new()
        {
            new Sprint(),
            new Sprint()
        };

        sprintRepository
            .Setup(x => x.GetLastClosed(It.IsAny<uint>()))
            .Returns(sprintsFromRepository);

        PresentSprintsCapacityRequest request = new();
        PresentSprintsCapacityResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintCapacities.Count.Should().Be(2);
    }

    [Fact]
    public async Task HavingTwoSprintsInRepositoryInDescendingOrder_WhenUseCaseIsExecuted_ThenTwoSprintsAreReturnedInThatOrder()
    {
        List<Sprint> sprintsFromRepository = new()
        {
            new Sprint
            {
                Number = 1,
                DateInterval = new DateInterval(new DateTime(2022, 10, 03), new DateTime(2022, 10, 17))
            },
            new Sprint
            {
                Number = 2,
                DateInterval = new DateInterval(new DateTime(2022, 09, 03), new DateTime(2022, 09, 17))
            }
        };

        sprintRepository
            .Setup(x => x.GetLastClosed(It.IsAny<uint>()))
            .Returns(sprintsFromRepository);

        PresentSprintsCapacityRequest request = new();
        PresentSprintsCapacityResponse response = await useCase.Handle(request, CancellationToken.None);

        IEnumerable<int> actualSprintNumbers = response.SprintCapacities
            .Select(x => x.SprintNumber);

        int[] expectedSprintNumbers = { 1, 2 };
        actualSprintNumbers.Should().Equal(expectedSprintNumbers);
    }

    [Fact]
    public async Task HavingTwoSprintsInRepositoryInAscendingOrder_WhenUseCaseIsExecuted_ThenTwoSprintsAreReturnedInReversedOrder()
    {
        List<Sprint> sprintsFromRepository = new()
        {
            new Sprint
            {
                Number = 1,
                DateInterval = new DateInterval(new DateTime(2022, 09, 03), new DateTime(2022, 09, 17))
            },
            new Sprint
            {
                Number = 2,
                DateInterval = new DateInterval(new DateTime(2022, 10, 03), new DateTime(2022, 10, 17))
            }
        };

        sprintRepository
            .Setup(x => x.GetLastClosed(It.IsAny<uint>()))
            .Returns(sprintsFromRepository);

        PresentSprintsCapacityRequest request = new();
        PresentSprintsCapacityResponse response = await useCase.Handle(request, CancellationToken.None);

        IEnumerable<int> actualSprintNumbers = response.SprintCapacities
            .Select(x => x.SprintNumber);

        int[] expectedSprintNumbers = { 2, 1 };
        actualSprintNumbers.Should().Equal(expectedSprintNumbers);
    }
}