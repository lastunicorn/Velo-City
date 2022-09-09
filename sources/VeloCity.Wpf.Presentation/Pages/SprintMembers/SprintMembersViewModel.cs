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
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintMembers;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintMembers
{
    public class SprintMembersViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private List<SprintMemberOverviewViewModel> sprintMembersOverview;

        public List<SprintMemberOverviewViewModel> SprintMembersOverview
        {
            get => sprintMembersOverview;
            set
            {
                sprintMembersOverview = value;
                OnPropertyChanged();
            }
        }

        public SprintMembersViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintMembers();
        }

        private async Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintMembers();
        }

        private async Task RetrieveSprintMembers()
        {
            PresentSprintMembersRequest request = new();

            PresentSprintMembersResponse response = await mediator.Send(request);

            DisplayResponse(response);
        }

        private void DisplayResponse(PresentSprintMembersResponse response)
        {
            List<SprintMemberOverviewViewModel> sprintMembersOverview = CreateSprintMemberOverviewItems(response.SprintMembers);
            CreateChartBars(sprintMembersOverview);

            SprintMembersOverview = sprintMembersOverview;
        }

        private static List<SprintMemberOverviewViewModel> CreateSprintMemberOverviewItems(IEnumerable<SprintMember> sprintMembers)
        {
            return sprintMembers
                .Select(x => new SprintMemberOverviewViewModel(x))
                .ToList();
        }

        private static void CreateChartBars(IEnumerable<SprintMemberOverviewViewModel> sprintMembersOverview)
        {
            Chart chart = new()
            {
                ActualSize = 100
            };

            IEnumerable<ChartBar> chartBars = sprintMembersOverview
                .Select(x =>
                {
                    ChartBar chartBar = new()
                    {
                        MaxValue = x.WorkHours + x.AbsenceHours,
                        FillValue = x.WorkHours
                    };

                    x.ChartBar = chartBar;

                    return chartBar;
                });

            chart.AddRange(chartBars);
            chart.Calculate();
        }
    }
}