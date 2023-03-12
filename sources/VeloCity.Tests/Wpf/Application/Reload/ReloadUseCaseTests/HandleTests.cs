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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.Reload.ReloadUseCaseTests
{
    public class HandleTests
    {
        private readonly EventBus eventBus;
        private readonly Mock<IDataStorage> dataStorage;
        private readonly ReloadUseCase useCase;

        public HandleTests()
        {
            eventBus = new EventBus();
            dataStorage = new Mock<IDataStorage>();

            useCase = new ReloadUseCase(eventBus, dataStorage.Object);
        }

        [Fact]
        public async Task HavingUseCaseInstance_WhenUseCaseIsExecuted_ThenDataStorageIsReopened()
        {
            ReloadRequest request = new();
            await useCase.Handle(request, CancellationToken.None);

            dataStorage.Verify(x => x.Reopen(), Times.Once);
        }

        [Fact]
        public async Task HavingDataStorageThatThrows_WhenUseCaseIsExecuted_ThenDoesNotThrow()
        {
            dataStorage
                .Setup(x => x.Reopen())
                .Throws<Exception>();

            ReloadRequest request = new();

            Func<Task> action = async () =>
            {
                await useCase.Handle(request, CancellationToken.None);
            };

            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HavingUseCaseInstance_WhenUseCaseIsExecuted_ThenRaiseReloadEvent()
        {
            EventBusClient<ReloadEvent> eventBusClient = eventBus.CreateMockSubscriberFor<ReloadEvent>();

            ReloadRequest request = new();
            await useCase.Handle(request, CancellationToken.None);

            eventBusClient.VerifyEventWasTriggered(1);
        }

        [Fact]
        public async Task HavingDataStorageThatThrows_WhenUseCaseIsExecuted_ThenDoesNotRaiseReloadEvent()
        {
            EventBusClient<ReloadEvent> eventBusClient = eventBus.CreateMockSubscriberFor<ReloadEvent>();

            dataStorage
                .Setup(x => x.Reopen())
                .Throws<Exception>();

            ReloadRequest request = new();

            await useCase.Handle(request, CancellationToken.None);

            eventBusClient.VerifyEventWasTriggered(0);
        }
    }
}
