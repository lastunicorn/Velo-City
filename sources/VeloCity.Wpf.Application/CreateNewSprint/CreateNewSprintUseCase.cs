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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintNewConfirmation;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint
{
    public class CreateNewSprintUseCase : IRequestHandler<CreateNewSprintRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserInterface userInterface;
        private readonly EventBus eventBus;
        private readonly ApplicationState applicationState;

        public CreateNewSprintUseCase(IUnitOfWork unitOfWork, IUserInterface userInterface, EventBus eventBus, ApplicationState applicationState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        }

        public async Task<Unit> Handle(CreateNewSprintRequest request, CancellationToken cancellationToken)
        {
            Sprint sprint = unitOfWork.SprintRepository.GetLast();

            SprintNewConfirmationResponse sprintNewConfirmationResponse = await RequestUserConfirmation(sprint);

            if (sprintNewConfirmationResponse.IsAccepted)
            {
                Sprint newSprint = new()
                {
                    Title = sprintNewConfirmationResponse.SprintTitle,
                    Number = sprint.Number + 1,
                    Id = sprint.Id + 1,
                    DateInterval = sprintNewConfirmationResponse.SprintTimeInterval,
                    State = SprintState.New
                };

                unitOfWork.SprintRepository.Add(newSprint);

                unitOfWork.SaveChanges();

                applicationState.SelectedSprintNumber = newSprint.Id;

                SprintsListChangedEvent sprintChangedEvent = new()
                {
                    NewSprintId = newSprint.Id
                };
                await eventBus.Publish(sprintChangedEvent, cancellationToken);
            }

            return Unit.Value;
        }

        private async Task<SprintNewConfirmationResponse> RequestUserConfirmation(Sprint sprint)
        {
            SprintNewConfirmationRequest sprintNewConfirmationRequest = new()
            {
                SprintNumber = sprint.Number + 1,
                SprintStartDate = sprint.EndDate.AddDays(1),
                SprintLength = 14
            };
            return userInterface.ConfirmNewSprint(sprintNewConfirmationRequest);
        }
    }

    public class SprintsListChangedEvent
    {
        public int? NewSprintId { get; set; }
    }
}