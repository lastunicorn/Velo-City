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

namespace DustInTheWind.VeloCity.Application.PresentSprintCalendar
{
    internal class PresentSprintCalendarUseCase : IRequestHandler<PresentSprintCalendarRequest, PresentSprintCalendarResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentSprintCalendarUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentSprintCalendarResponse> Handle(PresentSprintCalendarRequest request, CancellationToken cancellationToken)
        {
            Sprint sprint = RetrieveSprint(request.SprintNumber);

            PresentSprintCalendarResponse response = new()
            {
                SprintName = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Days = sprint.EnumerateAllDays().ToList(),
                SprintMembers = sprint.SprintMembersOrderedByEmployment.ToList()
            };

            return Task.FromResult(response);
        }

        private Sprint RetrieveSprint(int? sprintNumber)
        {
            if (sprintNumber == null)
            {
                Sprint sprint = unitOfWork.SprintRepository.GetLastInProgress();

                if (sprint == null)
                    throw new NoSprintException();

                RetrieveSprintMembersFor(sprint, null);

                return sprint;
            }
            else
            {
                Sprint sprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber.Value);

                if (sprint == null)
                    throw new SprintDoesNotExistException(sprintNumber.Value);

                RetrieveSprintMembersFor(sprint, null);

                return sprint;
            }
        }

        private void RetrieveSprintMembersFor(Sprint sprint, IReadOnlyCollection<string> excludedTeamMembers)
        {
            IEnumerable<TeamMember> teamMembers = unitOfWork.TeamMemberRepository.GetAll();

            if (excludedTeamMembers is { Count: > 0 })
                teamMembers = teamMembers.Where(x => !excludedTeamMembers.Any(z => x.Name.Contains(z)));

            foreach (TeamMember teamMember in teamMembers)
                sprint.AddSprintMember(teamMember);
        }
    }
}