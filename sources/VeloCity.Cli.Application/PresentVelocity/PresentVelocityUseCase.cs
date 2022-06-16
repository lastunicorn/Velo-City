// Velo City
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
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Application.PresentVelocity
{
    public class PresentVelocityUseCase : IRequestHandler<PresentVelocityRequest, PresentVelocityResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentVelocityUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentVelocityResponse> Handle(PresentVelocityRequest request, CancellationToken cancellationToken)
        {
            int sprintCount = CalculateSprintCount(request);
            List<SprintVelocity> sprintVelocities = RetrieveSprintVelocities(sprintCount);

            PresentVelocityResponse response = new()
            {
                SprintVelocities = sprintVelocities
            };

            return Task.FromResult(response);
        }

        private static int CalculateSprintCount(PresentVelocityRequest request)
        {
            return request.SprintCount is null or < 1
                ? 10
                : request.SprintCount.Value;
        }

        private List<SprintVelocity> RetrieveSprintVelocities(int sprintCount)
        {
            return unitOfWork.SprintRepository.GetLast(sprintCount)
                .Select(x => new SprintVelocity(x))
                .ToList();
        }
    }
}