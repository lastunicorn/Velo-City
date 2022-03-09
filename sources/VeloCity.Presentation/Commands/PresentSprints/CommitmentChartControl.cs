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

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentSprints
{
    internal class CommitmentChartControl : Control
    {
        private const int ChartMaxValue = 30;

        private int maxValue;

        public List<CommitmentChartItem> Items { get; set; }

        protected override void DoDisplay()
        {
            if (Items == null || Items.Count == 0)
                return;

            int sprintCount = Items.Count;
            CustomConsole.WriteLineEmphasized($"Commitment ({sprintCount} Sprints):");
            Console.WriteLine();

            maxValue = Items.Max(x => Math.Max(x.CommitmentStoryPoints, x.ActualStoryPoints));

            foreach (CommitmentChartItem item in Items)
            {
                Console.Write($"- Sprint {item.SprintNumber} - {item.ActualStoryPoints,2} SP / {item.CommitmentStoryPoints,2} SP - ");
                WriteChartLine(item);
            }
        }

        private void WriteChartLine(CommitmentChartItem item)
        {
            int commitmentSpChartValue = CalculateChartValue(item.CommitmentStoryPoints);
            int actualSpChartValue = CalculateChartValue(item.ActualStoryPoints);

            int bothCount = Math.Min(actualSpChartValue, commitmentSpChartValue);
            string bothString = new('═', bothCount);
            CustomConsole.Write(ConsoleColor.DarkGreen, bothString);

            int onlyCommitmentCount = actualSpChartValue < commitmentSpChartValue
                ? commitmentSpChartValue - actualSpChartValue
                : 0;

            if (onlyCommitmentCount > 0)
            {
                // ─ ═ » ·
                string onlyCommitmentString = new('-', onlyCommitmentCount);
                CustomConsole.Write(ConsoleColor.DarkRed, onlyCommitmentString);
            }

            int onlyActualCount = actualSpChartValue > commitmentSpChartValue
                ? actualSpChartValue - commitmentSpChartValue
                : 0;

            if (onlyActualCount > 0)
            {
                string onlyActualString = new('═', onlyActualCount);
                CustomConsole.Write(ConsoleColor.DarkRed, onlyActualString);
            }

            CustomConsole.WriteLine();
        }

        private int CalculateChartValue(int value)
        {
            return (int)Math.Round((float)value * ChartMaxValue / maxValue);
        }
    }
}