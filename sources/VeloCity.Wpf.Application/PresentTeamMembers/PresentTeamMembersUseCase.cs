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

namespace DustInTheWind.VeloCity.Wpf.Application.PresentTeamMembers;

internal class PresentTeamMembersUseCase : IRequestHandler<PresentTeamMembersRequest, PresentTeamMembersResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ApplicationState applicationState;

    public PresentTeamMembersUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public Task<PresentTeamMembersResponse> Handle(PresentTeamMembersRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<TeamMember> teamMembers = RetrieveTeamMembers();
        IEnumerable<TeamMember> orderedTeamMembers = OrderTeamMembers(teamMembers);
        PresentTeamMembersResponse response = CreateResponse(orderedTeamMembers);

        return Task.FromResult(response);
    }

    private IEnumerable<TeamMember> RetrieveTeamMembers()
    {
        return unitOfWork.TeamMemberRepository.GetAll();
    }

    private static IEnumerable<TeamMember> OrderTeamMembers(IEnumerable<TeamMember> allTeamMembers)
    {
        TeamMemberList teamMemberList = new(allTeamMembers);
        teamMemberList.OrderEmployedFirst();
        
        return teamMemberList;
    }

    private PresentTeamMembersResponse CreateResponse(IEnumerable<TeamMember> teamMembers)
    {
        return new PresentTeamMembersResponse
        {
            TeamMembers = teamMembers
                .Select(x => new TeamMemberInfo(x))
                .ToList(),
            CurrentTeamMemberId = applicationState.SelectedTeamMemberId
        };
    }
}