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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprints;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Presentation.Commands;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintsList
{
    public class SprintsListViewModel : ViewModelBase
    {
        private readonly IRequestBus requestBus;
        private readonly EventBus eventBus;
        private List<SprintViewModel> sprints;
        private SprintViewModel selectedSprint;
        private bool hasSprints;

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

                if (!IsInitializeMode)
                    _ = SetCurrentSprint(selectedSprint?.SprintId);
            }
        }

        public bool HasSprints
        {
            get => hasSprints;
            private set
            {
                hasSprints = value;
                OnPropertyChanged();
            }
        }

        public NewSprintCommand NewSprintCommand { get; }

        public SprintsListViewModel(IRequestBus requestBus, EventBus eventBus)
        {
            this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            
            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
            eventBus.Subscribe<SprintsListChangedEvent>(HandleSprintsListChangedEvent);

            NewSprintCommand = new NewSprintCommand(requestBus);

            _ = Initialize();
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await Initialize();
        }

        private Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
        {
            SelectedSprint = sprints.FirstOrDefault(x => x.SprintId == ev.NewSprintId);

            return Task.CompletedTask;
        }

        private async Task HandleSprintsListChangedEvent(SprintsListChangedEvent ev, CancellationToken cancellationToken)
        {
            await Initialize();

            SelectedSprint = sprints.FirstOrDefault(x => x.SprintId == ev.NewSprintId);
        }

        private async Task Initialize()
        {
            PresentSprintsRequest request = new();
            PresentSprintsResponse response = await requestBus.Send<PresentSprintsRequest, PresentSprintsResponse>(request);

            RunInInitializeMode(() =>
            {
                Sprints = response.Sprints
                    .Select(x => new SprintViewModel(x, eventBus))
                    .ToList();

                SelectedSprint = response.CurrentSprintId == null
                    ? null
                    : Sprints.FirstOrDefault(x => x.SprintId == response.CurrentSprintId.Value);

                HasSprints = Sprints?.Count > 0;
            });
        }

        private async Task SetCurrentSprint(int? sprintId)
        {
            SetCurrentSprintRequest request = new()
            {
                SprintId = sprintId
            };

            await requestBus.Send(request);
        }
    }
}