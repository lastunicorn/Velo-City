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
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.Sprints
{
    internal class CommitmentChartControl : BlockControl
    {
        private const int ChartMaxValue = 40;

        private float maxValue;

        public List<CommitmentChartItem> Items { get; set; }

        protected override void DoDisplayContent(ControlDisplay display)
        {
            if (Items == null || Items.Count == 0)
                return;

            int sprintCount = Items.Count;
            display.WriteRow(CustomConsole.EmphasizedColor, CustomConsole.EmphasizedBackgroundColor, $"Commitment ({sprintCount} Sprints):");
            display.WriteRow();

            maxValue = Items.Max(x => Math.Max(x.CommitmentStoryPoints, x.ActualStoryPoints));

            foreach (CommitmentChartItem item in Items)
            {
                string title = $"- Sprint {item.SprintNumber:D2} - {item.ActualStoryPoints.ToString("00")} / {item.CommitmentStoryPoints.ToString("00")} - ";
                display.Write(title);

                WriteChartLine(item, display);
            }
        }

        private void WriteChartLine(CommitmentChartItem item, ControlDisplay display)
        {
            int commitmentSpChartValue = CalculateChartValue(item.CommitmentStoryPoints);
            int actualSpChartValue = CalculateChartValue(item.ActualStoryPoints);

            int bothCount = Math.Min(actualSpChartValue, commitmentSpChartValue);
            string bothString = new('═', bothCount);
            display.Write(ConsoleColor.DarkGreen, null, bothString);

            int onlyCommitmentCount = actualSpChartValue < commitmentSpChartValue
                ? commitmentSpChartValue - actualSpChartValue
                : 0;

            if (onlyCommitmentCount > 0)
            {
                // ─ ═ » ·
                string onlyCommitmentString = new('-', onlyCommitmentCount);
                display.Write(ConsoleColor.DarkRed, null, onlyCommitmentString);
            }

            int onlyActualCount = actualSpChartValue > commitmentSpChartValue
                ? actualSpChartValue - commitmentSpChartValue
                : 0;

            if (onlyActualCount > 0)
            {
                string onlyActualString = new('═', onlyActualCount);
                display.Write(ConsoleColor.DarkRed, null, onlyActualString);
            }

            display.WriteRow();
        }

        private int CalculateChartValue(float value)
        {
            return (int)Math.Round(value * ChartMaxValue / maxValue);
        }
    }
}