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
using DustInTheWind.VeloCity.Wpf.Application.PresentSprint;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprints;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Presentation.Commands;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintCalendar;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintMembers;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintOverview;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.Sprints
{
    public class SprintsPageViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private List<SprintViewModel> sprints;
        private SprintOverviewViewModel sprintOverview;
        private SprintViewModel selectedSprint;
        private string detailsTitle;
        private SprintCalendarViewModel sprintCalendar;
        private SprintMembersViewModel sprintMembers;

        public List<SprintViewModel> Sprints
        {
            get => sprints;
            private set
            {
                sprints = value;
                OnPropertyChanged();
            }
        }

        public SprintViewModel SelectedSprint
        {
            get => selectedSprint;
            set
            {
                selectedSprint = value;
                OnPropertyChanged();

                SprintOverview = null;
                DetailsTitle = BuildDetailsTitle();

                if (selectedSprint != null)
                    _ = RetrieveSprintDetails(selectedSprint.SprintId);
            }
        }

        public string DetailsTitle
        {
            get => detailsTitle;
            set
            {
                detailsTitle = value;
                OnPropertyChanged();
            }
        }

        public RefreshCommand RefreshCommand { get; }

        public SprintOverviewViewModel SprintOverview
        {
            get => sprintOverview;
            private set
            {
                sprintOverview = value;
                OnPropertyChanged();
            }
        }

        public SprintCalendarViewModel SprintCalendar
        {
            get => sprintCalendar;
            set
            {
                sprintCalendar = value;
                OnPropertyChanged();
            }
        }

        public SprintMembersViewModel SprintMembers
        {
            get => sprintMembers;
            set
            {
                sprintMembers = value;
                OnPropertyChanged();
            }
        }

        public SprintsPageViewModel(IMediator mediator, EventBus eventBus)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            RefreshCommand = new RefreshCommand(mediator);

            eventBus.Subscribe<RefreshEvent>(HandleRefreshEvent);

            _ = Initialize();
        }

        private async Task HandleRefreshEvent(RefreshEvent ev, CancellationToken cancellationToken)
        {
            await Initialize();
        }

        private async Task Initialize()
        {
            PresentSprintsRequest request = new();

            PresentSprintsResponse response = await mediator.Send(request);

            Sprints = response.Sprints
                .Select(x => new SprintViewModel(x))
                .ToList();
        }

        private async Task RetrieveSprintDetails(int sprintId)
        {
            SprintOverview = null;
            SprintCalendar = null;

            PresentSprintRequest request = new()
            {
                SprintNumber = sprintId
            };

            PresentSprintResponse response = await mediator.Send(request);

            SprintOverview = new SprintOverviewViewModel(response);
            SprintCalendar = new SprintCalendarViewModel(response.SprintDays, response.SprintMembers);
            SprintMembers = new SprintMembersViewModel(response);
        }

        private string BuildDetailsTitle()
        {
            return SelectedSprint == null
                ? null
                : $"{SelectedSprint.SprintName} ({selectedSprint.SprintNumber})";
        }
    }
}