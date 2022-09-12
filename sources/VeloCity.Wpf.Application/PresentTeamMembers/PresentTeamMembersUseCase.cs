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
using DustInTheWind.VeloCity.Wpf.Application.PresentTeam;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentTeamMembers
{
    internal class PresentTeamMembersUseCase : IRequestHandler<PresentTeamMembersRequest, PresentTeamMembersResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;

        public PresentTeamMembersUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        }

        public Task<PresentTeamMembersResponse> Handle(PresentTeamMembersRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<TeamMember> allTeamMembers = unitOfWork.TeamMemberRepository.GetAll();

            List<TeamMember> employedTeamMembers = new();
            List<TeamMember> unemployedTeamMembers = new();

            foreach (TeamMember teamMember in allTeamMembers)
            {
                if (teamMember.IsEmployed)
                    employedTeamMembers.Add(teamMember);
                else
                    unemployedTeamMembers.Add(teamMember);
            }

            IEnumerable<TeamMember> orderedEmployedTeamMembers = employedTeamMembers
                .OrderByEmployment();

            IEnumerable<TeamMember> orderedUnemployedTeamMembers = unemployedTeamMembers
                .OrderByDescending(x => x.Employments.GetLastEmployment().EndDate);

            PresentTeamMembersResponse response = new()
            {
                TeamMembers = orderedEmployedTeamMembers
                    .Concat(orderedUnemployedTeamMembers)
                    .Select(x => new TeamMemberInfo(x))
                    .ToList(),
                CurrentTeamMemberId = applicationState.SelectedTeamMemberId
            };

            return Task.FromResult(response);
        }
    }
}