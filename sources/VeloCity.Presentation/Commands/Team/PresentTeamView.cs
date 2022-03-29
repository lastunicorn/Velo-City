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
using System.Text;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Presentation.Commands.Team
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
            Console.WriteLine(command.Information);

            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = "Team";

            foreach (TeamMember teamMember in command.TeamMembers)
            {
                IEnumerable<string> employmentsAsString = teamMember.Employments
                    .Select(RenderEmployment);

                string employmentsCellContent = string.Join(Environment.NewLine, employmentsAsString);
                dataGrid.Rows.Add(teamMember.Name.FullNameWithNickname, employmentsCellContent);
            }

            dataGrid.Display();
        }

        private static string RenderEmployment(Employment employment)
        {
            StringBuilder sb = new();

            sb.Append($"{employment.HoursPerDay} h/day");

            if (employment.WeekDays is { Count: > 0 })
            {
                if (sb.Length > 0)
                    sb.Append(" | ");

                sb.Append(string.Join(", ", employment.WeekDays));
            }

            if (sb.Length > 0)
                sb.Append(" | ");

            sb.Append(employment.TimeInterval.ToString());

            return sb.ToString();
        }
    }
}