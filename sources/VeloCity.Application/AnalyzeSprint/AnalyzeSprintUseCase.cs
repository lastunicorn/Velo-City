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

namespace DustInTheWind.VeloCity.Application.AnalyzeSprint
{
    internal class AnalyzeSprintUseCase : IRequestHandler<AnalyzeSprintRequest, AnalyzeSprintResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyzeSprintUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<AnalyzeSprintResponse> Handle(AnalyzeSprintRequest request, CancellationToken cancellationToken)
        {
            Sprint currentSprint = request.SprintNumber == null
                ? unitOfWork.SprintRepository.GetLast()
                : unitOfWork.SprintRepository.Get(request.SprintNumber.Value);

            List<DateTime> workDays = currentSprint.GetWorkDays()
                .Select(x => x.Date)
                .ToList();

            List<SprintMember> sprintMembers = unitOfWork.TeamMemberRepository.GetAll()
                .Select(x => x.ToSprintMember(currentSprint))
                .ToList();

            int totalWorkHours = sprintMembers
                .SelectMany(x => x.Days.Select(z => z.WorkHours))
                .Sum();

            float velocity = (float)currentSprint.ActualStoryPoints / totalWorkHours;

            float averageVelocity = unitOfWork.SprintRepository.GetBefore(currentSprint.Number, request.LookBackCount).ToList()
                .Select(x =>
                {
                    int totalWorkHours = unitOfWork.TeamMemberRepository.GetAll()
                        .Select(z => z.CalculateWorkHoursFor(x))
                        .Sum();

                    return (float)x.ActualStoryPoints / totalWorkHours;
                })
                .Average();

            AnalyzeSprintResponse response = new()
            {
                SprintName = currentSprint.Name,
                WorkDays = workDays,
                StartDate = currentSprint.StartDate,
                EndDate = currentSprint.EndDate,
                SprintMembers = sprintMembers,
                TotalWorkHours = totalWorkHours,
                ActualStoryPoints = currentSprint.ActualStoryPoints,
                ActualVelocity = velocity,
                CommitmentStoryPoints = currentSprint.CommitmentStoryPoints,
                EstimatedStoryPoints = totalWorkHours * averageVelocity,
                EstimatedVelocity = averageVelocity
            };

            return Task.FromResult(response);
        }
    }
}