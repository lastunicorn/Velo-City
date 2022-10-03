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

using System.Text;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.StartSprintConfirmation
{
    public class SprintStartConfirmationViewModel : ViewModelBase
    {
        private string title;
        private string sprintName;
        private int sprintNumber;
        private StoryPoints estimatedStoryPoints;
        private StoryPoints commitmentStoryPoints;
        private string sprintTitle;
        private string sprintGoal;

        public string Title
        {
            get => title;
            private set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public string SprintName
        {
            get => sprintName;
            set
            {
                sprintName = value;

                RefreshTitle();
            }
        }

        public int SprintNumber
        {
            get => sprintNumber;
            set
            {
                sprintNumber = value;

                RefreshTitle();
            }
        }

        public StoryPoints EstimatedStoryPoints
        {
            get => estimatedStoryPoints;
            set
            {
                estimatedStoryPoints = value;
                OnPropertyChanged();
            }
        }

        public StoryPoints CommitmentStoryPoints
        {
            get => commitmentStoryPoints;
            set
            {
                commitmentStoryPoints = value;
                OnPropertyChanged();
            }
        }

        public string SprintTitle
        {
            get => sprintTitle;
            set
            {
                sprintTitle = value;
                OnPropertyChanged();
            }
        }

        public string SprintGoal
        {
            get => sprintGoal;
            set
            {
                sprintGoal = value;
                OnPropertyChanged();
            }
        }

        private void RefreshTitle()
        {
            StringBuilder sb = new();

            sb.Append($"Start Sprint {sprintNumber}");

            if (sprintName != null)
                sb.Append($" - {sprintName}");

            Title = sb.ToString();
        }
    }
}