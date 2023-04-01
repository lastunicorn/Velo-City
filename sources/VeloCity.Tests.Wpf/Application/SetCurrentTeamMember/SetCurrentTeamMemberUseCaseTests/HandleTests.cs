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
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentTeamMember;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.SetCurrentTeamMember.SetCurrentTeamMemberUseCaseTests;

public class HandleTests
{
    private readonly EventBus eventBus;
    private readonly ApplicationState applicationState;
    private readonly SetCurrentTeamMemberUseCase useCase;

    public HandleTests()
    {
        applicationState = new ApplicationState();
        eventBus = new EventBus();

        useCase = new SetCurrentTeamMemberUseCase(applicationState, eventBus);
    }

    [Fact]
    public async Task HavingTeamMemberIdSpecifiedInRequest_WhenUseCaseIsExecuted_ThenTeamMemberIdIsSetInApplicationState()
    {
        SetCurrentTeamMemberRequest request = new()
        {
            TeamMemberId = 784
        };
        await useCase.Handle(request, CancellationToken.None);

        applicationState.SelectedTeamMemberId.Should().Be(784);
    }

    [Fact]
    public async Task HavingTeamMemberIdNotSpecifiedInRequest_WhenUseCaseIsExecuted_ThenTeamMemberIdIsSetToNullInApplicationState()
    {
        SetCurrentTeamMemberRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        applicationState.SelectedTeamMemberId.Should().BeNull();
    }

    [Fact]
    public async Task HavingUseCaseInstance_WhenUseCaseIsExecuted_ThenTeamMemberChangedEvent()
    {
        EventBusClient<TeamMemberChangedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<TeamMemberChangedEvent>();

        SetCurrentTeamMemberRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.VerifyEventWasTriggered(1);
    }

    [Fact]
    public async Task HavingUseCaseInstance_WhenUseCaseIsExecuted_ThenTeamMemberChangedEventContainTeamMemberId()
    {
        EventBusClient<TeamMemberChangedEvent> eventBusClient = eventBus.CreateMockSubscriberFor<TeamMemberChangedEvent>();

        SetCurrentTeamMemberRequest request = new()
        {
            TeamMemberId = 784
        };
        await useCase.Handle(request, CancellationToken.None);

        eventBusClient.Event.NewTeamMemberId.Should().Be(784);
    }
}