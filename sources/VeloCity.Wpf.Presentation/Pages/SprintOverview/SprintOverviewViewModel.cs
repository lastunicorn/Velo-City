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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprint;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintOverview;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintOverview
{
    public class SprintOverviewViewModel:ViewModelBase
    {
        private readonly IMediator mediator;
        private SprintState sprintState;
        private DateInterval timeInterval;
        private int workDays;
        private HoursValue totalWorkHours;
        private StoryPoints estimatedStoryPoints;
        private StoryPoints? estimatedStoryPointsWithVelocityPenalties;
        private Velocity estimatedVelocity;
        private StoryPoints commitmentStoryPoints;
        private StoryPoints actualStoryPoints;
        private Velocity actualVelocity;
        private List<NoteBase> notes;

        public DateInterval TimeInterval
        {
            get => timeInterval;
            private set
            {
                timeInterval = value;
                OnPropertyChanged();
            }
        }

        public SprintState SprintState
        {
            get => sprintState;
            private set
            {
                sprintState = value;
                OnPropertyChanged();
            }
        }

        public int WorkDays
        {
            get => workDays;
            private set
            {
                workDays = value;
                OnPropertyChanged();
            }
        }

        public HoursValue TotalWorkHours
        {
            get => totalWorkHours;
            private set
            {
                totalWorkHours = value;
                OnPropertyChanged();
            }
        }

        public StoryPoints EstimatedStoryPoints
        {
            get => estimatedStoryPoints;
            private set
            {
                estimatedStoryPoints = value;
                OnPropertyChanged();
            }
        }

        public StoryPoints? EstimatedStoryPointsWithVelocityPenalties
        {
            get => estimatedStoryPointsWithVelocityPenalties;
            private set
            {
                estimatedStoryPointsWithVelocityPenalties = value;
                OnPropertyChanged();
            }
        }

        public Velocity EstimatedVelocity
        {
            get => estimatedVelocity;
            private set
            {
                estimatedVelocity = value;
                OnPropertyChanged();
            }
        }

        public StoryPoints CommitmentStoryPoints
        {
            get => commitmentStoryPoints;
            private set
            {
                commitmentStoryPoints = value;
                OnPropertyChanged();
            }
        }

        public StoryPoints ActualStoryPoints
        {
            get => actualStoryPoints;
            private set
            {
                actualStoryPoints = value;
                OnPropertyChanged();
            }
        }

        public Velocity ActualVelocity
        {
            get => actualVelocity;
            private set
            {
                actualVelocity = value;
                OnPropertyChanged();
            }
        }

        public List<NoteBase> Notes
        {
            get => notes;
            private set
            {
                notes = value;
                OnPropertyChanged();
            }
        }

        public SprintOverviewViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<RefreshEvent>(HandleRefreshEvent);
            eventBus.Subscribe<CurrentSprintChangedEvent>(HandleCurrentSprintChangedEvent);
            eventBus.Subscribe<SprintUpdatedEvent>(HandleSprintUpdatedEvent);
        }

        private void DisplayResponse(PresentSprintOverviewResponse response)
        {
            DateTime? startDate = response.SprintDateInterval.StartDate;
            DateTime? endDate = response.SprintDateInterval.EndDate;
            TimeInterval = new DateInterval(startDate, endDate);
            SprintState = response.SprintState;

            WorkDays = response.WorkDaysCount;
            TotalWorkHours = response.TotalWorkHours;

            EstimatedStoryPoints = response.EstimatedStoryPoints;
            EstimatedStoryPointsWithVelocityPenalties = response.EstimatedStoryPointsWithVelocityPenalties.IsNull
                ? (StoryPoints?)null
                : response.EstimatedStoryPointsWithVelocityPenalties;
            EstimatedVelocity = response.EstimatedVelocity;
            CommitmentStoryPoints = response.CommitmentStoryPoints;

            ActualStoryPoints = response.ActualStoryPoints;
            ActualVelocity = response.ActualVelocity;

            Notes = CreateNotes(response).ToList();
        }

        private async Task HandleRefreshEvent(RefreshEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintDetails();
        }

        private async Task HandleCurrentSprintChangedEvent(CurrentSprintChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintDetails();
        }

        private async Task RetrieveSprintDetails()
        {
            PresentSprintOverviewRequest request = new();

            PresentSprintOverviewResponse response = await mediator.Send(request);

            DisplayResponse(response);
        }

        private Task HandleSprintUpdatedEvent(SprintUpdatedEvent ev, CancellationToken cancellationToken)
        {
            SprintState = ev.SprintState;

            return Task.CompletedTask;
        }

        private static IEnumerable<NoteBase> CreateNotes(PresentSprintOverviewResponse response)
        {
            bool previousSprintsExist = response.PreviouslyClosedSprints is { Count: > 0 };

            if (previousSprintsExist)
            {
                yield return new PreviousSprintsCalculationNote
                {
                    PreviousSprintNumbers = response.PreviouslyClosedSprints
                };
            }
            else
            {
                yield return new NoPreviousSprintsNote();
            }

            if (response.ExcludedSprints is { Count: > 0 })
            {
                yield return new ExcludedSprintsNote
                {
                    ExcludesSprintNumbers = response.ExcludedSprints
                };
            }

            if (response.EstimatedStoryPointsWithVelocityPenalties.IsNotNull)
            {
                yield return new VelocityPenaltiesNote
                {
                    VelocityPenalties = response.VelocityPenalties
                };
            }
        }
    }
}