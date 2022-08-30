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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.StartSprint
{
    internal class StartSprintUseCase : IRequestHandler<StartSprintRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;
        private readonly EventBus eventBus;

        public StartSprintUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState, EventBus eventBus)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<Unit> Handle(StartSprintRequest request, CancellationToken cancellationToken)
        {
            int? sprintId = applicationState.SelectedSprintId;

            if (sprintId == null)
                throw new Exception("No sprint is selected.");

            Sprint sprint = unitOfWork.SprintRepository.Get(sprintId.Value);

            ValidateSprintState(sprint);

            // todo: validate that the sprint is the next "new" sprint.

            sprint.State = SprintState.InProgress;

            await RaiseSprintUpdatedEvent(sprint, cancellationToken);

            return Unit.Value;
        }

        private static void ValidateSprintState(Sprint sprint)
        {
            switch (sprint.State)
            {
                case SprintState.Unknown:
                    throw new Exception($"The sprint {sprint.Number} is in a invalid state.");

                case SprintState.New:
                    break;

                case SprintState.InProgress:
                    throw new Exception($"The sprint {sprint.Number} is already in progress.");

                case SprintState.Closed:
                    throw new Exception($"The sprint {sprint.Number} is closed. A closed sprint cannot be started again.");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task RaiseSprintUpdatedEvent(Sprint sprint, CancellationToken cancellationToken)
        {
            SprintUpdatedEvent sprintUpdatedEvent = new()
            {
                SprintId = sprint.Id,
                SprintState = sprint.State
            };

            await eventBus.Publish(sprintUpdatedEvent, cancellationToken);
        }
    }
}