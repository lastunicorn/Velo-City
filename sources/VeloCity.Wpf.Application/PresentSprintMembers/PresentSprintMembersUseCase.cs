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

    public async Task<PresentSprintMembersResponse> Handle(PresentSprintMembersRequest request, CancellationToken cancellationToken)
    {
        Sprint sprint = await RetrieveSprintToAnalyze();
        List<SprintMember> sprintMembers = ComputeSprintMemberList(sprint);

        return CreateResponse(sprintMembers);
    }

    private async Task<Sprint> RetrieveSprintToAnalyze()
    {
        int? currentSprintId = applicationState.SelectedSprintId;

        return currentSprintId == null
            ? await RetrieveDefaultSprintToAnalyze()
            : await RetrieveSpecificSprintToAnalyze(applicationState.SelectedSprintId.Value);
    }

    private async Task<Sprint> RetrieveDefaultSprintToAnalyze()
    {
        Sprint sprint = await unitOfWork.SprintRepository.GetLastInProgress();

        return sprint ?? throw new NoSprintInProgressException();
    }

    private async Task<Sprint> RetrieveSpecificSprintToAnalyze(int sprintNumber)
    {
        Sprint sprint = await unitOfWork.SprintRepository.Get(sprintNumber);

        return sprint ?? throw new SprintDoesNotExistException(sprintNumber);
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
                .Select(x => new SprintMemberDto(x))
                .ToList()
        };
    }
}