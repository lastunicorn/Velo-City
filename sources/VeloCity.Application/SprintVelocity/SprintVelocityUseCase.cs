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

namespace DustInTheWind.VeloCity.Application.SprintVelocity
{
    public class SprintVelocityUseCase : IRequestHandler<SprintVelocityRequest, SprintVelocityResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public SprintVelocityUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<SprintVelocityResponse> Handle(SprintVelocityRequest request, CancellationToken cancellationToken)
        {
            Sprint sprint = unitOfWork.SprintRepository.Get(request.SprintId);

            List<DateTime> workDays = sprint.CalculateWorkDays().ToList();

            List<SprintMember> sprintMembers = unitOfWork.TeamMemberRepository.GetAll()
                .Select(x => x.ToSprintMember(sprint))
                .ToList();

            int totalWorkHours = sprintMembers
                .SelectMany(x => x.DayInfo.Select(z => z.WorkHours))
                .Sum();

            float velocity = (float)sprint.StoryPoints / totalWorkHours;

            SprintVelocityResponse response = new()
            {
                SprintName = sprint.Name,
                WorkDays = workDays,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                SprintMembers = sprintMembers,
                TotalWorkHours = totalWorkHours,
                TotalStoryPoints = sprint.StoryPoints,
                Velocity = velocity
            };

            return Task.FromResult(response);
        }
    }
}