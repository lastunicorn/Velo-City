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
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprints
{
    internal class SprintsSizeChartControl : BlockControl
    {
        private const int ChartMaxValue = 40;

        public List<SprintsSizeChartItem> Items { get; set; }

        protected override void DoDisplayContent(ControlDisplay display)
        {
            if (Items == null || Items.Count == 0)
                return;

            int sprintCount = Items.Count;
            CustomConsole.WriteLineEmphasized($"Sprint Size ({sprintCount} Sprints):");
            display.WriteRow();

            int maxValue = Items.Max(x => x.TotalWorkHours);

            foreach (SprintsSizeChartItem item in Items)
            {
                display.Write($"- Sprint {item.SprintNumber:D2} - {item.TotalWorkHours:D} h - ");

                string chartBar = CreateChartBar(item.TotalWorkHours, maxValue);
                display.WriteRow(ConsoleColor.DarkGreen, null, chartBar);
            }
        }

        private static string CreateChartBar(int value, int maxValue)
        {
            int chartValue = (int)Math.Round((float)value * ChartMaxValue / maxValue);
            return new string('═', chartValue);
        }
    }
}