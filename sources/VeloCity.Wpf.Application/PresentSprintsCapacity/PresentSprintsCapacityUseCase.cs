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
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintsCapacity
{
    public class PresentSprintsCapacityUseCase : IRequestHandler<PresentSprintsCapacityRequest, PresentSprintsCapacityResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentSprintsCapacityUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentSprintsCapacityResponse> Handle(PresentSprintsCapacityRequest request, CancellationToken cancellationToken)
        {
            uint sprintCount = CalculateSprintCount(request);
            List<SprintCapacity> sprintsCommitments = RetrieveSprintCommitment(sprintCount);

            PresentSprintsCapacityResponse response = new()
            {
                RequestedSprintCount = sprintCount,
                SprintCapacities = sprintsCommitments
            };

            return Task.FromResult(response);
        }

        private static uint CalculateSprintCount(PresentSprintsCapacityRequest request)
        {
            return request.SprintCount is null or < 1
                ? 10
                : request.SprintCount.Value;
        }

        private List<SprintCapacity> RetrieveSprintCommitment(uint sprintCount)
        {
            return unitOfWork.SprintRepository.GetLastClosed(sprintCount)
                .OrderByDescending(x => x.StartDate)
                .Select(x => new SprintCapacity(x))
                .ToList();
        }
    }
}