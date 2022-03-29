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
        private readonly IConfig config;

        public AnalyzeSprintUseCase(IUnitOfWork unitOfWork, IConfig config)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task<AnalyzeSprintResponse> Handle(AnalyzeSprintRequest request, CancellationToken cancellationToken)
        {
            Sprint currentSprint = request.SprintNumber == null
                ? RetrieveDefaultSprintToAnalyze(request.ExcludedTeamMembers)
                : RetrieveSpecificSprintToAnalyze(request.SprintNumber.Value, request.ExcludedTeamMembers);

            List<Sprint> previousSprints = RetrievePreviousSprints(currentSprint.Number, config.AnalysisLookBack, request.ExcludedSprints, request.ExcludedTeamMembers);
            Velocity estimatedVelocity = CalculateAverageVelocity(previousSprints);

            int totalWorkHours = currentSprint.CalculateTotalWorkHours();
            List<VelocityPenaltyInstance> velocityPenalties = currentSprint.GetVelocityPenalties();
            int? totalWorkHoursWithVelocityPenalties = currentSprint.CalculateTotalWorkHoursWithVelocityPenalties();

            AnalyzeSprintResponse response = new()
            {
                SprintName = currentSprint.Name,
                SprintState = currentSprint.State,
                StartDate = currentSprint.StartDate,
                EndDate = currentSprint.EndDate,
                SprintDays = currentSprint.EnumerateAllDays().ToList(),
                SprintMembers = currentSprint.SprintMembers
                    .OrderBy(x =>
                    {
                        Employment employment = x.TeamMember.Employments.GetLastEmployment();
                        return employment.TimeInterval.StartDate;
                    })
                    .ThenBy(x => x.Name)
                    .ToList(),
                TotalWorkHours = totalWorkHours,
                EstimatedStoryPoints = estimatedVelocity.IsNull
                    ? StoryPoints.Null
                    : totalWorkHours * estimatedVelocity,
                EstimatedStoryPointsWithVelocityPenalties = estimatedVelocity.IsNull || !velocityPenalties.Any()
                    ? StoryPoints.Null
                    : totalWorkHoursWithVelocityPenalties * estimatedVelocity,
                EstimatedVelocity = estimatedVelocity,
                VelocityPenalties = velocityPenalties
                    .Select(x => new VelocityPenaltyInfo
                    {
                        PersonName = x.TeamMember?.Name,
                        PenaltyValue = x.Value
                    })
                    .ToList(),
                CommitmentStoryPoints = currentSprint.CommitmentStoryPoints,
                ActualStoryPoints = currentSprint.ActualStoryPoints,
                ActualVelocity = currentSprint.ActualStoryPoints / totalWorkHours,
                LookBackSprintCount = config.AnalysisLookBack,
                PreviousSprints = previousSprints
                    .Select(x => x.Number)
                    .ToList(),
                ExcludedSprints = request.ExcludedSprints?.ToList(),
                ShowTeam = request.ShowTeam
            };

            return Task.FromResult(response);
        }

        private Sprint RetrieveDefaultSprintToAnalyze(IReadOnlyCollection<string> excludedTeamMembers)
        {
            Sprint currentSprint = unitOfWork.SprintRepository.GetLastInProgress();

            if (currentSprint == null)
                currentSprint = unitOfWork.SprintRepository.GetLast();

            if (currentSprint == null)
                throw new NoSprintException();

            RetrieveSprintMembersFor(currentSprint, excludedTeamMembers);

            return currentSprint;
        }

        private Sprint RetrieveSpecificSprintToAnalyze(int sprintNumber, IReadOnlyCollection<string> excludedTeamMembers)
        {
            Sprint currentSprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            if (currentSprint == null)
                throw new SprintDoesNotExistException(sprintNumber);

            RetrieveSprintMembersFor(currentSprint, excludedTeamMembers);

            return currentSprint;
        }

        private List<Sprint> RetrievePreviousSprints(int sprintNumber, int count, List<int> excludedSprints, IReadOnlyCollection<string> excludedTeamMembers)
        {
            bool excludedSprintsExists = excludedSprints is { Count: > 0 };

            List<Sprint> sprints = excludedSprintsExists
                ? unitOfWork.SprintRepository.GetClosedSprintsBefore(sprintNumber, count, excludedSprints).ToList()
                : unitOfWork.SprintRepository.GetClosedSprintsBefore(sprintNumber, count).ToList();

            foreach (Sprint sprint in sprints)
                RetrieveSprintMembersFor(sprint, excludedTeamMembers);

            return sprints;
        }

        private void RetrieveSprintMembersFor(Sprint sprint, IReadOnlyCollection<string> excludedTeamMembers)
        {
            IEnumerable<TeamMember> teamMembers = unitOfWork.TeamMemberRepository.GetAll();

            if (excludedTeamMembers is { Count: > 0 })
                teamMembers = teamMembers.Where(x => !excludedTeamMembers.Any(z => x.Name.Contains(z)));

            foreach (TeamMember teamMember in teamMembers)
                sprint.AddSprintMember(teamMember);
        }

        private static Velocity CalculateAverageVelocity(IEnumerable<Sprint> previousSprints)
        {
            IEnumerable<Velocity> previousVelocities = previousSprints
                .Select(x => x.CalculateVelocity());

            if (!previousVelocities.Any())
                return Velocity.Null;

            return previousVelocities
                .Select(x => x.Value)
                .Average();
        }
    }
}