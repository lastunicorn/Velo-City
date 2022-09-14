﻿// Velo City
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
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintOverview;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using MediatR;
using SprintState = DustInTheWind.VeloCity.Wpf.Presentation.CustomControls.SprintState;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintOverview
{
    public class SprintOverviewViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private DateInterval timeInterval;
        private SprintState sprintState;
        private string sprintGoal;
        private int workDays;
        private HoursValue totalWorkHours;
        private StoryPoints estimatedStoryPoints;
        private StoryPoints estimatedStoryPointsWithVelocityPenalties;
        private bool estimatedStoryPointsWithVelocityPenaltiesVisible;
        private Velocity estimatedVelocity;
        private StoryPoints commitmentStoryPoints;
        private StoryPoints actualStoryPoints;
        private Velocity actualVelocity;
        private string sprintComments;
        private List<NoteBase> notes;
        private int lookBackSprintCount;

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

        public string SprintGoal
        {
            get => sprintGoal;
            set
            {
                sprintGoal = value;
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

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties
        {
            get => estimatedStoryPointsWithVelocityPenalties;
            private set
            {
                estimatedStoryPointsWithVelocityPenalties = value;
                OnPropertyChanged();
            }
        }

        public bool EstimatedStoryPointsWithVelocityPenaltiesVisible
        {
            get => estimatedStoryPointsWithVelocityPenaltiesVisible;
            private set
            {
                estimatedStoryPointsWithVelocityPenaltiesVisible = value;
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

        public string SprintComments
        {
            get => sprintComments;
            set
            {
                sprintComments = value;
                OnPropertyChanged();
            }
        }

        public int LookBackSprintCount
        {
            get => lookBackSprintCount;
            set
            {
                lookBackSprintCount = value;
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

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
            eventBus.Subscribe<SprintUpdatedEvent>(HandleSprintUpdatedEvent);
        }

        private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintOverview();
        }

        private async Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
        {
            await RetrieveSprintOverview();
        }

        private async Task RetrieveSprintOverview()
        {
            PresentSprintOverviewRequest request = new();

            PresentSprintOverviewResponse response = await mediator.Send(request);

            DisplayResponse(response);
        }

        private void DisplayResponse(PresentSprintOverviewResponse response)
        {
            DateTime? startDate = response.SprintDateInterval.StartDate;
            DateTime? endDate = response.SprintDateInterval.EndDate;
            TimeInterval = new DateInterval(startDate, endDate);
            SprintState = response.SprintState.ToPresentationModel();
            SprintGoal = response.SprintGoal;

            WorkDays = response.WorkDaysCount;
            TotalWorkHours = response.TotalWorkHours;

            EstimatedStoryPoints = response.EstimatedStoryPoints;
            EstimatedStoryPointsWithVelocityPenalties = response.EstimatedStoryPointsWithVelocityPenalties;
            EstimatedStoryPointsWithVelocityPenaltiesVisible = !response.EstimatedStoryPointsWithVelocityPenalties.IsEmpty;
            EstimatedVelocity = response.EstimatedVelocity;
            CommitmentStoryPoints = response.SprintState == Domain.SprintState.New && response.CommitmentStoryPoints.IsZero
                ? StoryPoints.Empty
                : response.CommitmentStoryPoints;

            ActualStoryPoints = response.SprintState != Domain.SprintState.Closed && response.ActualStoryPoints.IsZero
                ? StoryPoints.Empty
                : response.ActualStoryPoints;
            ActualVelocity = response.SprintState != Domain.SprintState.Closed && response.ActualVelocity.IsZero
                ? Velocity.Empty
                : response.ActualVelocity;

            SprintComments = response.SprintComments;

            LookBackSprintCount = response.PreviouslyClosedSprints?.Count ?? 0;

            Notes = CreateNotes(response).ToList();
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

            if (response.EstimatedStoryPointsWithVelocityPenalties.IsNotEmpty)
            {
                yield return new VelocityPenaltiesNote
                {
                    VelocityPenalties = response.VelocityPenalties
                };
            }
        }

        private Task HandleSprintUpdatedEvent(SprintUpdatedEvent ev, CancellationToken cancellationToken)
        {
            SprintState = ev.SprintState.ToPresentationModel();
            SprintGoal = ev.SprintGoal;

            CommitmentStoryPoints = ev.CommitmentStoryPoints;

            ActualStoryPoints = ev.SprintState != Domain.SprintState.Closed && ev.ActualStoryPoints.IsZero
                ? StoryPoints.Empty
                : ev.ActualStoryPoints;
            ActualVelocity = ev.SprintState != Domain.SprintState.Closed && ev.ActualVelocity.IsZero
                ? Velocity.Empty
                : ev.ActualVelocity;

            SprintComments = ev.Comments;

            return Task.CompletedTask;
        }
    }
}