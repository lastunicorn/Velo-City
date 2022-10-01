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
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintDetails;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Presentation.Commands;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintCalendar;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMembers;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintOverview;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintsList;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.Sprints
{
    public class SprintsPageViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private string title;
        private bool isSprintSelected;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public bool IsSprintSelected
        {
            get => isSprintSelected;
            set
            {
                isSprintSelected = value;
                OnPropertyChanged();
            }
        }

        public SprintOverviewViewModel SprintOverviewViewModel { get; }

        public SprintCalendarViewModel SprintCalendarViewModel { get; }

        public SprintMembersViewModel SprintMembersViewModel { get; }

        public StartSprintCommand StartSprintCommand { get; }

        public CloseSprintCommand CloseSprintCommand { get; }

        public SprintsListViewModel SprintsListViewModel { get; }

        public SprintsPageViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            SprintsListViewModel = new SprintsListViewModel(mediator, eventBus);
            SprintOverviewViewModel = new SprintOverviewViewModel(mediator, eventBus);
            SprintCalendarViewModel = new SprintCalendarViewModel(mediator, eventBus);
            SprintMembersViewModel = new SprintMembersViewModel(mediator, eventBus);

            StartSprintCommand = new StartSprintCommand(mediator, eventBus);
            CloseSprintCommand = new CloseSprintCommand(mediator, eventBus);

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintDetails();
        }

        private async Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintDetails();
        }

        private async Task RetrieveSprintDetails()
        {
            PresentSprintDetailRequest request = new();
            PresentSprintDetailResponse response = await mediator.Send(request);

            Title = BuildTitle(response);
            IsSprintSelected = true;
        }

        private static string BuildTitle(PresentSprintDetailResponse response)
        {
            return response == null
                ? null
                : string.IsNullOrEmpty(response.SprintName)
                    ? $"Sprint {response.SprintNumber}"
                    : $"Sprint {response.SprintNumber} - {response.SprintName}";
        }
    }
}