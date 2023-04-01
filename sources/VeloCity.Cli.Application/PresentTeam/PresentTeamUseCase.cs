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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Application.PresentTeam;

internal class PresentTeamUseCase : IRequestHandler<PresentTeamRequest, PresentTeamResponse>
{
    private readonly IUnitOfWork unitOfWork;

    public PresentTeamUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<PresentTeamResponse> Handle(PresentTeamRequest request, CancellationToken cancellationToken)
    {
        if (request.Date != null)
            return CreateResponseForDate(request.Date.Value);

        if (request.DateInterval != null)
            return CreateResponseForDateInterval(request.DateInterval.Value);

        if (request.SprintNumber != null)
            return await CreateResponseForSprint(request.SprintNumber.Value);

        return await CreateResponseForCurrentSprint();
    }

    private PresentTeamResponse CreateResponseForDate(DateTime date)
    {
        return new PresentTeamResponse
        {
            TeamMembers = unitOfWork.TeamMemberRepository.GetByDate(date)
                .OrderByEmploymentForDate(date)
                .ToList(),
            ResponseType = TeamResponseType.Date,
            Date = date
        };
    }

    private PresentTeamResponse CreateResponseForDateInterval(DateInterval dateInterval)
    {
        return new PresentTeamResponse
        {
            TeamMembers = unitOfWork.TeamMemberRepository.GetByDateInterval(dateInterval)
                .OrderByEmploymentForDate(dateInterval.StartDate)
                .ToList(),
            ResponseType = TeamResponseType.DateInterval,
            DateInterval = dateInterval
        };
    }

    private async Task<PresentTeamResponse> CreateResponseForSprint(int sprintNumber)
    {
        Sprint sprint = await RetrieveSprint(sprintNumber);
        return CreateResponseForSprint(sprint);
    }

    private async Task<Sprint> RetrieveSprint(int sprintNumber)
    {
        Sprint sprint = await unitOfWork.SprintRepository.GetByNumber(sprintNumber);

        return sprint ?? throw new SprintDoesNotExistException(sprintNumber);
    }

    private async Task<PresentTeamResponse> CreateResponseForCurrentSprint()
    {
        Sprint currentSprint = await RetrieveCurrentSprint();
        return CreateResponseForSprint(currentSprint);
    }

    private async Task<Sprint> RetrieveCurrentSprint()
    {
        Sprint currentSprint = await unitOfWork.SprintRepository.GetLastInProgress();

        return currentSprint ?? throw new NoSprintException();
    }

    private PresentTeamResponse CreateResponseForSprint(Sprint sprint)
    {
        return new PresentTeamResponse
        {
            TeamMembers = unitOfWork.TeamMemberRepository.GetByDateInterval(sprint.DateInterval)
                .OrderByEmploymentForDate(sprint.StartDate)
                .ToList(),
            ResponseType = TeamResponseType.Sprint,
            SprintNumber = sprint.Number,
            DateInterval = sprint.DateInterval
        };
    }
}