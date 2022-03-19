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
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Application.PresentVacations;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.Vacations
{
    [Command("vacations", ShortDescription = "Displays the vacation days for the specified team member.", Order = 3)]
    [CommandUsage("vacations [person-name]")]
    public class VacationsCommand : ICommand
    {
        private readonly IMediator mediator;

        public List<TeamMemberVacationViewModel> TeamMemberVacations { get; private set; }

        public VacationsCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute(Arguments arguments)
        {
            PresentVacationsRequest request = new()
            {
                TeamMemberName = GetPersonName(arguments)
            };

            PresentVacationsResponse response = await mediator.Send(request);

            TeamMemberVacations = response.TeamMemberVacations
                .Select(x=> new TeamMemberVacationViewModel(x))
                .ToList();
        }

        private static string GetPersonName(Arguments arguments)
        {
            Argument argument = arguments.GetOrdinal(1);
            return argument?.Value;
        }
    }
}