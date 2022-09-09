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
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberEmployments;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.TeamMemberEmployments
{
    public class EmploymentsViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private int? teamMemberId;
        private List<EmploymentViewModel> employments;

        public List<EmploymentViewModel> Employments
        {
            get => employments;
            private set
            {
                employments = value;
                OnPropertyChanged();
            }
        }

        public EmploymentsViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await ReloadTeamMemberEmployments();
        }

        private async Task ReloadTeamMemberEmployments()
        {
            if (teamMemberId == null)
            {
                Employments = null;
            }
            else
            {
                PresentTeamMemberEmploymentsRequest request = new()
                {
                    TeamMemberId = teamMemberId.Value
                };
                PresentTeamMemberEmploymentsResponse response = await mediator.Send(request);

                Employments = response.Employments
                    .Select(x => new EmploymentViewModel(x))
                    .ToList();
            }
        }

        public void DisplayTeamMember(int? teamMemberId)
        {
            this.teamMemberId = teamMemberId;
            _ = ReloadTeamMemberEmployments();
        }
    }
}