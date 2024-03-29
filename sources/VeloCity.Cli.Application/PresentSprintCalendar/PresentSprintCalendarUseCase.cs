﻿// VeloCity
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
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Application.PresentSprintCalendar;

internal class PresentSprintCalendarUseCase : IRequestHandler<PresentSprintCalendarRequest, PresentSprintCalendarResponse>
{
    private readonly IUnitOfWork unitOfWork;

    public PresentSprintCalendarUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<PresentSprintCalendarResponse> Handle(PresentSprintCalendarRequest request, CancellationToken cancellationToken)
    {
        if (request.SprintNumber != null)
            return await CreateSprintCalendarResponse(request.SprintNumber.Value);

        if (request.StartDate != null)
            return await CreateMonthCalendarResponse(request.StartDate.Value, request.EndDate);

        return await CreateSprintCalendarResponse();
    }

    private async Task<PresentSprintCalendarResponse> CreateSprintCalendarResponse(int sprintNumber)
    {
        Sprint sprint = await unitOfWork.SprintRepository.GetByNumber(sprintNumber);

        if (sprint == null)
            throw new SprintDoesNotExistException(sprintNumber);

        return new PresentSprintCalendarResponse
        {
            SprintCalendar = new SprintCalendar(sprint)
        };
    }

    private async Task<PresentSprintCalendarResponse> CreateMonthCalendarResponse(DateTime startDate, DateTime? endDate)
    {
        MonthEnumeration monthEnumeration = new()
        {
            StartDate = startDate,
            EndDate = endDate,
            Count = endDate == null ? 1 : null
        };

        List<OfficialHoliday> officialHolidays = (await unitOfWork.OfficialHolidayRepository.GetAll())
            .ToList();

        return new PresentSprintCalendarResponse
        {
            MonthCalendars = monthEnumeration
                .Select(x =>
                {
                    DateTime monthStartDate = x.StartDate!.Value;
                    DateTime monthEndDate = x.EndDate!.Value;
                    DateInterval monthDateInterval = new(startDate, endDate);

                    IEnumerable<TeamMember> teamMembers = unitOfWork.TeamMemberRepository.GetByDateInterval(monthDateInterval).Result;

                    return new MonthCalendar(monthStartDate, monthEndDate)
                    {
                        OfficialHolidays = officialHolidays,
                        TeamMembers = teamMembers.ToList()
                    };
                })
                .ToList()
        };
    }

    private async Task<PresentSprintCalendarResponse> CreateSprintCalendarResponse()
    {
        Sprint sprint = await unitOfWork.SprintRepository.GetLastInProgress();

        if (sprint == null)
            throw new NoSprintInProgressException();

        return new PresentSprintCalendarResponse
        {
            SprintCalendar = new SprintCalendar(sprint)
        };
    }
}