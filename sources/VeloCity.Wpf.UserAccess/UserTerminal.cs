// VeloCity
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

using System.Windows;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.NewTeamMemberConfirmation;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintCloseConfirmation;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintNewConfirmation;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintStartConfirmation;
using DustInTheWind.VeloCity.Wpf.UserAccess.CloseSprintConfirmation;
using DustInTheWind.VeloCity.Wpf.UserAccess.NewSprintConfirmation;
using DustInTheWind.VeloCity.Wpf.UserAccess.NewTeamMemberConfirmation;
using DustInTheWind.VeloCity.Wpf.UserAccess.StartSprintConfirmation;

namespace DustInTheWind.VeloCity.Wpf.UserAccess;

public class UserTerminal : IUserTerminal
{
    public SprintNewConfirmationResponse ConfirmNewSprint(SprintNewConfirmationRequest request)
    {
        SprintNewConfirmationViewModel viewModel = new(request.SprintNumber, request.SprintStartDate)
        {
            SprintTitle = request.SprintTitle,
            SprintLength = request.SprintLength
        };
        NewSprintConfirmationWindow window = new()
        {
            DataContext = viewModel,
            Owner = Application.Current.MainWindow
        };

        bool? isAccepted = window.ShowDialog();

        return new SprintNewConfirmationResponse
        {
            IsAccepted = isAccepted == true,
            SprintTitle = viewModel.SprintTitle,
            SprintTimeInterval = new DateInterval(viewModel.StartDate, viewModel.EndDate)
        };
    }

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
            Owner = Application.Current.MainWindow
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

    public SprintCloseConfirmationResponse ConfirmCloseSprint(SprintCloseConfirmationRequest request)
    {
        SprintCloseConfirmationViewModel viewModel = new()
        {
            SprintName = request.SprintName,
            SprintNumber = request.SprintNumber,
            Comments = request.Comments
        };

        SprintCloseConfirmationWindow window = new()
        {
            DataContext = viewModel,
            Owner = Application.Current.MainWindow
        };

        bool? response = window.ShowDialog();

        return new SprintCloseConfirmationResponse
        {
            IsAccepted = response == true,
            ActualStoryPoints = viewModel.ActualStoryPoints,
            Comments = viewModel.Comments
        };
    }

    public NewTeamMemberConfirmationResponse ConfirmNewTeamMember(NewTeamMemberConfirmationRequest request)
    {
        NewTeamMemberConfirmationViewModel viewModel = new()
        {
            EmploymentHours = request.EmploymentHours,
            EmploymentCountry = request.EmploymentCountry,
            StartDate = request.StartDate
        };

        NewTeamMemberConfirmationWindow window = new()
        {
            DataContext = viewModel,
            Owner = Application.Current.MainWindow
        };

        bool? response = window.ShowDialog();

        return new NewTeamMemberConfirmationResponse
        {
            IsAccepted = response == true,
            EmploymentHours = viewModel.EmploymentHours,
            EmploymentCountry = viewModel.EmploymentCountry,
            StartDate = viewModel.StartDate,
            TeamMemberName = new PersonName()
            {
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                LastName = viewModel.LastName,
                Nickname = viewModel.Nickname,
            },
            EmploymentWeek = EmploymentWeek.NewDefault
        };
    }
}