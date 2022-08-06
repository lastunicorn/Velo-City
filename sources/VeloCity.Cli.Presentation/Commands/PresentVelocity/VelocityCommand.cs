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
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Application.PresentVelocity;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.PresentVelocity
{
    [Command("velocity", ShortDescription = "A chart with the velocity per sprints.", Order = 3)]
    public class VelocityCommand : ICommand
    {
        private readonly IMediator mediator;

        [CommandParameter(Order = 1, IsOptional = true)]
        public int? SprintCount { get; set; }

        public List<SprintVelocity> SprintVelocities { get; private set; }

        public VelocityCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            PresentVelocityRequest request = new()
            {
                SprintCount = SprintCount
            };

            PresentVelocityResponse response = await mediator.Send(request);

            SprintVelocities = response.SprintVelocities;
        }
    }
}