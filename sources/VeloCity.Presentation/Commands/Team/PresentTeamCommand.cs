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
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Application.PresentTeam;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.Team
{
    [Command("team", ShortDescription = "Displays the composition of the team (team members).", Order = 4)]
    [CommandUsage("team")]
    [CommandUsage("team -date [date]")]
    [CommandUsage("team -start-date [date] -end-date [date]")]
    public class PresentTeamCommand : ICommand
    {
        private readonly IMediator mediator;

        [CommandParameter(Name = "date", IsOptional = true)]
        public DateTime? Date { get; set; }

        [CommandParameter(Name = "start-date", IsOptional = true)]
        public DateTime? StartDate { get; set; }

        [CommandParameter(Name = "end-date", IsOptional = true)]
        public DateTime? EndDate { get; set; }

        [CommandParameter(Name = "sprint", IsOptional = true)]
        public int? Sprint { get; set; }

        public List<TeamMember> TeamMembers { get; private set; }

        public TeamResponseTypeViewModel TeamResponseType { get; private set; }

        public PresentTeamCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            PresentTeamRequest request = new()
            {
                Date = Date,
                StartDate = StartDate,
                EndDate = EndDate,
                SprintNumber = Sprint
            };
            PresentTeamResponse response = await mediator.Send(request);

            TeamMembers = response.TeamMembers;
            TeamResponseType = new TeamResponseTypeViewModel(response);
        }
    }
}