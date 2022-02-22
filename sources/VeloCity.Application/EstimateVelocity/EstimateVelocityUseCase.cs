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
using DustInTheWind.VeloCity.Application.SprintVelocity;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Application.EstimateVelocity
{
    public class EstimateVelocityUseCase : IRequestHandler<EstimateVelocityRequest, EstimateVelocityResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public EstimateVelocityUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<EstimateVelocityResponse> Handle(EstimateVelocityRequest request, CancellationToken cancellationToken)
        {
            float averageVelocity = unitOfWork.SprintRepository.GetBefore(request.SprintId, request.LookBack).ToList()
                .Select(x =>
                {
                    int totalWorkHours = unitOfWork.TeamMemberRepository.GetAll()
                        .Select(z => z.CalculateHoursFor(x))
                        .Sum();

                    return (float)x.StoryPoints / totalWorkHours;
                })
                .Average();

            Sprint sprint = unitOfWork.SprintRepository.Get(request.SprintId);
            
            int totalWorkHours = unitOfWork.TeamMemberRepository.GetAll()
                .Select(z => z.CalculateHoursFor(sprint))
                .Sum();

            EstimateVelocityResponse response = new()
            {
                SprintName = sprint.Name,
                WorkDays = sprint.CalculateWorkDays().ToList(),
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                TotalWorkHours = totalWorkHours,
                EstimatedStoryPoints = totalWorkHours * averageVelocity,
                EstimatedVelocity = averageVelocity
            };

            return Task.FromResult(response);
        }
    }
}