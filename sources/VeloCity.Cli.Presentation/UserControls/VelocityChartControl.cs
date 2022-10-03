// VeloCity
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

namespace DustInTheWind.VeloCity.Cli.Presentation.UserControls
{
    internal class VelocityChartControl : BlockControl
    {
        private const int ChartMaxValue = 40;

        private float maxValue;

        public List<VelocityChartItem> Items { get; set; }

        protected override void DoDisplayContent(ControlDisplay display)
        {
            if (Items == null || Items.Count == 0)
                return;

            int sprintCount = Items.Count;
            display.WriteRow(CustomConsole.EmphasizedColor, CustomConsole.EmphasizedBackgroundColor, $"Velocity ({sprintCount} Sprints):");
            display.WriteRow();

            maxValue = Items.Max(x => x.Velocity.Value);

            foreach (VelocityChartItem item in Items)
            {
                display.Write($"- Sprint {item.SprintNumber:D2} - {item.Velocity.ToString("0.0000")} - ");

                string chartBar = CreateChartBar(item);
                display.WriteRow(ConsoleColor.DarkGreen, null, chartBar);
            }
        }

        private string CreateChartBar(VelocityChartItem item)
        {
            if (maxValue == 0)
                return string.Empty;

            float value = item.Velocity;
            int chartValue = (int)Math.Round(value * ChartMaxValue / maxValue);

            return new string('═', chartValue);
        }
    }
}