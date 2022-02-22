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
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Application.SprintVelocity;
using MediatR;

namespace DustInTheWind.VeloCity.SprintVelocity
{
    internal class SprintVelocityCommand
    {
        private readonly SprintVelocityView view;
        private readonly IMediator mediator;

        public SprintVelocityCommand(SprintVelocityView view, IMediator mediator)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            SprintVelocityRequest request = new()
            {
                SprintId = 24
            };

            SprintVelocityResponse sprintVelocityResponse = await mediator.Send(request);

            view.DisplaySprintVelocity(sprintVelocityResponse);
        }
    }
}