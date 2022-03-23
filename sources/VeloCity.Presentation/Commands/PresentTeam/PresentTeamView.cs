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
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentTeam
{
    public class PresentTeamView : IView<PresentTeamCommand>
    {
        private readonly DataGridFactory dataGridFactory;

        public PresentTeamView(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        public void Display(PresentTeamCommand command)
        {
            Console.WriteLine(command.TeamResponseType);

            foreach (TeamMember teamMember in command.TeamMembers)
            {
                DataGrid dataGrid = dataGridFactory.Create();
                dataGrid.Title = teamMember.Name.ToString();
                dataGrid.Border.DisplayBorderBetweenRows = true;

                IEnumerable<string> employmentsAsString = teamMember.Employments
                    .Select(x => $"{x.HoursPerDay} h/day | {x.TimeInterval}");
                dataGrid.Rows.Add("Employment", string.Join(Environment.NewLine, employmentsAsString));

                if (!string.IsNullOrEmpty(teamMember.Comments))
                    dataGrid.Rows.Add("Comments", teamMember.Comments);

                dataGrid.Display();
            }
        }
    }
}