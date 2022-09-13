// Velo City
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMembers;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentTeamMember;
using DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.Team;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.TeamMembersList
{
    public class TeamMembersListViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private List<TeamMemberViewModel> teamMembers;
        private TeamMemberViewModel selectedTeamMember;
        private bool hasTeamMembers;

        public List<TeamMemberViewModel> TeamMembers
        {
            get => teamMembers;
            private set
            {
                teamMembers = value;
                OnPropertyChanged();
            }
        }

        public TeamMemberViewModel SelectedTeamMember
        {
            get => selectedTeamMember;
            set
            {
                if (ReferenceEquals(selectedTeamMember, value))
                    return;

                selectedTeamMember = value;
                OnPropertyChanged();

                if (!IsInitializeMode)
                    _ = SetCurrentTeamMember(selectedTeamMember?.TeamMemberId);
            }
        }

        public bool HasTeamMembers
        {
            get => hasTeamMembers;
            set
            {
                hasTeamMembers = value;
                OnPropertyChanged();
            }
        }

        public TeamMembersListViewModel(IMediator mediator, EventBus eventBus)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<TeamMemberChangedEvent>(HandleSprintChangedEvent);
            //eventBus.Subscribe<TeamMemberUpdatedEvent>(HandleSprintUpdatedEvent);

            _ = Initialize();
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await Initialize();
        }

        private Task HandleSprintChangedEvent(TeamMemberChangedEvent ev, CancellationToken cancellationToken)
        {
            SelectedTeamMember = teamMembers.FirstOrDefault(x => x.TeamMemberId == ev.NewTeamMemberId);

            return Task.CompletedTask;
        }

        //private Task HandleSprintUpdatedEvent(TeamMemberUpdatedEvent ev, CancellationToken cancellationToken)
        //{
        //    TeamMemberViewModel teamMemberViewModel = teamMembers.FirstOrDefault(x => x.TeamMemberId == ev.SprintId);

        //    //if (teamMemberViewModel != null)
        //    //    teamMemberViewModel.SprintState = ev.SprintState.ToPresentationModel();

        //    return Task.CompletedTask;
        //}

        private async Task Initialize()
        {
            PresentTeamMembersRequest request = new();
            PresentTeamMembersResponse response = await mediator.Send(request);

            RunInInitializeMode(() =>
            {
                TeamMembers = response.TeamMembers
                    .Select(x => new TeamMemberViewModel(x))
                    .ToList();

                SelectedTeamMember = response.CurrentTeamMemberId == null
                    ? null
                    : TeamMembers.FirstOrDefault(x => x.TeamMemberId == response.CurrentTeamMemberId.Value);

                HasTeamMembers = TeamMembers?.Count > 0;
            });
        }

        private async Task SetCurrentTeamMember(int? teamMemberId)
        {
            SetCurrentTeamMemberRequest request = new()
            {
                TeamMemberId = teamMemberId
            };

            await mediator.Send(request);
        }
    }
}