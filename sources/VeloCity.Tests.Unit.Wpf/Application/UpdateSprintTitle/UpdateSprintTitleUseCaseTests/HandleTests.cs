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
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using DustInTheWind.VeloCity.Wpf.Application.UpdateSprintTitle;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.UpdateSprintTitle.UpdateSprintTitleUseCaseTests;

public class HandleTests
{
    private readonly UpdateSprintTitleUseCase useCase;
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly EventBus eventBus;

    public HandleTests()
    {
        unitOfWork = new Mock<IUnitOfWork>();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .SetupGet(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        eventBus = new EventBus();

        useCase = new UpdateSprintTitleUseCase(unitOfWork.Object, eventBus);
    }

    [Fact]
    public async Task HavingSprintIdInRequest_WhenHandleRequest_ThenSprintIsRetrievedFromRepository()
    {
        UpdateSprintTitleRequest request = new()
        {
            SprintId = 364
        };

        try
        {
            await useCase.Handle(request, CancellationToken.None);
        }
        catch { }

        sprintRepository.Verify(x => x.Get(364), Times.Once);
    }

    [Fact]
    public async Task HavingSprintIdThatIsNotFoundInRepository_WhenHandleRequest_ThenThrows()
    {
        UpdateSprintTitleRequest request = new()
        {
            SprintId = 364
        };

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(null as Sprint);

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<SprintDoesNotExistException>();
    }

    [Fact]
    public async Task HavingSprintIdFoundInRepository_WhenHandleRequest_ThenTitleIsUpdatedWithRequestedValue()
    {
        Sprint sprint = new();

        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(sprint);

        UpdateSprintTitleRequest request = new()
        {
            SprintId = 364,
            SprintTitle = "bla bla"
        };

        await useCase.Handle(request, CancellationToken.None);

        sprint.Title.Should().Be("bla bla");
    }

    [Fact]
    public async Task HavingSprintIdFoundInRepository_WhenHandleRequest_ThenChangesArePersisted()
    {
        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(new Sprint());

        UpdateSprintTitleRequest request = new()
        {
            SprintId = 364,
            SprintTitle = "bla bla"
        };

        await useCase.Handle(request, CancellationToken.None);

        unitOfWork.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task HavingSprintIdFoundInRepository_WhenHandleRequest_ThenSprintUpdatedEventIsPublished()
    {
        sprintRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(new Sprint());

        EventBusClient<SprintUpdatedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintUpdatedEvent>();

        UpdateSprintTitleRequest request = new();

        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.EventWasTriggered.Should().BeTrue();
    }
}