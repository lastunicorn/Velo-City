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
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintDetails;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentSprintDetails.PresentSprintDetailsUseCaseTests;

public class Handle_SprintNumberFromApplicationStateTests
{
    private readonly Mock<IUnitOfWork> unitOfWork;
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly ApplicationState applicationState;
    private readonly PresentSprintDetailsUseCase useCase;

    public Handle_SprintNumberFromApplicationStateTests()
    {
        unitOfWork = new Mock<IUnitOfWork>();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        applicationState = new ApplicationState();

        useCase = new PresentSprintDetailsUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingSprintIdSpecifiedInApplicationStateButNotExistingInStorage_WhenUseCaseIsExecuted_ThenThrows()
    {
        sprintRepository
            .Setup(x => x.Get(101))
            .ReturnsAsync(null as Sprint);

        applicationState.SelectedSprintId = 101;

        PresentSprintDetailRequest request = new();

        Func<Task> action = async () =>
        {
            await useCase.Handle(request, CancellationToken.None);
        };

        await action.Should().ThrowAsync<SprintDoesNotExistException>();
    }

    [Fact]
    public async Task HavingSprintIdSpecifiedInApplicationState_WhenUseCaseIsExecuted_ThenSprintWithSpecifiedIdIsRequestedFromUnitOfWork()
    {
        Sprint sprintFromStorage = new();

        sprintRepository
            .Setup(x => x.Get(101))
            .ReturnsAsync(sprintFromStorage);

        applicationState.SelectedSprintId = 101;

        PresentSprintDetailRequest request = new();
        PresentSprintDetailResponse response = await useCase.Handle(request, CancellationToken.None);

        sprintRepository.Verify(x => x.Get(101), Times.Once);
    }

    [Fact]
    public async Task HavingSprintIdSpecifiedInTheApplicationState_WhenUseCaseIsExecuted_ThenSprintIdFromStorageIsReturnedInTheResponse()
    {
        Sprint sprintFromStorage = new()
        {
            Id = 54
        };

        sprintRepository
            .Setup(x => x.Get(101))
            .ReturnsAsync(sprintFromStorage);

        applicationState.SelectedSprintId = 101;

        PresentSprintDetailRequest request = new();
        PresentSprintDetailResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintId.Should().Be(54);
    }

    [Fact]
    public async Task HavingSprintIdSpecifiedInTheApplicationState_WhenUseCaseIsExecuted_ThenSprintTitleFromStorageIsReturnedInTheResponse()
    {
        Sprint sprintFromStorage = new()
        {
            Title = "this is a title"
        };

        sprintRepository
            .Setup(x => x.Get(101))
            .ReturnsAsync(sprintFromStorage);

        applicationState.SelectedSprintId = 101;

        PresentSprintDetailRequest request = new();
        PresentSprintDetailResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintTitle.Should().Be("this is a title");
    }

    [Fact]
    public async Task HavingSprintIdSpecifiedInTheApplicationState_WhenUseCaseIsExecuted_ThenSprintNumberFromStorageIsReturnedInTheResponse()
    {
        Sprint sprintFromStorage = new()
        {
            Number = 278
        };

        sprintRepository
            .Setup(x => x.Get(101))
            .ReturnsAsync(sprintFromStorage);

        applicationState.SelectedSprintId = 101;

        PresentSprintDetailRequest request = new();
        PresentSprintDetailResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintNumber.Should().Be(278);
    }

    [Fact]
    public async Task HavingSprintIdSpecifiedInTheApplicationState_WhenUseCaseIsExecuted_ThenSprintStateFromStorageIsReturnedInTheResponse()
    {
        Sprint sprintFromStorage = new()
        {
            State = SprintState.New
        };

        sprintRepository
            .Setup(x => x.Get(101))
            .ReturnsAsync(sprintFromStorage);

        applicationState.SelectedSprintId = 101;

        PresentSprintDetailRequest request = new();
        PresentSprintDetailResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintState.Should().Be(SprintState.New);
    }
}