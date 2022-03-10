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
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentTeam
{
    public class PresentTeamCommand : ICliCommand
    {
        private readonly PresentTeamView view;
        private readonly IMediator mediator;

        public PresentTeamCommand(PresentTeamView view, IMediator mediator)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute(string[] args)
        {
            PresentTeamRequest request = new()
            {
                Date = GetDate(args),
                StartDate = GetStartDate(args),
                EndDate = GetEndDate(args)
            };
            PresentTeamResponse response = await mediator.Send(request);

            view.Display(response);
        }

        private static DateTime? GetDate(IReadOnlyList<string> args)
        {
            for (int i = 0; i < args.Count; i++)
            {
                if (args[i] != "-date")
                    continue;

                if (args.Count == i + 1)
                    return null;

                string rawValue = args[i + 1];

                bool isSuccess = DateTime.TryParse(rawValue, out DateTime value);

                return isSuccess ? value : null;
            }

            return null;
        }

        private static DateTime? GetStartDate(IReadOnlyList<string> args)
        {
            for (int i = 0; i < args.Count; i++)
            {
                if (args[i] != "-start-date")
                    continue;

                if (args.Count == i + 1)
                    return null;

                string rawValue = args[i + 1];

                bool isSuccess = DateTime.TryParse(rawValue, out DateTime value);

                return isSuccess ? value : null;
            }

            return null;
        }

        private static DateTime? GetEndDate(IReadOnlyList<string> args)
        {
            for (int i = 0; i < args.Count; i++)
            {
                if (args[i] != "-end-date")
                    continue;

                if (args.Count == i + 1)
                    return null;

                string rawValue = args[i + 1];

                bool isSuccess = DateTime.TryParse(rawValue, out DateTime value);

                return isSuccess ? value : null;
            }

            return null;
        }
    }
}