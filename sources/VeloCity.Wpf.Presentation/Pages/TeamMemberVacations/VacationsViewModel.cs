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
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberVacations;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentTeamMember;
using DustInTheWind.VeloCity.Wpf.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.TeamMemberVacations
{
    public class VacationsViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private List<VacationViewModel> vacations;

        public List<VacationViewModel> Vacations
        {
            get => vacations;
            private set
            {
                vacations = value;
                OnPropertyChanged();
            }
        }

        public VacationsViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<TeamMemberChangedEvent>(HandleSprintChangedEvent);
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await ReloadVacations();
        }

        private async Task HandleSprintChangedEvent(TeamMemberChangedEvent ev, CancellationToken cancellationToken)
        {
            await ReloadVacations();
        }

        private async Task ReloadVacations()
        {
            PresentTeamMemberVacationsRequest request = new();
            PresentTeamMemberVacationsResponse response = await mediator.Send(request);

            Vacations = response.Vacations
                .Select(VacationViewModel.From)
                .ToList();
        }
    }
}