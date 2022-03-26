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

namespace DustInTheWind.VeloCity.Application.PresentVacations
{
    internal class PresentVacationsUseCase : IRequestHandler<PresentVacationsRequest, PresentVacationsResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentVacationsUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentVacationsResponse> Handle(PresentVacationsRequest request, CancellationToken cancellationToken)
        {
            if (request.TeamMemberName == null)
                throw new TeamMemberNameRequiredException();

            List<TeamMemberVacation> teamMemberVacations = unitOfWork.TeamMemberRepository.Find(request.TeamMemberName)
                .Select(ToTeamMemberVacation)
                .ToList();

            if (teamMemberVacations.Count == 0)
                throw new TeamMemberNotFoundException(request.TeamMemberName);

            PresentVacationsResponse response = new()
            {
                TeamMemberVacations = teamMemberVacations
            };

            return Task.FromResult(response);
        }

        private static TeamMemberVacation ToTeamMemberVacation(TeamMember teamMember)
        {
            return new TeamMemberVacation
            {
                PersonName = teamMember.Name,
                Vacations = teamMember.Vacations
            };
        }
    }
}