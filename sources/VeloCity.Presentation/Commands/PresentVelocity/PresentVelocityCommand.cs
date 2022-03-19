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
using DustInTheWind.VeloCity.Application.PresentVelocity;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentVelocity
{
    [Command("velocity", Enabled = false)]
    public class PresentVelocityCommand : ICommand
    {
        private readonly IMediator mediator;

        public List<SprintVelocity> SprintVelocities { get; private set; }

        public PresentVelocityCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute(Arguments arguments)
        {
            PresentVelocityRequest request = new()
            {
                Count = GetSprintCount(arguments)
            };

            PresentVelocityResponse response = await mediator.Send(request);

            SprintVelocities = response.SprintVelocities;
        }

        private static int? GetSprintCount(Arguments arguments)
        {
            Argument argument = arguments[1];

            return argument == null
                ? null
                : int.Parse(argument.Value);
        }
    }
}