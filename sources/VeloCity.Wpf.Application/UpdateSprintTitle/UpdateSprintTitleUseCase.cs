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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.UpdateSprintTitle;

internal class UpdateSprintTitleUseCase : IRequestHandler<UpdateSprintTitleRequest>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly EventBus eventBus;

    public UpdateSprintTitleUseCase(IUnitOfWork unitOfWork, EventBus eventBus)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<Unit> Handle(UpdateSprintTitleRequest request, CancellationToken cancellationToken)
    {
        Sprint sprint = await RetrieveSprint(request.SprintId);

        sprint.Title = request.SprintTitle;
        await unitOfWork.SaveChanges();

        await RaiseSprintUpdatedEvent(sprint, cancellationToken);

        return Unit.Value;
    }

    private async Task<Sprint> RetrieveSprint(int sprintId)
    {
        Sprint sprint = await unitOfWork.SprintRepository.Get(sprintId);

        if (sprint == null)
            throw new SprintDoesNotExistException(sprintId);

        return sprint;
    }

    private async Task RaiseSprintUpdatedEvent(Sprint sprint, CancellationToken cancellationToken)
    {
        SprintUpdatedEvent sprintUpdatedEvent = new()
        {
            SprintId = sprint.Id,
            SprintNumber = sprint.Number,
            SprintTitle = sprint.Title,
            SprintState = sprint.State,
            CommitmentStoryPoints = sprint.CommitmentStoryPoints,
            SprintGoal = sprint.Goal,
            ActualStoryPoints = sprint.ActualStoryPoints,
            ActualVelocity = sprint.Velocity,
            Comments = sprint.Comments
        };

        await eventBus.Publish(sprintUpdatedEvent, cancellationToken);
    }
}