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
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.SetCurrentSprint.SetCurrentSprintUseCaseTests
{
    public class HandleTests
    {
        private readonly EventBus eventBus;
        private readonly ApplicationState applicationState;
        private readonly SetCurrentSprintUseCase useCase;

        public HandleTests()
        {
            applicationState = new ApplicationState();
            eventBus = new EventBus();

            useCase = new SetCurrentSprintUseCase(applicationState, eventBus);
        }

        [Fact]
        public async Task HavingSprintIdSpecifiedInRequest_WhenUseCaseIsExecuted_ThenSprintIdIsSetInApplicationState()
        {
            SetCurrentSprintRequest request = new()
            {
                SprintId = 3789
            };
            await useCase.Handle(request, CancellationToken.None);

            applicationState.SelectedSprintId.Should().Be(3789);
        }

        [Fact]
        public async Task HavingSprintIdNotSpecifiedInRequest_WhenUseCaseIsExecuted_ThenSprintIdIsSetToNullInApplicationState()
        {
            applicationState.SelectedSprintId = 100;

            SetCurrentSprintRequest request = new();
            await useCase.Handle(request, CancellationToken.None);

            applicationState.SelectedSprintId.Should().BeNull();
        }

        [Fact]
        public async Task HavingUseCaseInstance_WhenUseCaseIsExecuted_ThenRaiseSprintChangedEvent()
        {
            EventBusClient<SprintChangedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintChangedEvent>();

            SetCurrentSprintRequest request = new();
            await useCase.Handle(request, CancellationToken.None);

            eventBusClient.VerifyEventWasTriggered(1);
        }

        [Fact]
        public async Task HavingUseCaseInstance_WhenUseCaseIsExecuted_ThenSprintChangedEventContainSprintNumber()
        {
            EventBusClient<SprintChangedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<SprintChangedEvent>();

            SetCurrentSprintRequest request = new()
            {
                SprintId = 3789
            };
            await useCase.Handle(request, CancellationToken.None);

            eventBusClient.Event.NewSprintNumber.Should().Be(3789);
        }
    }
}
