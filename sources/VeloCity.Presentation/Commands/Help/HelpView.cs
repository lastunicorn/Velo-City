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
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.VeloCity.Presentation.Commands.Help
{
    public class HelpView
    {
        public void Display(List<CommandInfo> commands)
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

            foreach (CommandInfo commandInfo in commands)
            {
                ContentRow row = new();
                
                row.AddCell(commandInfo.Name);
                row.AddCell(commandInfo.DescriptionLines);
                
                dataGrid.Rows.Add(row);
            }

            dataGrid.Display();
        }
    }
}