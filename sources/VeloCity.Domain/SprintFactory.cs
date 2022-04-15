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
using DustInTheWind.VeloCity.Domain.DataAccess;

namespace DustInTheWind.VeloCity.Domain
{
    public class SprintFactory
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReadOnlyCollection<string> excludedTeamMembers;

        public SprintFactory(IUnitOfWork unitOfWork, IReadOnlyCollection<string> excludedTeamMembers)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.excludedTeamMembers = excludedTeamMembers;
        }

        public Sprint GenerateImaginarySprint(DateTime startDate, DateTime endDate)
        {
            Sprint sprint = new()
            {
                EndDate = endDate,
                StartDate = startDate
            };

            IEnumerable<OfficialHoliday> officialHolidays = unitOfWork.OfficialHolidayRepository.GetAll();
            sprint.OfficialHolidays.AddRange(officialHolidays);

            RetrieveSprintMembersFor(sprint);

            return sprint;
        }

        public List<Sprint> GetExistingSprints(DateTime startDate, DateTime endDate)
        {
            List<Sprint> sprints = unitOfWork.SprintRepository.Get(startDate, endDate).ToList();

            foreach (Sprint futureSprint in sprints)
            {
                IEnumerable<OfficialHoliday> officialHolidays = unitOfWork.OfficialHolidayRepository.GetAll();
                futureSprint.OfficialHolidays.AddRange(officialHolidays);

                RetrieveSprintMembersFor(futureSprint);
            }

            return sprints;
        }

        public Sprint GetCurrentSprint()
        {
            Sprint currentSprint = unitOfWork.SprintRepository.GetLastInProgress();

            if (currentSprint == null)
                currentSprint = unitOfWork.SprintRepository.GetLast();

            if (currentSprint == null)
                throw new NoSprintException();

            RetrieveSprintMembersFor(currentSprint);

            return currentSprint;
        }

        public List<Sprint> GetLastClosed(int count, List<int> excludedSprints)
        {
            bool excludedSprintsExists = excludedSprints is { Count: > 0 };

            List<Sprint> sprints = excludedSprintsExists
                ? unitOfWork.SprintRepository.GetLastClosed(count, excludedSprints).ToList()
                : unitOfWork.SprintRepository.GetLastClosed(count).ToList();

            foreach (Sprint sprint in sprints)
                RetrieveSprintMembersFor(sprint);

            return sprints;
        }

        private void RetrieveSprintMembersFor(Sprint sprint)
        {
            IEnumerable<TeamMember> teamMembers = unitOfWork.TeamMemberRepository.GetAll();

            if (excludedTeamMembers is { Count: > 0 })
                teamMembers = teamMembers.Where(x => !excludedTeamMembers.Any(z => x.Name.Contains(z)));

            foreach (TeamMember teamMember in teamMembers)
                sprint.AddSprintMember(teamMember);
        }
    }
}