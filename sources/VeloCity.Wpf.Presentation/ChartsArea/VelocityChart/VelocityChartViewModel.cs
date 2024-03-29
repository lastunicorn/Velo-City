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
using DustInTheWind.VeloCity.Wpf.Application.PresentCommitment;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;
using LiveCharts;
using LiveCharts.Wpf;

namespace DustInTheWind.VeloCity.Wpf.Presentation.ChartsArea.VelocityChart;

internal class VelocityChartViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;
    private ChartValues<float> actualValues;
    private uint sprintCount;
    private List<string> sprintsLabels;

    public uint SprintCount
    {
        get => sprintCount;
        set
        {
            if (value == sprintCount)
                return;

            sprintCount = value;
            OnPropertyChanged();

            if (!IsInitializeMode)
                _ = Initialize();
        }
    }

    public SeriesCollection SeriesCollection { get; private set; }

    public ChartValues<float> ActualValues
    {
        get => actualValues;
        private set
        {
            actualValues = value;
            OnPropertyChanged();
        }
    }

    public List<string> SprintsLabels
    {
        get => sprintsLabels;
        set
        {
            sprintsLabels = value;
            OnPropertyChanged();
        }
    }

    public Func<double, string> AxisYLabelFormatter { get; } = x => ((StoryPoints)x).ToString("standard");

    public VelocityChartViewModel(IRequestBus requestBus, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);

        _ = Initialize();
    }

    private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        await RunInInitializeMode(async () =>
        {
            PresentCommitmentRequest request = new()
            {
                SprintCount = SprintCount == 0
                    ? null
                    : SprintCount
            };
            PresentCommitmentResponse response = await requestBus.Send<PresentCommitmentRequest, PresentCommitmentResponse>(request);

            SprintCount = response.RequestedSprintCount;

            IEnumerable<float> actualValues1 = response.SprintsCommitments
                .Select(x => x.ActualStoryPoints.Value);

            ActualValues = new ChartValues<float>(actualValues1);

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Actual Burn",
                    Values = ActualValues
                }
            };

            SprintsLabels = response.SprintsCommitments
                .Select(x => $"Sprint {x.SprintNumber}")
                .ToList();
        });
    }
}