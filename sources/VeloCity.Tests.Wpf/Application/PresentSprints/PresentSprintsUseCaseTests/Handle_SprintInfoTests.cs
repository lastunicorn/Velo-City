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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprints;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprints.PresentSprintsUseCaseTests;

public class Handle_SprintInfoTests
{
    private readonly PresentSprintsUseCase useCase;
    private readonly List<Sprint> sprintsFromRepository;

    public Handle_SprintInfoTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        sprintsFromRepository = new List<Sprint>();

        sprintRepository
            .Setup(x => x.GetAll())
            .Returns(sprintsFromRepository);

        unitOfWork
            .SetupGet(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        ApplicationState applicationState = new();

        useCase = new PresentSprintsUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingSprintRepositoryReturnOneSprint_WhenUseCaseIsExecuted_ThenReturnedSprintInfoContainsSprintId()
    {
        sprintsFromRepository.AddRange(new[]
        {
            new Sprint
            {
                Id = 482
            }
        });

        PresentSprintsRequest request = new();

        PresentSprintsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Sprints[0].Id.Should().Be(482);
    }

    [Fact]
    public async Task HavingSprintRepositoryReturnOneSprint_WhenUseCaseIsExecuted_ThenReturnedSprintInfoContainsSprintTitle()
    {
        sprintsFromRepository.AddRange(new[]
        {
            new Sprint
            {
                Title = "some title"
            }
        });

        PresentSprintsRequest request = new();

        PresentSprintsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Sprints[0].Title.Should().Be("some title");
    }

    [Fact]
    public async Task HavingSprintRepositoryReturnOneSprint_WhenUseCaseIsExecuted_ThenReturnedSprintInfoContainsSprintNumber()
    {
        sprintsFromRepository.AddRange(new[]
        {
            new Sprint
            {
                Number = 95762
            }
        });

        PresentSprintsRequest request = new();

        PresentSprintsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Sprints[0].Number.Should().Be(95762);
    }

    [Fact]
    public async Task HavingSprintRepositoryReturnOneSprint_WhenUseCaseIsExecuted_ThenReturnedSprintInfoContainsSprintDateInterval()
    {
        sprintsFromRepository.AddRange(new[]
        {
            new Sprint
            {
                DateInterval = new DateInterval(new DateTime(2014, 04, 02), new DateTime(2014, 04, 16))
            }
        });

        PresentSprintsRequest request = new();

        PresentSprintsResponse response = await useCase.Handle(request, CancellationToken.None);

        DateInterval expectedDateInterval = new(new DateTime(2014, 04, 02), new DateTime(2014, 04, 16));
        response.Sprints[0].DateInterval.Should().Be(expectedDateInterval);
    }

    [Fact]
    public async Task HavingSprintRepositoryReturnOneSprint_WhenUseCaseIsExecuted_ThenReturnedSprintInfoContainsSprintState()
    {
        sprintsFromRepository.AddRange(new[]
        {
            new Sprint
            {
                State = SprintState.Closed
            }
        });

        PresentSprintsRequest request = new();

        PresentSprintsResponse response = await useCase.Handle(request, CancellationToken.None);

        response.Sprints[0].State.Should().Be(SprintState.Closed);
    }
}