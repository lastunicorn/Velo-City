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
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    public class AnalyzeSprintCommand : ICliCommand
    {
        private readonly AnalyzeSprintView view;
        private readonly IMediator mediator;

        public AnalyzeSprintCommand(AnalyzeSprintView view, IMediator mediator)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute(string[] args)
        {
            AnalyzeSprintRequest request = new()
            {
                SprintNumber = GetSprintNumber(args),
                ExcludedSprints = GetExcludedSprintsList(args)
            };

            AnalyzeSprintResponse response = await mediator.Send(request);

            view.Display(response);
        }

        private static int? GetSprintNumber(IReadOnlyList<string> args)
        {
            return args.Count > 1
                ? int.Parse(args[1])
                : null;
        }

        private static List<int> GetExcludedSprintsList(IReadOnlyList<string> args)
        {
            if (args.Count > 2)
            {
                if (args[2] == "-exclude")
                {
                    string rawValue = args[3];
                    List<int> excludedSprintNumbers = rawValue.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();
                    return excludedSprintNumbers;
                }
            }

            return null;
        }
    }
}