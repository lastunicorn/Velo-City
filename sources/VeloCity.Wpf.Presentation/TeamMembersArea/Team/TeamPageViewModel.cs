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
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberDetails;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentTeamMember;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;
using DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.TeamMemberEmployments;
using DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.TeamMembersList;
using DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.TeamMemberVacations;

namespace DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.Team
{
    public class TeamPageViewModel : ViewModelBase
    {
        private readonly IRequestBus requestBus;
        private string title;
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

        public VacationsViewModel VacationsViewModel { get; }

        public TeamMembersListViewModel TeamMembersListViewModel { get; }

        public TeamPageViewModel(IRequestBus requestBus, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

            TeamMembersListViewModel = new TeamMembersListViewModel(requestBus, eventBus);
            EmploymentsViewModel = new EmploymentsViewModel(requestBus, eventBus);
            VacationsViewModel = new VacationsViewModel(requestBus, eventBus);

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<TeamMemberChangedEvent>(HandleTeamMemberChangedEvent);

            _ = RetrieveTeamMemberDetails();
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveTeamMemberDetails();
        }

        private async Task HandleTeamMemberChangedEvent(TeamMemberChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveTeamMemberDetails();
        }

        private async Task RetrieveTeamMemberDetails()
        {
            PresentTeamMemberDetailsRequest request = new();

            PresentTeamMemberDetailsResponse response = await requestBus.Send<PresentTeamMemberDetailsRequest, PresentTeamMemberDetailsResponse>(request);

            Title = response.TeamMemberName;
            IsTeamMemberSelected = response.TeamMemberName != null;
        }
    }
}