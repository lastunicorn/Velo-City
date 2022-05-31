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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Application.PresentTeam
{
    internal class PresentTeamUseCase : IRequestHandler<PresentTeamRequest, PresentTeamResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentTeamUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentTeamResponse> Handle(PresentTeamRequest request, CancellationToken cancellationToken)
        {
            PresentTeamResponse response = CreateResponse(request);
            return Task.FromResult(response);
        }

        private PresentTeamResponse CreateResponse(PresentTeamRequest request)
        {
            if (request.Date != null)
                return CreateResponseForDate(request.Date.Value);

            if (request.DateInterval != null)
                return CreateResponseForDateInterval(request.DateInterval.Value);

            if (request.SprintNumber != null)
                return CreateResponseForSprint(request.SprintNumber.Value);

            return CreateResponseForCurrentSprint();
        }

        private PresentTeamResponse CreateResponseForDate(DateTime date)
        {
            return new PresentTeamResponse
            {
                TeamMembers = TeamMembersExtensions.OrderByEmploymentForDate(unitOfWork.TeamMemberRepository.GetByDate(date), date)
                    .ToList(),
                ResponseType = TeamResponseType.Date,
                Date = date
            };
        }

        private PresentTeamResponse CreateResponseForDateInterval(DateInterval dateInterval)
        {
            return new PresentTeamResponse
            {
                TeamMembers = TeamMembersExtensions.OrderByEmploymentForDate(unitOfWork.TeamMemberRepository.GetByDateInterval(dateInterval), dateInterval.StartDate)
                    .ToList(),
                ResponseType = TeamResponseType.DateInterval,
                DateInterval = dateInterval
            };
        }

        private PresentTeamResponse CreateResponseForSprint(int sprintNumber)
        {
            Sprint sprint = RetrieveSprint(sprintNumber);
            return CreateResponseForSprint(sprint);
        }

        private Sprint RetrieveSprint(int sprintNumber)
        {
            Sprint sprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            if (sprint == null)
                throw new SprintDoesNotExistException(sprintNumber);

            return sprint;
        }

        private PresentTeamResponse CreateResponseForCurrentSprint()
        {
            Sprint currentSprint = RetrieveCurrentSprint();
            return CreateResponseForSprint(currentSprint);
        }

        private Sprint RetrieveCurrentSprint()
        {
            Sprint currentSprint = unitOfWork.SprintRepository.GetLastInProgress();

            if (currentSprint == null)
                throw new NoSprintException();

            return currentSprint;
        }

        private PresentTeamResponse CreateResponseForSprint(Sprint sprint)
        {
            return new PresentTeamResponse
            {
                TeamMembers = unitOfWork.TeamMemberRepository.GetByDateInterval(sprint.DateInterval)
                    .OrderByEmploymentForDate(sprint.StartDate)
                    .ToList(),
                ResponseType = TeamResponseType.Sprint,
                SprintNumber = sprint.Number,
                DateInterval = sprint.DateInterval
            };
        }
    }
}