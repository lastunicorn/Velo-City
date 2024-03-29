﻿// VeloCity
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
using DustInTheWind.VeloCity.Wpf.Application.PresentSprints;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintsList;

public class SprintViewModel : ViewModelBase
{
    private SprintState sprintState;
    private readonly string sprintNumber;
    private string subtitle;

    public int SprintId { get; }

    public string SprintNumber
    {
        get => sprintNumber;
        private init
        {
            sprintNumber = value;
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

    public DateInterval SprintDateInterval { get; }

    public SprintState SprintState
    {
        get => sprintState;
        private set
        {
            sprintState = value;
            OnPropertyChanged();
        }
    }

    public SprintViewModel(SprintDto sprintInfo, EventBus eventBus)
    {
        if (sprintInfo == null) throw new ArgumentNullException(nameof(sprintInfo));

        SprintId = sprintInfo.Id;
        Subtitle = string.IsNullOrEmpty(sprintInfo.Title) ? null : sprintInfo.Title;
        SprintNumber = sprintInfo.Number.ToString();
        SprintState = sprintInfo.State.ToPresentationModel();
        SprintDateInterval = sprintInfo.DateInterval;

        eventBus.Subscribe<SprintUpdatedEvent>(HandleSprintUpdatedEvent);
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