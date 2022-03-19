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
using DustInTheWind.VeloCity.Application.PresentSprints;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentSprints
{
    [Command("sprints", ShortDescription = "Displays an overview of the last sprints.", Order = 2)]
    [CommandUsage("sprints")]
    [CommandUsage("sprints [sprint-count]")]
    public class PresentSprintsCommand : ICommand
    {
        private readonly IMediator mediator;

        public List<SprintOverview> SprintOverviews { get; private set; }

        public PresentSprintsCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute(Arguments arguments)
        {
            PresentSprintsRequest request = new()
            {
                Count = GetSprintCount(arguments)
            };

            PresentSprintsResponse response = await mediator.Send(request);

            SprintOverviews = response.SprintOverviews;
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