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

using DustInTheWind.VeloCity.Ports.UserAccess.SprintStartConfirmation;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.StartSprintConfirmation;

namespace DustInTheWind.VeloCity.Wpf.UserAccess
{
    public class SprintStartConfirmationDisplay : ISprintStartConfirmation
    {
        public SprintStartConfirmationResponse ConfirmStartSprint(SprintStartConfirmationRequest request)
        {
            SprintStartConfirmationViewModel viewModel = new()
            {
                SprintTitle = request.SprintTitle,
                SprintNumber = request.SprintNumber,
                EstimatedStoryPoints = request.EstimatedStoryPoints,
                CommitmentStoryPoints = request.CommitmentStoryPoints,
                SprintGoal = request.SprintGoal
            };
            SprintStartConfirmationWindow window = new()
            {
                DataContext = viewModel,
                Owner = System.Windows.Application.Current.MainWindow
            };

            bool? response = window.ShowDialog();

            return new SprintStartConfirmationResponse
            {
                IsAccepted = response == true,
                CommitmentStoryPoints = viewModel.CommitmentStoryPoints,
                SprintTitle = viewModel.SprintTitle,
                SprintGoal = viewModel.SprintGoal
            };
        }
    }
}