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
using DustInTheWind.VeloCity.Wpf.Application.PresentTeam;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.Team
{
    public class TeamPageViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private List<TeamMemberViewModel> teamMembers;
        private TeamMemberViewModel selectedTeamMember;

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
            }
        }

        public TeamPageViewModel(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _ = Initialize();
        }

        private async Task Initialize()
        {
            PresentTeamRequest request = new();

            PresentTeamResponse response = await mediator.Send(request);

            TeamMembers = response.TeamMembers
                .Select(x => new TeamMemberViewModel(x))
                .ToList();
        }
    }
}