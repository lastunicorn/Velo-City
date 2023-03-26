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
        TeamMember teamMember = RetrieveTeamMember(request.TeamMemberId);
        UpdateVacation(teamMember, request.Date, request.Hours);
        unitOfWork.SaveChanges();

        await PublishEvent(cancellationToken);

        return Unit.Value;
    }

    private TeamMember RetrieveTeamMember(int teamMemberId)
    {
        TeamMember teamMember = unitOfWork.TeamMemberRepository.Get(teamMemberId);

        if (teamMember == null)
            throw new TeamMemberDoesNotExistException(teamMemberId);

        return teamMember;
    }

    private static void UpdateVacation(TeamMember teamMember, DateTime date, HoursValue? hours)
    {
        bool shouldRemoveVacation = hours == null || hours.Value <= 0;

        if (shouldRemoveVacation)
            teamMember.RemoveVacation(date);
        else
            teamMember.AddVacation(date, hours.Value);
    }

    private async Task PublishEvent(CancellationToken cancellationToken)
    {
        TeamMemberVacationChangedEvent teamMemberVacationChangedEvent = new();
        await eventBus.Publish(teamMemberVacationChangedEvent, cancellationToken);
    }
}