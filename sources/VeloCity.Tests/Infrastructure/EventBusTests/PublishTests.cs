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

using DustInTheWind.VeloCity.Infrastructure;

namespace DustInTheWind.VeloCity.Tests.Infrastructure.EventBusTests;

public class PublishTests
{
    private readonly EventBus eventBus;

    public PublishTests()
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
        EventBusClient<DummyEvent1> eventBusClient1 = eventBus.CreateMockSubscriberFor<DummyEvent1>();

        // Act
        DummyEvent1 dummyEvent1 = new();
        await eventBus.Publish(dummyEvent1);

        // Assert
        eventBusClient1.EventWasTriggered.Should().BeTrue();
    }

    [Fact]
    public async Task HavingOneSubscriberToDummyEvent1_WhenThatEventIsPublished_ThenSameEventInstanceIsReceivedBySubscriber()
    {
        // Arrange
        EventBusClient<DummyEvent1> eventBusClient1 = eventBus.CreateMockSubscriberFor<DummyEvent1>();

        // Act
        DummyEvent1 dummyEvent1 = new();
        await eventBus.Publish(dummyEvent1);

        // Assert
        eventBusClient1.Event.Should().BeSameAs(dummyEvent1);
    }

    [Fact]
    public async Task HavingOneSubscriberToDummyEvent1_WhenAnotherEventIsPublished_ThenSubscriberNotAnnounced()
    {
        // Arrange
        EventBusClient<DummyEvent1> eventBusClient1 = eventBus.CreateMockSubscriberFor<DummyEvent1>();

        // Act
        DummyEvent2 dummyEvent2 = new();
        await eventBus.Publish(dummyEvent2);

        // Assert
        eventBusClient1.EventWasTriggered.Should().BeFalse();
    }

    [Fact]
    public async Task HavingTwoSubscribersToTwoDifferentEvents_WhenOneEventIsPublished_ThenCorrespondingSubscriberIsAnnounced()
    {
        // Arrange
        EventBusClient<DummyEvent1> eventBusClient1 = eventBus.CreateMockSubscriberFor<DummyEvent1>();
        EventBusClient<DummyEvent2> eventBusClient2 = eventBus.CreateMockSubscriberFor<DummyEvent2>();

        // Act
        DummyEvent2 dummyEvent2 = new();
        await eventBus.Publish(dummyEvent2);

        // Assert
        eventBusClient1.EventWasTriggered.Should().BeFalse();
        eventBusClient2.EventWasTriggered.Should().BeTrue();
    }

    [Fact]
    public async Task HavingTwoSubscribersToSameEvent_WhenThatEventIsPublished_ThenBothSubscriberAreAnnounced()
    {
        // Arrange
        EventBusClient<DummyEvent1> eventBusClient1 = eventBus.CreateMockSubscriberFor<DummyEvent1>();
        EventBusClient<DummyEvent1> eventBusClient2 = eventBus.CreateMockSubscriberFor<DummyEvent1>();

        // Act
        DummyEvent1 dummyEvent1 = new();
        await eventBus.Publish(dummyEvent1);

        // Assert
        eventBusClient1.EventWasTriggered.Should().BeTrue();
        eventBusClient2.EventWasTriggered.Should().BeTrue();
    }
}