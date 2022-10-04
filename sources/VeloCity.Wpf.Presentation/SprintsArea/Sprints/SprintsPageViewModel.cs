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
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
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
        private bool isContentDisplayed;
        private int displayedSprintId;
        private string subtitle;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public string Subtitle
        {
            get => subtitle;
            set
            {
                subtitle = value;
                OnPropertyChanged();
            }
        }

        public bool IsContentDisplayed
        {
            get => isContentDisplayed;
            set
            {
                isContentDisplayed = value;
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
            eventBus.Subscribe<SprintUpdatedEvent>(HandleSprintUpdatedEvent);
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintDetails();
        }

        private async Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintDetails();
        }

        private Task HandleSprintUpdatedEvent(SprintUpdatedEvent ev, CancellationToken cancellationToken)
        {
            if (displayedSprintId == ev.SprintId)
            {
                Title = $"Sprint {ev.SprintNumber}";
                Subtitle = ev.SprintTitle;
            }

            return Task.CompletedTask;
        }

        private async Task RetrieveSprintDetails()
        {
            PresentSprintDetailRequest request = new();
            PresentSprintDetailResponse response = await mediator.Send(request);

            displayedSprintId = response.SprintId;

            Title = $"Sprint {response.SprintNumber}";
            Subtitle = response.SprintTitle;
            IsContentDisplayed = true;
        }
    }
}