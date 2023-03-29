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
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberEmployments;

internal class PresentTeamMemberEmploymentsUseCase : IRequestHandler<PresentTeamMemberEmploymentsRequest, PresentTeamMemberEmploymentsResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ApplicationState applicationState;

    public PresentTeamMemberEmploymentsUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public async Task<PresentTeamMemberEmploymentsResponse> Handle(PresentTeamMemberEmploymentsRequest request, CancellationToken cancellationToken)
    {
        List<EmploymentInfo> employmentInfos = await ComputeEmployments();
        return CreateResponse(employmentInfos);
    }

    private async Task<List<EmploymentInfo>> ComputeEmployments()
    {
        if (applicationState.SelectedTeamMemberId == null)
            return new List<EmploymentInfo>();

        int currentTeamMemberId = applicationState.SelectedTeamMemberId.Value;
        TeamMember teamMember = await unitOfWork.TeamMemberRepository.Get(currentTeamMemberId);

        if (teamMember == null)
            return new List<EmploymentInfo>();

        return teamMember.Employments
            .Select(x => new EmploymentInfo(x))
            .ToList();
    }

    private static PresentTeamMemberEmploymentsResponse CreateResponse(List<EmploymentInfo> employmentInfos)
    {
        return new PresentTeamMemberEmploymentsResponse
        {
            Employments = employmentInfos
        };
    }
}