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
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Application.PresentVacations;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Presentation.Commands.Vacations
{
    public class VacationsView : IView<VacationsCommand>
    {
        private readonly DataGridFactory dataGridFactory;

        public VacationsView(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        public void Display(VacationsCommand command)
        {
            switch (command.RequestType)
            {
                case RequestType.ByName:
                    if (command.TeamMemberVacations.Count == 0)
                    {
                        CustomConsole.WriteLineWarning($"No team member was found with name '{command.PersonName}'.");
                        return;
                    }
                    else
                    {
                        CustomConsole.WriteLine($"Vacations for team member '{command.PersonName}':");
                    }
                    break;

                case RequestType.ByCurrentDate:
                    if (command.TeamMemberVacations.Count == 0)
                    {
                        CustomConsole.WriteLineWarning($"No team member is active at date {command.Date:yyyy MM dd}.");
                        return;
                    }
                    else
                    {
                        CustomConsole.WriteLine($"Vacations for all the team members active at {command.Date:yyyy MM dd}:");
                    }
                    break;
            }

            DisplayVacations(command.TeamMemberVacations);
        }

        private void DisplayVacations(List<TeamMemberVacationViewModel> teamMemberVacations)
        {
            foreach (TeamMemberVacationViewModel teamMemberVacation in teamMemberVacations)
            {
                DataGrid dataGrid = dataGridFactory.Create();
                dataGrid.Title = teamMemberVacation.PersonName.ToString();
                dataGrid.Border.DisplayBorderBetweenRows = true;

                IEnumerable<ContentRow> rows = teamMemberVacation.MonthsOfVacations.Select(ToRow);

                foreach (ContentRow row in rows)
                    dataGrid.Rows.Add(row);

                dataGrid.Display();
            }
        }

        private static ContentRow ToRow(MonthOfVacationsViewModel monthOfVacations)
        {
            ContentRow row = new();
            row.AddCell($"{monthOfVacations.DateTimeMonth:short-name}");

            List<string> lines = monthOfVacations.Vacations
                .Select(x => x.ToString())
                .ToList();

            row.AddCell(lines);

            return row;
        }
    }
}