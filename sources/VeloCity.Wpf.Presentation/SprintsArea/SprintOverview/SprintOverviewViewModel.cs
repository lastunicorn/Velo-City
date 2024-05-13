// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintOverview;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintOverview;

public class SprintOverviewViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;
    private string sprintGoal;
    private StoryPoints estimatedStoryPoints;
    private float? estimatedStoryPointsValue;
    private IEnumerable<string> estimatedStoryPointsInfo;
    private StoryPoints estimatedStoryPointsWithVelocityPenalties;
    private IEnumerable<string> estimatedStoryPointsWithVelocityPenaltiesInfo;
    private bool estimatedStoryPointsWithVelocityPenaltiesVisible;
    private Velocity estimatedVelocity;
    private IEnumerable<string> estimatedVelocityInfo;
    private StoryPoints commitmentStoryPoints;
    private float? commitmentStoryPointsValue;
    private StoryPoints actualStoryPoints;
    private float? actualStoryPointsValue;
    private Velocity actualVelocity;
    private string sprintComments;
    private List<NoteBase> notes;
    private int lookBackSprintCount;

    public string SprintGoal
    {
        get => sprintGoal;
        set
        {
            sprintGoal = value;
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

            EstimatedStoryPointsValue = estimatedStoryPoints;
        }
    }

    public float? EstimatedStoryPointsValue
    {
        get => estimatedStoryPointsValue;
        private set
        {
            estimatedStoryPointsValue = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<string> EstimatedStoryPointsInfo
    {
        get => estimatedStoryPointsInfo;
        private set
        {
            estimatedStoryPointsInfo = value;
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

    public IEnumerable<string> EstimatedStoryPointsWithVelocityPenaltiesInfo
    {
        get => estimatedStoryPointsWithVelocityPenaltiesInfo;
        private set
        {
            estimatedStoryPointsWithVelocityPenaltiesInfo = value;
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

    public IEnumerable<string> EstimatedVelocityInfo
    {
        get => estimatedVelocityInfo;
        private set
        {
            estimatedVelocityInfo = value;
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

            CommitmentStoryPointsValue = commitmentStoryPoints;
        }
    }

    public float? CommitmentStoryPointsValue
    {
        get => commitmentStoryPointsValue;
        private set
        {
            commitmentStoryPointsValue = value;
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

            ActualStoryPointsValue = value;
        }
    }

    public float? ActualStoryPointsValue
    {
        get => actualStoryPointsValue;
        private set
        {
            actualStoryPointsValue = value;
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

    public GeneralInfoViewModel GeneralInfoViewModel { get; }

    public SprintOverviewViewModel(IRequestBus requestBus, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        GeneralInfoViewModel = new GeneralInfoViewModel(requestBus, eventBus);

        eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
        eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
        eventBus.Subscribe<SprintUpdatedEvent>(HandleSprintUpdatedEvent);
        eventBus.Subscribe<TeamMemberVacationChangedEvent>(HandleTeamMemberVacationChangedEvent);
        eventBus.Subscribe<SprintsListChangedEvent>(HandleSprintsListChangedEvent);
    }

    private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
    {
        await RetrieveSprintOverview();
    }

    private async Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
    {
        await RetrieveSprintOverview();
    }

    private async Task HandleTeamMemberVacationChangedEvent(TeamMemberVacationChangedEvent ev, CancellationToken cancellationToken)
    {
        await RetrieveSprintOverview();
    }

    private async Task HandleSprintsListChangedEvent(SprintsListChangedEvent ev, CancellationToken cancellationToken)
    {
        await RetrieveSprintOverview();
    }

    private async Task RetrieveSprintOverview()
    {
        PresentSprintOverviewRequest request = new();

        PresentSprintOverviewResponse response = await requestBus.Send<PresentSprintOverviewRequest, PresentSprintOverviewResponse>(request);

        DisplayResponse(response);
    }

    private void DisplayResponse(PresentSprintOverviewResponse response)
    {
        SprintGoal = response.SprintGoal;

        EstimatedStoryPoints = response.EstimatedStoryPoints;
        EstimatedStoryPointsInfo = new EstimatedStoryPointsInfo
        {
            PreviousSprintNumbers = response.PreviouslyClosedSprintNumbers
        };
        EstimatedStoryPointsWithVelocityPenalties = response.EstimatedStoryPointsWithVelocityPenalties;
        EstimatedStoryPointsWithVelocityPenaltiesInfo = new EstimatedStoryPointsWithVelocityPenaltiesInfo
        {
            VelocityPenalties = response.VelocityPenalties
        };
        EstimatedStoryPointsWithVelocityPenaltiesVisible = !response.EstimatedStoryPointsWithVelocityPenalties.IsEmpty;
        EstimatedVelocity = response.EstimatedVelocity;
        EstimatedVelocityInfo = new EstimatedVelocityInfo
        {
            PreviousSprintNumbers = response.PreviouslyClosedSprintNumbers
        };
        CommitmentStoryPoints = response.SprintState == Domain.SprintModel.SprintState.New && response.CommitmentStoryPoints.IsZero
            ? StoryPoints.Empty
            : response.CommitmentStoryPoints;

        ActualStoryPoints = response.SprintState != Domain.SprintModel.SprintState.Closed && response.ActualStoryPoints.IsZero
            ? StoryPoints.Empty
            : response.ActualStoryPoints;
        ActualVelocity = response.SprintState != Domain.SprintModel.SprintState.Closed && response.ActualVelocity.IsZero
            ? Velocity.Empty
            : response.ActualVelocity;

        SprintComments = response.SprintComments;

        LookBackSprintCount = response.PreviouslyClosedSprintNumbers?.Count ?? 0;

        Notes = CreateNotes(response).ToList();
    }

    private static IEnumerable<NoteBase> CreateNotes(PresentSprintOverviewResponse response)
    {
        bool previousSprintsExist = response.PreviouslyClosedSprintNumbers is { Count: > 0 };

        if (previousSprintsExist)
        {
            yield return new PreviousSprintsCalculationNote
            {
                PreviousSprintNumbers = response.PreviouslyClosedSprintNumbers
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
    }

    private Task HandleSprintUpdatedEvent(SprintUpdatedEvent ev, CancellationToken cancellationToken)
    {
        SprintGoal = ev.SprintGoal;

        CommitmentStoryPoints = ev.CommitmentStoryPoints;

        ActualStoryPoints = ev.SprintState != Domain.SprintModel.SprintState.Closed && ev.ActualStoryPoints.IsZero
            ? StoryPoints.Empty
            : ev.ActualStoryPoints;
        ActualVelocity = ev.SprintState != Domain.SprintModel.SprintState.Closed && ev.ActualVelocity.IsZero
            ? Velocity.Empty
            : ev.ActualVelocity;

        SprintComments = ev.Comments;

        return Task.CompletedTask;
    }
}