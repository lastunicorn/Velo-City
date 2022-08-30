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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CanStartSprint;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Commands
{
    public class StartSprintCommand : ICommand
    {
        private readonly IMediator mediator;
        private readonly EventBus eventBus;

        public event EventHandler CanExecuteChanged;

        public StartSprintCommand(IMediator mediator, EventBus eventBus)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            eventBus.Subscribe<CurrentSprintChangedEvent>(HandleCurrentSprintChangedEvent);
        }

        private Task HandleCurrentSprintChangedEvent(CurrentSprintChangedEvent ev, CancellationToken cancellationToken)
        {
            OnCanExecuteChanged();

            return Task.CompletedTask;
        }

        public bool CanExecute(object parameter)
        {
            return CanStartCurrentSprint().Result;
        }

        private async Task<bool> CanStartCurrentSprint()
        {
            CanStartSprintRequest request = new();
            CanStartSprintResponse response = await mediator.Send(request);

            return response.CanStartCurrentSprint;
        }

        public void Execute(object parameter)
        {
            StartSprintRequest request = new();
            _ = mediator.Send(request);
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}