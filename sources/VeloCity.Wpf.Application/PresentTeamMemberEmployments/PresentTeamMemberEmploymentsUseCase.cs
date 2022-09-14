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

namespace DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberEmployments
{
    internal class PresentTeamMemberEmploymentsUseCase : IRequestHandler<PresentTeamMemberEmploymentsRequest, PresentTeamMemberEmploymentsResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;

        public PresentTeamMemberEmploymentsUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        }

        public Task<PresentTeamMemberEmploymentsResponse> Handle(PresentTeamMemberEmploymentsRequest request, CancellationToken cancellationToken)
        {
            PresentTeamMemberEmploymentsResponse response = new()
            {
                Employments = new List<EmploymentInfo>()
            };

            if (applicationState.SelectedTeamMemberId != null)
            {
                int currentTeamMemberId = applicationState.SelectedTeamMemberId.Value;
                TeamMember teamMember = unitOfWork.TeamMemberRepository.Get(currentTeamMemberId);

                if (teamMember != null)
                {
                    IEnumerable<EmploymentInfo> employmentInfos = teamMember.Employments
                        .Select(x => new EmploymentInfo(x));

                    response.Employments.AddRange(employmentInfos);
                }
            }

            return Task.FromResult(response);
        }
    }
}