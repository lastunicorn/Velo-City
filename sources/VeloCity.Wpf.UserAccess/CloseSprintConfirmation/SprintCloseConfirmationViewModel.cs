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
using DustInTheWind.VeloCity.Wpf.Presentation;

namespace DustInTheWind.VeloCity.Wpf.UserAccess.CloseSprintConfirmation
{
    public class SprintCloseConfirmationViewModel : ViewModelBase
    {
        private string title;
        private string sprintName;
        private int sprintNumber;
        private StoryPoints actualStoryPoints;
        private string comments;

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

        public StoryPoints ActualStoryPoints
        {
            get => actualStoryPoints;
            set
            {
                actualStoryPoints = value;
                OnPropertyChanged();
            }
        }

        public string Comments
        {
            get => comments;
            set
            {
                comments = value;
                OnPropertyChanged();
            }
        }

        private void RefreshTitle()
        {
            Title = $"Close Sprint {sprintNumber}";
        }
    }
}