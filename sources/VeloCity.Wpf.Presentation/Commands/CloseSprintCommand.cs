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
using DustInTheWind.VeloCity.Wpf.Application.CanCloseSprint;
using DustInTheWind.VeloCity.Wpf.Application.CloseSprint;
using DustInTheWind.VeloCity.Wpf.Application.Refresh;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using DustInTheWind.VeloCity.Wpf.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Commands
{
    public class CloseSprintCommand : ICommand
    {
        private readonly IMediator mediator;

        public event EventHandler CanExecuteChanged;

        public CloseSprintCommand(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
            eventBus.Subscribe<SprintChangedEvent>(HandleSprintChangedEvent);
            eventBus.Subscribe<SprintUpdatedEvent>(HandleSprintUpdatedEvent);
        }

        private Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
        {
            OnCanExecuteChanged();

            return Task.CompletedTask;
        }

        private Task HandleSprintChangedEvent(SprintChangedEvent ev, CancellationToken cancellationToken)
        {
            OnCanExecuteChanged();

            return Task.CompletedTask;
        }

        private Task HandleSprintUpdatedEvent(SprintUpdatedEvent ev, CancellationToken cancellationToken)
        {
            OnCanExecuteChanged();

            return Task.CompletedTask;
        }

        public bool CanExecute(object parameter)
        {
            return CanCloseCurrentSprint().Result;
        }

        private async Task<bool> CanCloseCurrentSprint()
        {
            CanCloseSprintRequest request = new();
            CanCloseSprintResponse response = await mediator.Send(request);

            return response.CanCloseSprint;
        }

        public void Execute(object parameter)
        {
            CloseSprintRequest request = new();
            _ = mediator.Send(request);
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}