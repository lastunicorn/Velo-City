using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ConsoleTools;

namespace DustInTheWind.VeloCity.Presentation.UserControls
{
    internal class CommitmentChart
    {
        private const int ChartMaxValue = 30;

        private int maxValue;

        public List<CommitmentChartItem> Items { get; set; }

        public void Display()
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