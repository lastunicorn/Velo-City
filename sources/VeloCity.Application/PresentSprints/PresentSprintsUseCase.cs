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

namespace DustInTheWind.VeloCity.Application.PresentSprints
{
    internal class PresentSprintsUseCase : IRequestHandler<PresentSprintsRequest, PresentSprintsResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentSprintsUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentSprintsResponse> Handle(PresentSprintsRequest request, CancellationToken cancellationToken)
        {
            int sprintCount = request.Count is null or < 1
                ? 6
                : request.Count.Value;

            List<SprintOverview> sprintOverviews = unitOfWork.SprintRepository.GetPage(0, sprintCount)
                .Select(ExtractSprintOverview)
                .ToList();

            PresentSprintsResponse response = new()
            {
                SprintOverviews = sprintOverviews
            };

            return Task.FromResult(response);
        }

        private SprintOverview ExtractSprintOverview(Sprint sprint)
        {
            List<SprintMember> sprintMembers = unitOfWork.TeamMemberRepository.GetAll()
                .Select(x => x.ToSprintMember(sprint))
                .ToList();

            int totalWorkHours = sprintMembers
                .SelectMany(x => x.Days.Select(z => z.WorkHours))
                .Sum();

            float velocity = (float)sprint.ActualStoryPoints / totalWorkHours;

            float averageVelocity = unitOfWork.SprintRepository.GetBefore(sprint.Number, 6).ToList()
                .Select(x =>
                {
                    int totalWorkHours = unitOfWork.TeamMemberRepository.GetAll()
                        .Select(z => z.CalculateWorkHoursFor(x))
                        .Sum();

                    return (float)x.ActualStoryPoints / totalWorkHours;
                })
                .Average();

            return new SprintOverview
            {
                Name = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                TotalWorkHours = totalWorkHours,
                ActualStoryPoints = sprint.ActualStoryPoints,
                ActualVelocity = velocity,
                CommitmentStoryPoints = sprint.CommitmentStoryPoints,
                EstimatedStoryPoints = totalWorkHours * averageVelocity,
                EstimatedVelocity = averageVelocity
            };
        }
    }
}