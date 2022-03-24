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
                ? RetrieveDefaultSprintToAnalyze()
                : RetrieveSprintToAnalyze(request.SprintNumber.Value);

            List<SprintMember> sprintMembers = RetrieveSprintMembers(currentSprint);

            List<Sprint> previousSprints = RetrievePreviousSprints(currentSprint.Number, request.LookBackSprintCount, request.ExcludedSprints);
            float? estimatedVelocity = previousSprints.Count == 0
                ? null
                : CalculateAverageVelocity(previousSprints);

            int totalWorkHours = CalculateTotalWorkHours(sprintMembers);
            int totalWorkHoursWithVelocityPenalties = CalculateTotalWorkHoursWithVelocityPenalties(sprintMembers); ;

            List<VelocityPenaltyInfo> teamMembersWithVelocityPenalties = sprintMembers
                .Where(x => x.TeamMember?.VelocityPenalties?.Count > 0)
                .Select(x => new VelocityPenaltyInfo
                {
                    PersonName = x.TeamMember?.Name,
                    PenaltyValue = x.VelocityPenalty
                })
                .ToList();

            AnalyzeSprintResponse response = new()
            {
                SprintName = currentSprint.Name,
                SprintState = currentSprint.State,
                WorkDays = currentSprint.EnumerateWorkDays().ToList(),
                StartDate = currentSprint.StartDate,
                EndDate = currentSprint.EndDate,
                SprintMembers = sprintMembers,
                TotalWorkHours = totalWorkHours,
                ActualStoryPoints = currentSprint.ActualStoryPoints,
                ActualVelocity = (float)currentSprint.ActualStoryPoints / totalWorkHours,
                CommitmentStoryPoints = currentSprint.CommitmentStoryPoints,
                EstimatedStoryPoints = totalWorkHours * estimatedVelocity,
                EstimatedStoryPointsWithVelocityPenalties = totalWorkHoursWithVelocityPenalties * estimatedVelocity,
                EstimatedVelocity = estimatedVelocity,
                VelocityPenalties = teamMembersWithVelocityPenalties,
                LookBackSprintCount = request.LookBackSprintCount,
                PreviousSprints = previousSprints.Select(x => x.Number).ToList(),
                ExcludesSprints = request.ExcludedSprints?.ToList()
            };

            return Task.FromResult(response);
        }

        private static int CalculateTotalWorkHours(IEnumerable<SprintMember> sprintMembers)
        {
            return sprintMembers
                .Select(x => x.WorkHours)
                .Sum();
        }

        private static int CalculateTotalWorkHoursWithVelocityPenalties(IEnumerable<SprintMember> sprintMembers)
        {
            return sprintMembers
                .Select(x => x.WorkHoursWithVelocityPenalties)
                .Sum();
        }

        private List<SprintMember> RetrieveSprintMembers(Sprint currentSprint)
        {
            return unitOfWork.TeamMemberRepository.GetAll()
                .Select(x => x.ToSprintMember(currentSprint))
                .Where(x => x.IsEmployed)
                .ToList();
        }

        private Sprint RetrieveDefaultSprintToAnalyze()
        {
            Sprint currentSprint = unitOfWork.SprintRepository.GetLastInProgress();

            if (currentSprint == null)
                currentSprint = unitOfWork.SprintRepository.GetLast();

            if (currentSprint == null)
                throw new NoSprintException();

            return currentSprint;
        }

        private Sprint RetrieveSprintToAnalyze(int sprintNumber)
        {
            Sprint currentSprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            if (currentSprint == null)
                throw new SprintDoesNotExistException(sprintNumber);

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
            List<TeamMember> allTeamMembers = unitOfWork.TeamMemberRepository.GetAll()
                .ToList();

            IEnumerable<float> previousVelocities = previousSprints
                .Select(x => CalculateAverageVelocity(x, allTeamMembers));

            float averageVelocity = previousVelocities.Any()
                ? previousVelocities.Average()
                : 0;

            return averageVelocity;
        }

        private static float CalculateAverageVelocity(Sprint sprint, IEnumerable<TeamMember> allTeamMembers)
        {
            int totalWorkHours = allTeamMembers
                .Select(x => x.ToSprintMember(sprint).CalculateWorkHours())
                .Sum();

            return (float)sprint.ActualStoryPoints / totalWorkHours;
        }
    }
}