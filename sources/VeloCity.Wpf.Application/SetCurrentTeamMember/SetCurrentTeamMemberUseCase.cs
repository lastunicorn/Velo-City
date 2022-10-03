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
using DustInTheWind.VeloCity.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.SetCurrentTeamMember
{
    internal class SetCurrentTeamMemberUseCase : IRequestHandler<SetCurrentTeamMemberRequest>
    {
        private readonly ApplicationState applicationState;
        private readonly EventBus eventBus;

        public SetCurrentTeamMemberUseCase(ApplicationState applicationState, EventBus eventBus)
        {
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<Unit> Handle(SetCurrentTeamMemberRequest request, CancellationToken cancellationToken)
        {
            applicationState.SelectedTeamMemberId = request.TeamMemberId;

            TeamMemberChangedEvent ev = new()
            {
                NewTeamMemberId = request.TeamMemberId
            };

            await eventBus.Publish(ev, cancellationToken);

            return Unit.Value;
        }
    }
}