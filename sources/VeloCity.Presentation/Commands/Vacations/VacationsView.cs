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
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Application.PresentVacations;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Presentation.Commands.Vacations
{
    public class VacationsView : IView<VacationsCommand>
    {
        public void Display(VacationsCommand command)
        {
            foreach (TeamMemberVacation teamMemberVacation in command.TeamMemberVacations)
            {
                CustomConsole.WriteLine();

                DataGrid dataGrid = new()
                {
                    Title = teamMemberVacation.PersonName,
                    TitleRow =
                    {
                        ForegroundColor = ConsoleColor.Black,
                        BackgroundColor = ConsoleColor.DarkGray
                    },
                    Border =
                    {
                        DisplayBorderBetweenRows = true
                    }
                };

                foreach ((DateTime date, List<VacationResponse> vacationInfos) in teamMemberVacation.VacationsMyMonth)
                {
                    ContentRow row = ToRow(date, vacationInfos);
                    dataGrid.Rows.Add(row);
                }

                dataGrid.Display();
            }
        }

        private static ContentRow ToRow(DateTime date, IEnumerable<VacationResponse> vacationInfos)
        {
            ContentRow row = new();
            row.AddCell($"{date:yyyy MM}");

            List<string> lines = vacationInfos
                .Select(ToString)
                .ToList();

            row.AddCell(lines);

            return row;
        }

        private static string ToString(VacationResponse vacationResponse)
        {
            StringBuilder sb = new();
            sb.Append($"{vacationResponse.Date:d}");

            if (vacationResponse.HourCount != null)
                sb.Append($" ({vacationResponse.HourCount}h)");

            if (vacationResponse.Comments != null)
                sb.Append($" - {vacationResponse.Comments}");

            return sb.ToString();
        }
    }
}