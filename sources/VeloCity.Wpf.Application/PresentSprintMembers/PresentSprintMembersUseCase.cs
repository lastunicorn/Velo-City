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
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintMembers;

internal class PresentSprintMembersUseCase : IRequestHandler<PresentSprintMembersRequest, PresentSprintMembersResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ApplicationState applicationState;

    public PresentSprintMembersUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public Task<PresentSprintMembersResponse> Handle(PresentSprintMembersRequest request, CancellationToken cancellationToken)
    {
        Sprint sprint = RetrieveSprintToAnalyze();
        List<SprintMember> sprintMembers = ComputeSprintMemberList(sprint);
        PresentSprintMembersResponse response = CreateResponse(sprintMembers);

        return Task.FromResult(response);
    }

    private Sprint RetrieveSprintToAnalyze()
    {
        int? currentSprintId = applicationState.SelectedSprintId;

        return currentSprintId == null
            ? RetrieveDefaultSprintToAnalyze()
            : RetrieveSpecificSprintToAnalyze(applicationState.SelectedSprintId.Value);
    }

    private Sprint RetrieveDefaultSprintToAnalyze()
    {
        Sprint sprint = unitOfWork.SprintRepository.GetLastInProgress();

        if (sprint == null)
            throw new NoSprintInProgressException();

        return sprint;
    }

    private Sprint RetrieveSpecificSprintToAnalyze(int sprintNumber)
    {
        Sprint sprint = unitOfWork.SprintRepository.Get(sprintNumber);

        if (sprint == null)
            throw new SprintDoesNotExistException(sprintNumber);

        return sprint;
    }

    private static List<SprintMember> ComputeSprintMemberList(Sprint sprint)
    {
        return sprint.SprintMembersOrderedByEmployment?.ToList();
    }

    private static PresentSprintMembersResponse CreateResponse(List<SprintMember> sprintMembers)
    {
        return new PresentSprintMembersResponse
        {
            SprintMembers = sprintMembers
            .Select(x=> new SprintMemberDto(x))
            .ToList()
        };
    }
}
