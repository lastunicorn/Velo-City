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
    [Command("vacations", ShortDescription = "The team member's vacation days.", Order = 5)]
    public class VacationsCommand : ICommand
    {
        private readonly IMediator mediator;

        [CommandParameter(Name = "name", ShortName = 'n', Order = 1, IsOptional = true)]
        public string PersonName { get; set; }

        public List<TeamMemberVacationViewModel> TeamMemberVacations { get; private set; }

        public VacationsCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            PresentVacationsRequest request = new()
            {
                TeamMemberName = PersonName
            };

            PresentVacationsResponse response = await mediator.Send(request);

            TeamMemberVacations = response.TeamMemberVacations
                .Select(x => new TeamMemberVacationViewModel(x))
                .ToList();
        }
    }
}