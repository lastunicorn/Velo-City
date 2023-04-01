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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Application.PresentVacations;

internal class PresentVacationsUseCase : IRequestHandler<PresentVacationsRequest, PresentVacationsResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ISystemClock systemClock;

    public PresentVacationsUseCase(IUnitOfWork unitOfWork, ISystemClock systemClock)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    }

    public Task<PresentVacationsResponse> Handle(PresentVacationsRequest request, CancellationToken cancellationToken)
    {
        PresentVacationsResponse response = CreateResponse(request);
        return Task.FromResult(response);
    }

    private PresentVacationsResponse CreateResponse(PresentVacationsRequest request)
    {
        if (request.TeamMemberName != null)
            return GetVacationsByTeamMember(request.TeamMemberName);

        if (request.Date != null)
            return GetVacationsByDate(request.Date.Value);

        return GetVacationsByDate(systemClock.Today);
    }

    private PresentVacationsResponse GetVacationsByTeamMember(string teamMemberName)
    {
        IEnumerable<TeamMember> teamMembers = unitOfWork.TeamMemberRepository.Find(teamMemberName);

        return new PresentVacationsResponse
        {
            TeamMemberVacations = teamMembers
                .OrderByEmployment()
                .Select(x => new TeamMemberVacations(x))
                .ToList(),
            RequestType = RequestType.ByName,
            RequestedTeamMemberName = teamMemberName
        };
    }

    private PresentVacationsResponse GetVacationsByDate(DateTime date)
    {
        IEnumerable<TeamMember> teamMembers = unitOfWork.TeamMemberRepository.GetByDate(date);

        return new PresentVacationsResponse
        {
            TeamMemberVacations = teamMembers
                .OrderByEmployment()
                .Select(x => new TeamMemberVacations(x))
                .ToList(),
            RequestType = RequestType.ByCurrentDate,
            RequestedDate = date
        };
    }
}