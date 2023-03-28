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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;

internal class PresentSprintCalendarUseCase : IRequestHandler<PresentSprintCalendarRequest, PresentSprintCalendarResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ApplicationState applicationState;
    private readonly ISystemClock systemClock;

    public PresentSprintCalendarUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState, ISystemClock systemClock)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    }

    public async Task<PresentSprintCalendarResponse> Handle(PresentSprintCalendarRequest request, CancellationToken cancellationToken)
    {
        Sprint sprintToAnalyze = await RetrieveSprintToAnalyze();
        List<SprintCalendarDay> sprintCalendarDays = CreateCalendarDays(sprintToAnalyze);

        return new PresentSprintCalendarResponse
        {
            SprintCalendarDays = sprintCalendarDays
        };
    }

    private async Task<Sprint> RetrieveSprintToAnalyze()
    {
        return applicationState.SelectedSprintId == null
            ? await RetrieveDefaultSprintToAnalyze()
            : await RetrieveSpecificSprintToAnalyze(applicationState.SelectedSprintId.Value);
    }

    private async Task<Sprint> RetrieveDefaultSprintToAnalyze()
    {
        Sprint sprint = await unitOfWork.SprintRepository.GetLastInProgress();

        if (sprint == null)
            throw new NoSprintInProgressException();

        return sprint;
    }

    private async Task<Sprint> RetrieveSpecificSprintToAnalyze(int sprintNumber)
    {
        Sprint sprint = await unitOfWork.SprintRepository.Get(sprintNumber);

        if (sprint == null)
            throw new SprintDoesNotExistException(sprintNumber);

        return sprint;
    }

    private List<SprintCalendarDay> CreateCalendarDays(Sprint sprint)
    {
        IEnumerable<SprintDay> sprintDays = sprint.EnumerateAllDays();
        IEnumerable<SprintMember> sprintMembers = sprint.SprintMembersOrderedByEmployment;

        return sprintDays
            .Select(x =>
            {
                List<SprintMemberDay> sprintMemberDays = GetAllSprintMemberDays(x.Date, sprintMembers);
                return new SprintCalendarDay(x, sprintMemberDays, systemClock.Today);
            })
            .ToList();
    }

    private static List<SprintMemberDay> GetAllSprintMemberDays(DateTime date, IEnumerable<SprintMember> sprintMembers)
    {
        if (sprintMembers == null)
            return new List<SprintMemberDay>();

        return sprintMembers
            .Select(x => x.Days[date])
            .Where(x => x != null)
            .ToList();
    }
}