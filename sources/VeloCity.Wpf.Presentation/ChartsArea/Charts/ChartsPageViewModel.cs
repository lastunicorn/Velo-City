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
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Presentation.ChartsArea.CommitmentChart;
using DustInTheWind.VeloCity.Wpf.Presentation.ChartsArea.VelocityChart;

namespace DustInTheWind.VeloCity.Wpf.Presentation.ChartsArea.Charts
{
    public class ChartsPageViewModel : ViewModelBase
    {
        private ChartItemViewModel selectedChartItemViewModel;

        public List<ChartItemViewModel> ChartItemViewModels { get; }

        public ChartItemViewModel SelectedChartItemViewModel
        {
            get => selectedChartItemViewModel;
            set
            {
                selectedChartItemViewModel = value;
                OnPropertyChanged();
            }
        }

        public ChartsPageViewModel(IRequestBus requestBus, EventBus eventBus)
        {
            if (requestBus == null) throw new ArgumentNullException(nameof(requestBus));
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));

            ChartItemViewModels = new List<ChartItemViewModel>
            {
                new()
                {
                    Title = "Burn Velocity Chart",
                    ViewModel = new VelocityChartViewModel(requestBus, eventBus)
                },
                new()
                {
                    Title = "Commitment Chart",
                    ViewModel = new CommitmentChartViewModel(requestBus, eventBus)
                }
            };

            SelectedChartItemViewModel = ChartItemViewModels[0];
        }
    }
}