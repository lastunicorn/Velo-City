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
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeam;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.TeamMemberEmployments;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.Team
{
    public class TeamPageViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private string title;
        private List<TeamMemberViewModel> teamMembers;
        private TeamMemberViewModel selectedTeamMember;
        private bool hasTeamMembers;
        private bool isTeamMemberSelected;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

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
                selectedTeamMember = value;
                OnPropertyChanged();

                UpdateTitle();
                IsTeamMemberSelected = value != null;
                EmploymentsViewModel.DisplayTeamMember(value?.TeamMemberInfo?.Id);
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

        public bool IsTeamMemberSelected
        {
            get => isTeamMemberSelected;
            set
            {
                isTeamMemberSelected = value;
                OnPropertyChanged();
            }
        }

        public EmploymentsViewModel EmploymentsViewModel { get; }

        public TeamPageViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            EmploymentsViewModel = new EmploymentsViewModel(mediator, eventBus);

            _ = Initialize();
        }

        private async Task Initialize()
        {
            PresentTeamRequest request = new();

            PresentTeamResponse response = await mediator.Send(request);

            TeamMembers = response.TeamMembers
                .Select(x => new TeamMemberViewModel(x))
                .ToList();

            HasTeamMembers = TeamMembers?.Count > 0;
        }

        private void UpdateTitle()
        {
            Title = SelectedTeamMember?.TeamMemberInfo?.Name;
        }
    }
}