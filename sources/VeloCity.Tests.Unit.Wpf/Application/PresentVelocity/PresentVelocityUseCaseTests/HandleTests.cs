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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.PresentVelocity;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentVelocity.PresentVelocityUseCaseTests;

public class HandleTests
{
    private readonly PresentVelocityUseCase useCase;
    private readonly Mock<ISprintRepository> sprintRepository;

    public HandleTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        useCase = new PresentVelocityUseCase(unitOfWork.Object);
    }

    [Fact]
    public async Task HavingSprintCountSpecifiedInRequest_WhenUseCaseIsExecuted_ThenRetrieveFromRepositoryTheSpecifiedCountOfClosedSprints()
    {
        PresentVelocityRequest request = new()
        {
            SprintCount = 65
        };
        await useCase.Handle(request, CancellationToken.None);

        sprintRepository.Verify(x => x.GetLastClosed(65), Times.Once);
    }

    [Fact]
    public async Task HavingSprintCountSpecifiedInRequest_WhenUseCaseIsExecuted_ThenThatSprintCountValueIsReturnedInTheResponse()
    {
        PresentVelocityRequest request = new()
        {
            SprintCount = 48
        };
        PresentVelocityResponse response = await useCase.Handle(request, CancellationToken.None);

        response.RequestedSprintCount.Should().Be(48);
    }

    [Fact]
    public async Task HavingNoSprintCountSpecifiedInRequest_WhenUseCaseIsExecuted_ThenRetrieveFromRepository10ClosedSprints()
    {
        PresentVelocityRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        sprintRepository.Verify(x => x.GetLastClosed(10), Times.Once);
    }

    [Fact]
    public async Task HavingNoSprintCountSpecifiedInRequest_WhenUseCaseIsExecuted_ThenSprintCount10IsReturnedInTheResponse()
    {
        PresentVelocityRequest request = new();
        PresentVelocityResponse response = await useCase.Handle(request, CancellationToken.None);

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
            .ReturnsAsync(sprintsFromRepository);

        PresentVelocityRequest request = new();
        PresentVelocityResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintVelocities.Count.Should().Be(1);
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
            .ReturnsAsync(sprintsFromRepository);

        PresentVelocityRequest request = new();
        PresentVelocityResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintVelocities.Count.Should().Be(2);
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
            .ReturnsAsync(sprintsFromRepository);

        PresentVelocityRequest request = new();
        PresentVelocityResponse response = await useCase.Handle(request, CancellationToken.None);

        IEnumerable<int> actualSprintNumbers = response.SprintVelocities
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
            .ReturnsAsync(sprintsFromRepository);

        PresentVelocityRequest request = new();
        PresentVelocityResponse response = await useCase.Handle(request, CancellationToken.None);

        IEnumerable<int> actualSprintNumbers = response.SprintVelocities
            .Select(x => x.SprintNumber);

        int[] expectedSprintNumbers = { 2, 1 };
        actualSprintNumbers.Should().Equal(expectedSprintNumbers);
    }
}