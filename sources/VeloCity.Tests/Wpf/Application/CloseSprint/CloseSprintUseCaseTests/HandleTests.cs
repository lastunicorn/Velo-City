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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintCloseConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CloseSprint;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.CloseSprint.CloseSprintUseCaseTests
{
    public class HandleTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<ISprintRepository> sprintRepository;
        private readonly ApplicationState applicationState;
        private readonly EventBus eventBus;
        private readonly Mock<IUserInterface> userInterface;
        private readonly CloseSprintUseCase useCase;

        public HandleTests()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            sprintRepository = new Mock<ISprintRepository>();
            applicationState = new ApplicationState();
            eventBus = new EventBus();
            userInterface = new Mock<IUserInterface>();

            unitOfWork
                .Setup(x => x.SprintRepository)
                .Returns(sprintRepository.Object);

            useCase = new CloseSprintUseCase(unitOfWork.Object, applicationState, eventBus, userInterface.Object);
        }

        [Fact]
        public async Task HavingNoSprintIdSetInApplicationState_WhenUseCaseIsExecuted_ThenThrows()
        {
            applicationState.SelectedSprintId = null;
            CloseSprintRequest request = new CloseSprintRequest();

            Func<Task> action = async () =>
            {
                await useCase.Handle(request, CancellationToken.None);
            };

            await action.Should().ThrowAsync<NoSprintSelectedException>();
        }

        [Fact]
        public async Task HavingSprintIdSetInApplicationState_WhenUseCaseIsExecuted_ThenThatSprintIsRetrievedFromRepository()
        {
            applicationState.SelectedSprintId = 247;
            Sprint sprintFromRepository = new Sprint()
            {
                State = SprintState.InProgress
            };

            sprintRepository
                .Setup(x => x.Get(It.IsAny<int>()))
                .Returns(sprintFromRepository);

            userInterface
                .Setup(x => x.ConfirmCloseSprint(It.IsAny<SprintCloseConfirmationRequest>()))
                .Returns(new SprintCloseConfirmationResponse());

            CloseSprintRequest request = new CloseSprintRequest();
            await useCase.Handle(request, CancellationToken.None);

            sprintRepository.Verify(x => x.Get(247), Times.Once);
        }

        [Fact]
        public async Task HavingSprintIdSetInApplicationStateButNoSprintInRepository_WhenUseCaseIsExecuted_ThenThrows()
        {
            applicationState.SelectedSprintId = 247;

            sprintRepository
                .Setup(x => x.Get(It.IsAny<int>()))
                .Returns(null as Sprint);

            CloseSprintRequest request = new CloseSprintRequest();
            Func<Task> action = async () =>
            {
                await useCase.Handle(request, CancellationToken.None);
            };

            await action.Should().ThrowAsync<SprintDoesNotExistException>();
        }

        [Fact]
        public async Task HavingValidInProgressSprintInRepositoryAndNullResponseFromTheUser_WhenUseCaseIsExecuted_ThenThrows()
        {
            applicationState.SelectedSprintId = 247;
            Sprint sprintFromRepository = new Sprint()
            {
                State = SprintState.InProgress
            };

            sprintRepository
                .Setup(x => x.Get(It.IsAny<int>()))
                .Returns(sprintFromRepository);

            userInterface
                .Setup(x => x.ConfirmCloseSprint(It.IsAny<SprintCloseConfirmationRequest>()))
                .Returns(null as SprintCloseConfirmationResponse);

            CloseSprintRequest request = new CloseSprintRequest();
            Func<Task> action = async () =>
            {
                await useCase.Handle(request, CancellationToken.None);
            };

            await action.Should().ThrowAsync<InternalException>();
        }
    }
}
