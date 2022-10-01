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
using System.Windows;
using System.Windows.Input;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMemberCalendar;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMembers
{
    public class ShowSprintMemberCalendarCommand : ICommand
    {
        private readonly IMediator mediator;

        public SprintMember SprintMember { get; set; }

        public event EventHandler CanExecuteChanged;

        public ShowSprintMemberCalendarCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            TeamMemberSprintViewModel viewModel = new(mediator);
            viewModel.SetSprintMember(SprintMember);

            Window owner = System.Windows.Application.Current.MainWindow;

            TeamMemberSprintWindow window = new()
            {
                DataContext = viewModel,
                Owner = owner
            };
            
            if (owner != null)
            {
                window.Width = owner.ActualWidth - 150;
                window.Height = owner.ActualHeight - 100;
            }

            window.ShowDialog();
        }
    }
}