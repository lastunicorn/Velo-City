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
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentTeam
{
    public class PresentTeamCommand : ICommand
    {
        private readonly IMediator mediator;

        public List<TeamMember> TeamMembers { get; set; }

        public PresentTeamCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute(Arguments arguments)
        {
            PresentTeamRequest request = new()
            {
                Date = GetDate(arguments),
                StartDate = GetStartDate(arguments),
                EndDate = GetEndDate(arguments)
            };
            PresentTeamResponse response = await mediator.Send(request);

            TeamMembers = response.TeamMembers;
        }

        private static DateTime? GetDate(Arguments arguments)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i] != "-date")
                    continue;

                if (arguments.Count == i + 1)
                    return null;

                string rawValue = arguments[i + 1];

                bool isSuccess = DateTime.TryParse(rawValue, out DateTime value);

                return isSuccess ? value : null;
            }

            return null;
        }

        private static DateTime? GetStartDate(Arguments arguments)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i] != "-start-date")
                    continue;

                if (arguments.Count == i + 1)
                    return null;

                string rawValue = arguments[i + 1];

                bool isSuccess = DateTime.TryParse(rawValue, out DateTime value);

                return isSuccess ? value : null;
            }

            return null;
        }

        private static DateTime? GetEndDate(Arguments arguments)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i] != "-end-date")
                    continue;

                if (arguments.Count == i + 1)
                    return null;

                string rawValue = arguments[i + 1];

                bool isSuccess = DateTime.TryParse(rawValue, out DateTime value);

                return isSuccess ? value : null;
            }

            return null;
        }
    }
}