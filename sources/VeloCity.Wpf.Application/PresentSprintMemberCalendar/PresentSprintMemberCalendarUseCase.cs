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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintMemberCalendar;

public class PresentSprintMemberCalendarUseCase : IRequestHandler<PresentSprintMemberCalendarRequest, PresentSprintMemberCalendarResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ISystemClock systemClock;

    public PresentSprintMemberCalendarUseCase(IUnitOfWork unitOfWork, ISystemClock systemClock)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    }

    public async Task<PresentSprintMemberCalendarResponse> Handle(PresentSprintMemberCalendarRequest request, CancellationToken cancellationToken)
    {
        Sprint sprint = await RetrieveSprint(request.SprintId);
        SprintMember sprintMember = GetSprintMember(sprint, request.TeamMemberId);

        DateTime currentDate = systemClock.Today;

        return new PresentSprintMemberCalendarResponse
        {
            TeamMemberId = sprintMember.TeamMember.Id,
            TeamMemberName = sprintMember.Name,
            SprintId = sprintMember.Sprint.Id,
            SprintNumber = sprintMember.Sprint.Number,
            Days = sprintMember.Days
                .Select(x => new SprintMemberDayDto(x, currentDate))
                .ToList()
        };
    }

    private async Task<Sprint> RetrieveSprint(int sprintId)
    {
        Sprint sprint = await unitOfWork.SprintRepository.Get(sprintId);

        if (sprint == null)
            throw new SprintDoesNotExistException(sprintId);

        return sprint;
    }

    private static SprintMember GetSprintMember(Sprint sprint, int teamMemberId)
    {
        SprintMember sprintMember = sprint.GetSprintMember(teamMemberId);

        if (sprintMember == null)
            throw new TeamMemberNotInSprintException(teamMemberId, sprint.Number);

        return sprintMember;
    }
}