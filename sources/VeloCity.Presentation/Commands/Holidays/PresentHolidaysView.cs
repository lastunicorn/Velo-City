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
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Presentation.Commands.Holidays
{
    public class PresentHolidaysView : IView<PresentHolidaysCommand>
    {
        private readonly DataGridFactory dataGridFactory;

        public PresentHolidaysView(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        public void Display(PresentHolidaysCommand command)
        {
            Console.WriteLine(command.RequestType);

            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = "Official Holidays";

            dataGrid.Columns.Add("Date");
            dataGrid.Columns.Add("Country");
            dataGrid.Columns.Add("Name");

            foreach (OfficialHolidayInstance officialHolidayInstance in command.OfficialHolidays)
            {
                string dateCellContent = officialHolidayInstance.Date.ToString("d");
                string countryCellContent = officialHolidayInstance.Country;
                string nameCellContent = officialHolidayInstance.Name;

                dataGrid.Rows.Add(dateCellContent, countryCellContent, nameCellContent);
            }

            dataGrid.Display();
        }
    }
}