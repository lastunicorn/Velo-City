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
            foreach (TeamMemberVacationViewModel teamMemberVacation in command.TeamMemberVacations)
            {
                DataGrid dataGrid = dataGridFactory.Create();
                dataGrid.Title = teamMemberVacation.PersonName;
                dataGrid.Border.DisplayBorderBetweenRows = true;

                foreach ((DateTime date, List<VacationViewModel> vacationViewModels) in teamMemberVacation.VacationsMyMonth)
                {
                    ContentRow row = ToRow(date, vacationViewModels);
                    dataGrid.Rows.Add(row);
                }

                dataGrid.Display();
            }
        }

        private static ContentRow ToRow(DateTime date, IEnumerable<VacationViewModel> vacationResponses)
        {
            ContentRow row = new();
            row.AddCell($"{date:yyyy MM}");

            List<string> lines = vacationResponses
                .Select(ToString)
                .ToList();

            row.AddCell(lines);

            return row;
        }

        private static string ToString(VacationViewModel vacationViewModel)
        {
            StringBuilder sb = new();
            sb.Append($"{vacationViewModel}");

            if (vacationViewModel.HourCount != null)
                sb.Append($" ({vacationViewModel.HourCount}h)");

            if (vacationViewModel.Comments != null)
                sb.Append($" - {vacationViewModel.Comments}");

            return sb.ToString();
        }
    }
}