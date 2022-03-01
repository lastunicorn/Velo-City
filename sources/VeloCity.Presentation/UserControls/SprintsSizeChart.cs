using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ConsoleTools;

namespace DustInTheWind.VeloCity.Presentation.UserControls
{
    internal class SprintsSizeChart
    {
        private const int ChartMaxValue = 30;

        public List<SprintsSizeChartItem> Items { get; set; }

        public void Display()
        {
            if (Items == null || Items.Count == 0)
                return;

            int sprintCount = Items.Count;
            CustomConsole.WriteLineEmphasized($"Sprint Size ({sprintCount} Sprints):");
            Console.WriteLine();

            int maxValue = Items.Max(x => x.TotalWorkHours);

            foreach (SprintsSizeChartItem item in Items)
            {
                CustomConsole.Write($"- Sprint {item.SprintNumber} - {item.TotalWorkHours:D} h - ");

                string chartBar = CreateChartBar(item.TotalWorkHours, maxValue);
                CustomConsole.WriteLine(ConsoleColor.DarkGreen, chartBar);
            }
        }

        private static string CreateChartBar(int value, int maxValue)
        {
            int chartValue = (int)Math.Round((float)value * ChartMaxValue / maxValue);
            return new string('═', chartValue);
        }
    }
}