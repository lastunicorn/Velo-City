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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintMembers;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMembers
{
    public class SprintMembersViewModel : ViewModelBase
    {
        private readonly IRequestBus requestBus;
        private readonly EventBus eventBus;
        private List<SprintMemberViewModel> sprintMemberViewModels;

        public List<SprintMemberViewModel> SprintMemberViewModels
        {
            get => sprintMemberViewModels;
            private set
            {
                sprintMemberViewModels = value;
                OnPropertyChanged();
            }
        }

        public SprintMembersViewModel(IRequestBus requestBus, EventBus eventBus)
        {
            this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
            eventBus.Subscribe<TeamMemberVacationChangedEvent>(HandleTeamMemberVacationChangedEvent);
            eventBus.Subscribe<SprintsListChangedEvent>(HandleSprintsListChangedEvent);
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintMembers();
        }

        private async Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintMembers();
        }

        private async Task HandleTeamMemberVacationChangedEvent(TeamMemberVacationChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintMembers();
        }

        private async Task HandleSprintsListChangedEvent(SprintsListChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintMembers();
        }

        private async Task RetrieveSprintMembers()
        {
            PresentSprintMembersRequest request = new();

            PresentSprintMembersResponse response = await requestBus.Send<PresentSprintMembersRequest, PresentSprintMembersResponse>(request);

            DisplayResponse(response);
        }

        private void DisplayResponse(PresentSprintMembersResponse response)
        {
            List<SprintMemberViewModel> sprintMemberViewModels = CreateViewModels(response.SprintMembers);
            CreateChartBars(sprintMemberViewModels);

            SprintMemberViewModels = sprintMemberViewModels;
        }

        private List<SprintMemberViewModel> CreateViewModels(IEnumerable<SprintMember> sprintMembers)
        {
            return sprintMembers
                .Select(x => new SprintMemberViewModel(requestBus, eventBus, x))
                .ToList();
        }

        private static void CreateChartBars(IEnumerable<SprintMemberViewModel> sprintMemberViewModels)
        {
            SprintMembersWorkChart chart = new(sprintMemberViewModels);

            foreach (ChartBarValue<SprintMemberViewModel> chartBarValue in chart)
            {
                if (chartBarValue.Item != null)
                    chartBarValue.Item.ChartBarValue = chartBarValue;
            }
        }
    }
}