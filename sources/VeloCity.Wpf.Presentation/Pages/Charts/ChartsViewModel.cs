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
using DustInTheWind.VeloCity.Wpf.Application.PresentVelocity;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using LiveCharts;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.Charts
{
    public class ChartsViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private ChartValues<float> values;
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

        public ChartValues<float> Values
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

        public Func<double, string> AxisYLabelFormatter { get; } = x => ((Velocity)x).ToString("standard");

        public ChartsViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<RefreshEvent>(HandleRefreshEvent);

            _ = Initialize();
        }

        private async Task HandleRefreshEvent(RefreshEvent ev, CancellationToken cancellationToken)
        {
            await Initialize();
        }

        private async Task Initialize()
        {
            await RunInInitializeMode(async () =>
            {
                PresentVelocityRequest request = new()
                {
                    SprintCount = SprintCount == 0
                        ? null
                        : SprintCount
                };
                PresentVelocityResponse response = await mediator.Send(request);

                SprintCount = response.RequestedSprintCount;

                IEnumerable<float> velocityValues = response.SprintVelocities
                    .Select(x => x.Velocity.Value);

                Values = new ChartValues<float>(velocityValues);

                SprintsLabels = response.SprintVelocities
                    .Select(x => $"Sprint {x.SprintNumber}")
                    .ToList();
            });
        }
    }
}