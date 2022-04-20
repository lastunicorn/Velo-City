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

namespace DustInTheWind.VeloCity.Presentation.Commands.Help
{
    public class HelpView : IView<HelpCommand>
    {
        public void Display(HelpCommand command)
        {
            if (command.Commands is { Count: > 0 })
                DisplayCommandsOverview(command.Commands);
            else if (command.CommandDetails != null)
                DisplayCommandDetails(command.CommandDetails);
        }

        private static void DisplayCommandsOverview(IEnumerable<CommandShortInfo> commands)
        {
            Console.WriteLine("usage: velo [command]");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine();

            DataGrid dataGrid = new()
            {
                Border = { IsVisible = false }
            };

            Column column = dataGrid.Columns.Add(new Column());
            column.CellPaddingLeft = 0;

            IEnumerable<ContentRow> rows = commands.Select(CreateContentRow);
            dataGrid.Rows.AddRange(rows);

            dataGrid.Display();
        }

        private static ContentRow CreateContentRow(CommandShortInfo commandShortInfo)
        {
            ContentRow row = new();

            row.AddCell(commandShortInfo.Name);
            row.AddCell(commandShortInfo.Description);

            return row;
        }

        private static void DisplayCommandDetails(CommandFullInfo commandDetails)
        {
            Console.WriteLine("usage:");
            Console.WriteLine(commandDetails.Usage);

            Console.WriteLine();
            Console.WriteLine($"Description: {commandDetails.Description}");
        }
    }
}