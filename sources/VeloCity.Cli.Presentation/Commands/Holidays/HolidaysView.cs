// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Holidays;

public class HolidaysView : IView<HolidaysCommand>
{
    private readonly DataGridFactory dataGridFactory;

    public HolidaysView(DataGridFactory dataGridFactory)
    {
        this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
    }

    public void Display(HolidaysCommand command)
    {
        Console.WriteLine(command.Information);

        if (command.OfficialHolidays == null || command.OfficialHolidays.Count == 0)
            DisplayEmptyInformation();
        else
            DisplayTable(command);
    }

    private static void DisplayEmptyInformation()
    {
        CustomConsole.WriteLine();
        CustomConsole.WriteLineWarning("There are no holidays in the requested time interval.");
    }

    private void DisplayTable(HolidaysCommand command)
    {
        DataGrid dataGrid = dataGridFactory.Create();
        dataGrid.Title = "Official Holidays";

        dataGrid.Columns.Add("Date");
        dataGrid.Columns.Add("Country");
        dataGrid.Columns.Add("Name");

        foreach (OfficialHolidayInstance officialHolidayInstance in command.OfficialHolidays)
        {
            string dateCellContent = $"{officialHolidayInstance.Date:d} ({officialHolidayInstance.Date:ddd})";
            string countryCellContent = officialHolidayInstance.Country;
            string nameCellContent = officialHolidayInstance.Name;

            dataGrid.Rows.Add(dateCellContent, countryCellContent, nameCellContent);
        }

        dataGrid.Display();
    }
}