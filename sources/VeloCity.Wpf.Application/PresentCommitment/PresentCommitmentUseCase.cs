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

using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentCommitment;

public class PresentCommitmentUseCase : IRequestHandler<PresentCommitmentRequest, PresentCommitmentResponse>
{
    private readonly IUnitOfWork unitOfWork;

    public PresentCommitmentUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<PresentCommitmentResponse> Handle(PresentCommitmentRequest request, CancellationToken cancellationToken)
    {
        uint sprintCount = ComputeSprintCount(request);
        List<SprintCommitment> sprintVelocities = await RetrieveSprintCommitment(sprintCount);
        return CreateResponse(sprintCount, sprintVelocities);
    }

    private static uint ComputeSprintCount(PresentCommitmentRequest request)
    {
        return request.SprintCount is null or < 1
            ? 10
            : request.SprintCount.Value;
    }

    private async Task<List<SprintCommitment>> RetrieveSprintCommitment(uint sprintCount)
    {
        IEnumerable<Sprint> sprints = await unitOfWork.SprintRepository.GetLastClosed(sprintCount);

        return sprints
            .OrderByDescending(x => x.StartDate)
            .Select(x => new SprintCommitment(x))
            .ToList();
    }

    private static PresentCommitmentResponse CreateResponse(uint sprintCount, List<SprintCommitment> sprintVelocities)
    {
        return new PresentCommitmentResponse
        {
            RequestedSprintCount = sprintCount,
            SprintsCommitments = sprintVelocities
        };
    }
}