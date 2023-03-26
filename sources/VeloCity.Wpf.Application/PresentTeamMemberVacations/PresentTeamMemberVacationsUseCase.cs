// VeloCity
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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberVacations
{
    internal class PresentTeamMemberVacationsUseCase : IRequestHandler<PresentTeamMemberVacationsRequest, PresentTeamMemberVacationsResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;

        public PresentTeamMemberVacationsUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        }

        public Task<PresentTeamMemberVacationsResponse> Handle(PresentTeamMemberVacationsRequest request, CancellationToken cancellationToken)
        {
            PresentTeamMemberVacationsResponse response = new()
            {
                Vacations = new List<VacationInfo>()
            };

            if (applicationState.SelectedTeamMemberId != null)
            {
                int currentTeamMemberId = applicationState.SelectedTeamMemberId.Value;
                TeamMember teamMember = unitOfWork.TeamMemberRepository.Get(currentTeamMemberId);

                if (teamMember?.Vacations != null)
                {
                    IEnumerable<VacationInfo> vacationInfos = teamMember.Vacations
                        .Select(VacationInfo.From);

                    response.Vacations.AddRange(vacationInfos);
                }
            }

            return Task.FromResult(response);
        }
    }
}