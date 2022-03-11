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
            PresentTeamResponse response = new()
            {
                TeamMembers = RetrieveTeamMembers(request).ToList()
            };

            return Task.FromResult(response);
        }

        private IEnumerable<TeamMember> RetrieveTeamMembers(PresentTeamRequest request)
        {
            if (request.Date != null)
                return unitOfWork.TeamMemberRepository.GetByDate(request.Date.Value);

            if (request.StartDate != null || request.EndDate != null)
                return unitOfWork.TeamMemberRepository.GetByDateInterval(request.StartDate, request.EndDate);

            Sprint currentSprint = unitOfWork.SprintRepository.GetLast();
            return unitOfWork.TeamMemberRepository.GetByDateInterval(currentSprint.StartDate, currentSprint.EndDate);
        }
    }
}