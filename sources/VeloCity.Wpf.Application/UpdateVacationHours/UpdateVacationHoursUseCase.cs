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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;

internal class UpdateVacationHoursUseCase : IRequestHandler<UpdateVacationHoursRequest, Unit>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly EventBus eventBus;

    public UpdateVacationHoursUseCase(IUnitOfWork unitOfWork, EventBus eventBus)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<Unit> Handle(UpdateVacationHoursRequest request, CancellationToken cancellationToken)
    {
        TeamMember teamMember = await RetrieveTeamMember(request.TeamMemberId);
        teamMember.SetVacation(request.Date, request.Hours);
        unitOfWork.SaveChanges();

        await PublishEvent(cancellationToken);

        return Unit.Value;
    }

    private async Task<TeamMember> RetrieveTeamMember(int teamMemberId)
    {
        TeamMember teamMember = await unitOfWork.TeamMemberRepository.Get(teamMemberId);

        return teamMember ?? throw new TeamMemberDoesNotExistException(teamMemberId);
    }

    private async Task PublishEvent(CancellationToken cancellationToken)
    {
        TeamMemberVacationChangedEvent teamMemberVacationChangedEvent = new();
        await eventBus.Publish(teamMemberVacationChangedEvent, cancellationToken);
    }
}