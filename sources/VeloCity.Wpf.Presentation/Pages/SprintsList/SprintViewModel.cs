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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprints;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintsList
{
    public class SprintViewModel : ViewModelBase
    {
        private SprintState sprintState;

        public int SprintId { get; }

        public string SprintName { get; }

        public int SprintNumber { get; }

        public DateInterval SprintDateInterval { get; }

        public SprintState SprintState
        {
            get => sprintState;
            set
            {
                sprintState = value;
                OnPropertyChanged();
            }
        }

        public SprintViewModel(SprintInfo sprintInfo)
        {
            if (sprintInfo == null) throw new ArgumentNullException(nameof(sprintInfo));

            SprintId = sprintInfo.Id;
            SprintName = sprintInfo.Name;
            SprintNumber = sprintInfo.Number;
            SprintState = sprintInfo.State;
            SprintDateInterval = sprintInfo.DateInterval;
        }

        public override string ToString()
        {
            return $"{SprintName} [{SprintDateInterval}]";
        }
    }
}