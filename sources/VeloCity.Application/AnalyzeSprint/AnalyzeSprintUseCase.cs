﻿// Velo City
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
            Sprint currentSprint = RetrieveSprintToAnalyze(request.SprintNumber);
            List<SprintMember> sprintMembers = RetrieveSprintMembers(currentSprint);

            List<Sprint> previousSprints = RetrievePreviousSprints(currentSprint.Number, request.LookBackSprintCount, request.ExcludedSprints);
            float averageVelocity = CalculateAverageVelocity(previousSprints);

            int totalWorkHours = CalculateTotalWorkHours(sprintMembers);

            AnalyzeSprintResponse response = new()
            {
                SprintName = currentSprint.Name,
                WorkDays = currentSprint.EnumerateWorkDates().ToList(),
                StartDate = currentSprint.StartDate,
                EndDate = currentSprint.EndDate,
                SprintMembers = sprintMembers,
                TotalWorkHours = totalWorkHours,
                ActualStoryPoints = currentSprint.ActualStoryPoints,
                ActualVelocity = (float)currentSprint.ActualStoryPoints / totalWorkHours,
                CommitmentStoryPoints = currentSprint.CommitmentStoryPoints,
                EstimatedStoryPoints = totalWorkHours * averageVelocity,
                EstimatedVelocity = averageVelocity,
                LookBackSprintCount = request.LookBackSprintCount,
                PreviousSprints = previousSprints.Select(x => x.Number).ToList(),
                ExcludesSprints = request.ExcludedSprints?.ToList()
            };

            return Task.FromResult(response);
        }

        private static int CalculateTotalWorkHours(IEnumerable<SprintMember> sprintMembers)
        {
            return sprintMembers
                .SelectMany(x => x.Days.Select(z => z.WorkHours))
                .Sum();
        }

        private List<SprintMember> RetrieveSprintMembers(Sprint currentSprint)
        {
            List<SprintMember> sprintMembers = unitOfWork.TeamMemberRepository.GetAll()
                .Select(x => x.ToSprintMember(currentSprint))
                .Where(x => x.IsEmployed)
                .ToList();
            return sprintMembers;
        }

        private Sprint RetrieveSprintToAnalyze(int? sprintNumber)
        {
            Sprint currentSprint = sprintNumber == null
                ? unitOfWork.SprintRepository.GetLast()
                : unitOfWork.SprintRepository.Get(sprintNumber.Value);

            if (currentSprint == null)
            {
                if (sprintNumber == null)
                    throw new NoSprintException();

                throw new SprintDoesNotExistException(sprintNumber.Value);
            }

            return currentSprint;
        }

        private List<Sprint> RetrievePreviousSprints(int sprintNumber, int count, List<int> excludedSprints)
        {
            bool excludedSprintsExists = excludedSprints is { Count: > 0 };

            IEnumerable<Sprint> sprints = excludedSprintsExists
                ? unitOfWork.SprintRepository.GetClosedSprintsBefore(sprintNumber, count, excludedSprints)
                : unitOfWork.SprintRepository.GetClosedSprintsBefore(sprintNumber, count);
            
            return sprints.ToList();
        }

        private float CalculateAverageVelocity(IEnumerable<Sprint> previousSprints)
        {
            List<TeamMember> allTeamMembers = unitOfWork.TeamMemberRepository.GetAll().ToList();

            IEnumerable<float> previousVelocities = previousSprints
                .Select(x =>
                {
                    int totalWorkHours = allTeamMembers
                        .Select(z => z.CalculateWorkHoursFor(x))
                        .Sum();

                    return (float)x.ActualStoryPoints / totalWorkHours;
                });

            float averageVelocity = previousVelocities.Any()
                ? previousVelocities.Average()
                : 0;

            return averageVelocity;
        }
    }
}