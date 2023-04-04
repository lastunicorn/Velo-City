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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprints;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintsList;

public class SprintViewModel : ViewModelBase
{
    private SprintState sprintState;
    private readonly string title;
    private string subtitle;
    private string sprintDateInterval;

    public int SprintId { get; }

    public string Title
    {
        get => title;
        private init
        {
            title = value;
            OnPropertyChanged();
        }
    }

    public string Subtitle
    {
        get => subtitle;
        private set
        {
            subtitle = value;
            OnPropertyChanged();
        }
    }

    public string SprintDateInterval
    {
        get => sprintDateInterval;
        set
        {
            if (value == sprintDateInterval) return;
            sprintDateInterval = value;
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

    public SprintViewModel(SprintInfo sprintInfo, EventBus eventBus)
    {
        if (sprintInfo == null) throw new ArgumentNullException(nameof(sprintInfo));

        SprintId = sprintInfo.Id;
        subtitle = sprintInfo.Title;
        Title = BuildTitle(sprintInfo);
        SprintState = sprintInfo.State.ToPresentationModel();
        SprintDateInterval = BuildDateInterval(sprintInfo);

        eventBus.Subscribe<SprintUpdatedEvent>(HandleSprintUpdatedEvent);
    }

    private static string BuildTitle(SprintInfo sprintInfo)
    {
        int sprintNumber = sprintInfo.Number;
        return $"Sprint {sprintNumber}";
    }

    private static string BuildDateInterval(SprintInfo sprintInfo)
    {
        DateTime? startDate = sprintInfo.DateInterval.StartDate;
        DateTime? endDate = sprintInfo.DateInterval.EndDate;

        return $"[{startDate:dd MMM} - {endDate:dd MMM}]";

    }

    private Task HandleSprintUpdatedEvent(SprintUpdatedEvent ev, CancellationToken cancellationToken)
    {
        if (ev.SprintId == SprintId)
        {
            SprintState = ev.SprintState.ToPresentationModel();
            Subtitle = ev.SprintTitle;
        }

        return Task.CompletedTask;
    }

    public override string ToString()
    {
        return $"{Subtitle} [{SprintDateInterval}]";
    }
}