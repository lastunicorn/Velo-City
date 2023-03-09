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
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Infrastructure;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Infrastructure
{
    public class EventBusTests
    {
        private readonly EventBus eventBus;

        public EventBusTests()
        {
            eventBus = new EventBus();
        }

        [Fact]
        public async Task HavingNoSubscriber_WhenAnEventIsPublished_ThenDoesNotThrow()
        {
            // Act
            Func<Task> action = async () =>
            {
                DummyEvent1 dummyEvent1 = new();
                await eventBus.Publish(dummyEvent1);
            };

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task HavingOneSubscriberToDummyEvent1_WhenThatEventIsPublished_ThenSubscriberIsAnnounced()
        {
            // Arrange
            MockSubscriber<DummyEvent1> mockSubscriber1 = new();
            mockSubscriber1.SubscribeTo(eventBus);

            // Act
            DummyEvent1 dummyEvent1 = new();
            await eventBus.Publish(dummyEvent1);

            // Assert
            mockSubscriber1.EventWasTriggered.Should().BeTrue();
        }

        [Fact]
        public async Task HavingOneSubscriberToDummyEvent1_WhenThatEventIsPublished_ThenSameEventInstanceIsReceivedBySubscriber()
        {
            // Arrange
            MockSubscriber<DummyEvent1> mockSubscriber1 = new();
            mockSubscriber1.SubscribeTo(eventBus);

            // Act
            DummyEvent1 dummyEvent1 = new();
            await eventBus.Publish(dummyEvent1);

            // Assert
            mockSubscriber1.Event.Should().BeSameAs(dummyEvent1);
        }

        [Fact]
        public async Task HavingOneSubscriberToDummyEvent1_WhenAnotherEventIsPublished_ThenSubscriberNotAnnounced()
        {
            // Arrange
            MockSubscriber<DummyEvent1> mockSubscriber1 = new();
            mockSubscriber1.SubscribeTo(eventBus);

            // Act
            DummyEvent2 dummyEvent2 = new();
            await eventBus.Publish(dummyEvent2);

            // Assert
            mockSubscriber1.EventWasTriggered.Should().BeFalse();
        }

        [Fact]
        public async Task HavingTwoSubscribersToTwoDifferentEvents_WhenOneEventIsPublished_ThenCorrespondingSubscriberIsAnnounced()
        {
            // Arrange
            MockSubscriber<DummyEvent1> mockSubscriber1 = new();
            mockSubscriber1.SubscribeTo(eventBus);

            MockSubscriber<DummyEvent2> mockSubscriber2 = new();
            mockSubscriber2.SubscribeTo(eventBus);

            // Act
            DummyEvent2 dummyEvent2 = new();
            await eventBus.Publish(dummyEvent2);

            // Assert
            mockSubscriber1.EventWasTriggered.Should().BeFalse();
            mockSubscriber2.EventWasTriggered.Should().BeTrue();
        }

        [Fact]
        public async Task HavingTwoSubscribersToSameEvent_WhenThatEventIsPublished_ThenBothSubscriberAreAnnounced()
        {
            // Arrange
            MockSubscriber<DummyEvent1> mockSubscriber1 = new();
            mockSubscriber1.SubscribeTo(eventBus);

            MockSubscriber<DummyEvent1> mockSubscriber2 = new();
            mockSubscriber2.SubscribeTo(eventBus);

            // Act
            DummyEvent1 dummyEvent1 = new();
            await eventBus.Publish(dummyEvent1);

            // Assert
            mockSubscriber1.EventWasTriggered.Should().BeTrue();
            mockSubscriber2.EventWasTriggered.Should().BeTrue();
        }
    }
}
