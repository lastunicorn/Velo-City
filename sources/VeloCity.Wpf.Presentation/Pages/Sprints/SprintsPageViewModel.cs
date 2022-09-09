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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintDetails;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Presentation.Commands;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintCalendar;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintMembers;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintOverview;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintsList;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.Sprints
{
    public class SprintsPageViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private string detailsTitle;
        private bool isSprintSelected;

        public string DetailsTitle
        {
            get => detailsTitle;
            set
            {
                detailsTitle = value;
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

            eventBus.Subscribe<RefreshEvent>(HandleRefreshEvent);
            eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
        }

        private async Task HandleRefreshEvent(RefreshEvent ev, CancellationToken cancellationToken)
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

            DetailsTitle = BuildDetailsTitle(response);
            IsSprintSelected = true;
        }

        private static string BuildDetailsTitle(PresentSprintDetailResponse response)
        {
            return response == null
                ? null
                : string.IsNullOrEmpty(response.SprintName)
                    ? $"Sprint {response.SprintNumber}"
                    : $"Sprint {response.SprintNumber} - {response.SprintName}";
        }
    }
}