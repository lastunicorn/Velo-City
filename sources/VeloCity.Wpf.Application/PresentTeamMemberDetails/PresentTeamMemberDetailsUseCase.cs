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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberDetails
{
    internal class PresentTeamMemberDetailsUseCase : IRequestHandler<PresentTeamMemberDetailsRequest, PresentTeamMemberDetailsResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;

        public PresentTeamMemberDetailsUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        }

        public Task<PresentTeamMemberDetailsResponse> Handle(PresentTeamMemberDetailsRequest request, CancellationToken cancellationToken)
        {
            TeamMember currentTeamMember = RetrieveCurrentTeamMember();
            PresentTeamMemberDetailsResponse response = BuildResponse(currentTeamMember);

            return Task.FromResult(response);
        }

        private TeamMember RetrieveCurrentTeamMember()
        {
            if (applicationState.SelectedTeamMemberId != null)
            {
                int teamMemberId = applicationState.SelectedTeamMemberId.Value;
                return unitOfWork.TeamMemberRepository.Get(teamMemberId);
            }
            else
            {
                return null;
            }
        }

        private static PresentTeamMemberDetailsResponse BuildResponse(TeamMember currentTeamMember)
        {
            PresentTeamMemberDetailsResponse response = new();

            if (currentTeamMember != null)
                response.TeamMemberName = currentTeamMember.Name;

            return response;
        }
    }
}