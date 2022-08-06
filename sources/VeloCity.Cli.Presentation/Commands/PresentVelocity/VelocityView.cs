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
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Application.PresentVelocity;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.PresentVelocity
{
    public class VelocityView : IView<VelocityCommand>
    {
        public void Display(VelocityCommand command)
        {
            bool sprintsExist = command.SprintVelocities is { Count: > 0 };

            if (sprintsExist)
                DisplaySprints(command.SprintVelocities);
            else
                Console.WriteLine("There are no sprints.");
        }

        private static void DisplaySprints(IReadOnlyCollection<SprintVelocity> sprintVelocities)
        {
            Console.WriteLine();

            VelocityChartControl velocityChartControl = new()
            {
                Items = sprintVelocities
                    .Select(x => new VelocityChartItem
                    {
                        SprintNumber = x.SprintNumber,
                        Velocity = x.Velocity
                    })
                    .ToList()
            };

            velocityChartControl.Display();
        }
    }
}