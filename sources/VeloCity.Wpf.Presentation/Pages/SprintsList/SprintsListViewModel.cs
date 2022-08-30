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
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintCalendar;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintMembers;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintOverview;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.Sprints;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintsList
{
    public class SprintsListViewModel : ViewModelBase
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
                if (ReferenceEquals(selectedSprint, value))
                    return;

                selectedSprint = value;
                OnPropertyChanged();

                _ = SetCurrentSprint(selectedSprint?.SprintId);
            }
        }

        public SprintsListViewModel(IMediator mediator, EventBus eventBus)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<RefreshEvent>(HandleRefreshEvent);
            eventBus.Subscribe<CurrentSprintChangedEvent>(HandleCurrentSprintChangedEvent);

            _ = Initialize();
        }

        private async Task HandleRefreshEvent(RefreshEvent ev, CancellationToken cancellationToken)
        {
            await Initialize();
        }

        private Task HandleCurrentSprintChangedEvent(CurrentSprintChangedEvent ev, CancellationToken cancellationToken)
        {
            SelectedSprint = sprints.FirstOrDefault(x => x.SprintId == ev.SprintId);

            return Task.CompletedTask;
        }

        private async Task Initialize()
        {
            int? initialSelectedSprintNumber = selectedSprint?.SprintNumber;

            PresentSprintsRequest request = new();

            PresentSprintsResponse response = await mediator.Send(request);

            Sprints = response.Sprints
                .Select(x => new SprintViewModel(x))
                .ToList();

            if (initialSelectedSprintNumber != null)
                SelectedSprint = Sprints.FirstOrDefault(x => x.SprintNumber == initialSelectedSprintNumber);
        }

        private async Task SetCurrentSprint(int? sprintId)
        {
            SetCurrentSprintRequest request = new()
            {
                SprintId = sprintId
            };

            await mediator.Send(request);
        }
    }
}