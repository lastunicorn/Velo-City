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

public class GeneralInfoViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;

    private DateTime? startTime;
    private DateTime? endTime;
    private SprintState sprintState;
    private int workDays;
    private HoursValue totalWorkHours;

    public DateTime? StartTime
    {
        get => startTime;
        private set
        {
            if (value.Equals(startTime)) return;
            startTime = value;
            OnPropertyChanged();
        }
    }

    public DateTime? EndTime
    {
        get => endTime;
        private set
        {
            if (value.Equals(endTime)) return;
            endTime = value;
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

    public GeneralInfoViewModel(IRequestBus requestBus, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
        eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
        eventBus.Subscribe<SprintUpdatedEvent>(HandleSprintUpdatedEvent);
        eventBus.Subscribe<TeamMemberVacationChangedEvent>(HandleTeamMemberVacationChangedEvent);
        eventBus.Subscribe<SprintsListChangedEvent>(HandleSprintsListChangedEvent);

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
        StartTime = response.SprintDateInterval.StartDate;
        EndTime = response.SprintDateInterval.EndDate;

        SprintState = response.SprintState.ToPresentationModel();

        WorkDays = response.WorkDaysCount;
        TotalWorkHours = response.TotalWorkHours;
    }

    private Task HandleSprintUpdatedEvent(SprintUpdatedEvent ev, CancellationToken cancellationToken)
    {
        SprintState = ev.SprintState.ToPresentationModel();

        return Task.CompletedTask;
    }
}