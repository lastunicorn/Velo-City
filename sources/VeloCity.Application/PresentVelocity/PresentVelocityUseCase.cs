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
using DustInTheWind.VeloCity.Domain;
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
            int sprintCount = request.Count is null or < 1
                ? 10
                : request.Count.Value;

            List<SprintVelocity> sprintVelocities = unitOfWork.SprintRepository.GetPage(0, sprintCount)
                .Select(x => new SprintVelocity
                {
                    SprintNumber = x.Number,
                    Velocity = CalculateVelocity(x)
                })
                .ToList();

            PresentVelocityResponse response = new()
            {
                SprintVelocities = sprintVelocities
            };

            return Task.FromResult(response);
        }

        private Velocity CalculateVelocity(Sprint sprint)
        {
            List<SprintMember> sprintMembers = unitOfWork.TeamMemberRepository.GetAll()
                .Select(x => x.ToSprintMember(sprint))
                .ToList();

            int totalWorkHours = sprintMembers
                .SelectMany(x => x.Days.Select(z => z.WorkHours))
                .Sum();

            return sprint.ActualStoryPoints / totalWorkHours;
        }
    }
}