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

using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CanCloseSprint;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.CanCloseSprint.CanCloseSprintUseCaseTests
{
    public class HandleTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<ISprintRepository> sprintRepository;
        private readonly ApplicationState applicationState;
        private readonly CanCloseSprintUseCase useCase;

        public HandleTests()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            sprintRepository = new Mock<ISprintRepository>();

            unitOfWork
                .Setup(x => x.SprintRepository)
                .Returns(sprintRepository.Object);

            applicationState = new ApplicationState();

            useCase = new CanCloseSprintUseCase(unitOfWork.Object, applicationState);
        }

        [Fact]
        public async Task HavingSprintIdSpecifiedInApplicationState_WhenUseCaseIsExecuted_ThenSprintWithSpecifiedIdIsRequestedFromStorage()
        {
            applicationState.SelectedSprintId = 15;

            CanCloseSprintRequest request = new();
            _ = await useCase.Handle(request, CancellationToken.None);

            sprintRepository.Verify(x => x.Get(15), Times.Once);
        }

        [Fact]
        public async Task HavingNoSprintWithIdSpecifiedInApplicationState_WhenUseCaseIsExecuted_ThenCanCloseSprintIsFalse()
        {
            applicationState.SelectedSprintId = null;

            CanCloseSprintRequest request = new();
            CanCloseSprintResponse response = await useCase.Handle(request, CancellationToken.None);

            response.CanCloseSprint.Should().BeFalse();
        }

        [Fact]
        public async Task HavingSprintWithInvalidStateInStorage_WhenUseCaseIsExecuted_ThenCanCloseSprintIsFalse()
        {
            applicationState.SelectedSprintId = 15;

            Sprint sprintFromStorage = new()
            {
                State = (SprintState)98732497
            };

            sprintRepository
                .Setup(x => x.Get(15))
                .Returns(sprintFromStorage);

            CanCloseSprintRequest request = new();
            CanCloseSprintResponse response = await useCase.Handle(request, CancellationToken.None);

            response.CanCloseSprint.Should().BeFalse();
        }

        [Theory]
        [InlineData(SprintState.Unknown, false)]
        [InlineData(SprintState.New, false)]
        [InlineData(SprintState.InProgress, true)]
        [InlineData(SprintState.Closed, false)]
        public async Task HavingSprintWithSpecificStateInStorage_WhenUseCaseIsExecuted_ThenCanCloseSprintHasExpectedValueBasedOnState(SprintState sprintState, bool canCloseSprint)
        {
            applicationState.SelectedSprintId = 15;

            Sprint sprintFromStorage = new()
            {
                State = sprintState
            };

            sprintRepository
                .Setup(x => x.Get(15))
                .Returns(sprintFromStorage);

            CanCloseSprintRequest request = new();
            CanCloseSprintResponse response = await useCase.Handle(request, CancellationToken.None);

            response.CanCloseSprint.Should().Be(canCloseSprint);
        }
    }
}
