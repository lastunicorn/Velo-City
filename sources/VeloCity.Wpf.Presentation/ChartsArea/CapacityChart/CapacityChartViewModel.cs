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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintsCapacity;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;
using LiveCharts;
using LiveCharts.Wpf;

namespace DustInTheWind.VeloCity.Wpf.Presentation.ChartsArea.CapacityChart
{
    internal class CapacityChartViewModel : ViewModelBase
    {
        private readonly IRequestBus requestBus;
        private ChartValues<int> values;
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
        
        public ChartValues<int> Values
        {
            get => values;
            private set
            {
                values = value;
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

        public Func<double, string> AxisYLabelFormatter { get; } = x => ((HoursValue)x).ToString();

        public CapacityChartViewModel(IRequestBus requestBus, EventBus eventBus)
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
                PresentSprintsCapacityRequest request = new()
                {
                    SprintCount = SprintCount == 0
                        ? null
                        : SprintCount
                };
                PresentSprintsCapacityResponse response = await requestBus.Send<PresentSprintsCapacityRequest, PresentSprintsCapacityResponse>(request);

                SprintCount = response.RequestedSprintCount;
                
                IEnumerable<int> actualValues1 = response.SprintCapacities
                    .Select(x => x.Hours.Value);

                Values = new ChartValues<int>(actualValues1);

                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Sprint Capacity",
                        Values = Values
                    }
                };

                SprintsLabels = response.SprintCapacities
                    .Select(x => $"Sprint {x.SprintNumber}")
                    .ToList();
            });
        }
    }
}