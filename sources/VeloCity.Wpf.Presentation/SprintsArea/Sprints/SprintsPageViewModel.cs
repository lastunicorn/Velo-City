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

using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintDetails;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using DustInTheWind.VeloCity.Wpf.Presentation.Commands;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintCalendar;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMembers;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintOverview;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintsList;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.Sprints;

public class SprintsPageViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;
    private bool isContentDisplayed;
    private int displayedSprintId;
    private string title;
    private string subtitle;
    private SprintState sprintState;

    public bool IsContentDisplayed
    {
        get => isContentDisplayed;
        set
        {
            isContentDisplayed = value;
            OnPropertyChanged();
        }
    }

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

    public SprintState SprintState
    {
        get => sprintState;
        set
        {
            sprintState = value;
            OnPropertyChanged();
        }
    }

    public SprintOverviewViewModel SprintOverviewViewModel { get; }

    public SprintCalendarViewModel SprintCalendarViewModel { get; }

    public SprintMembersViewModel SprintMembersViewModel { get; }

    public StartSprintCommand StartSprintCommand { get; }

    public CloseSprintCommand CloseSprintCommand { get; }

    public SprintsListViewModel SprintsListViewModel { get; }

    public SprintsPageViewModel(IRequestBus requestBus, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        SprintsListViewModel = new SprintsListViewModel(requestBus, eventBus);
        SprintOverviewViewModel = new SprintOverviewViewModel(requestBus, eventBus);
        SprintCalendarViewModel = new SprintCalendarViewModel(requestBus, eventBus);
        SprintMembersViewModel = new SprintMembersViewModel(requestBus, eventBus);

        StartSprintCommand = new StartSprintCommand(requestBus, eventBus);
        CloseSprintCommand = new CloseSprintCommand(requestBus, eventBus);

        eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
        eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
        eventBus.Subscribe<SprintUpdatedEvent>(HandleSprintUpdatedEvent);
        eventBus.Subscribe<SprintsListChangedEvent>(HandleSprintsListChangedEvent);
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
            SprintState = ev.SprintState.ToPresentationModel();
        }

        return Task.CompletedTask;
    }

    private async Task HandleSprintsListChangedEvent(SprintsListChangedEvent ev, CancellationToken cancellationToken)
    {
        await RetrieveSprintDetails();
    }

    private async Task RetrieveSprintDetails()
    {
        PresentSprintDetailRequest request = new();
        PresentSprintDetailResponse response = await requestBus.Send<PresentSprintDetailRequest, PresentSprintDetailResponse>(request);

        displayedSprintId = response.SprintId;

        IsContentDisplayed = true;
        Title = $"Sprint {response.SprintNumber}";
        Subtitle = response.SprintTitle;
        SprintState = response.SprintState.ToPresentationModel();
    }
}