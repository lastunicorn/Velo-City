using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ConsoleTools;

namespace DustInTheWind.VeloCity.Presentation.UserControls
{
    internal class VelocityChart
    {
        private const int ChartMaxValue = 30;

        private float maxValue;

        public List<VelocityChartItem> Items { get; set; }

        public void Display()
        {
            if (Items == null || Items.Count == 0)
                return;

            int sprintCount = Items.Count;
            CustomConsole.WriteLineEmphasized($"Velocity ({sprintCount} Sprints):");
            Console.WriteLine();

            maxValue = Items.Max(x => x.Velocity);

            foreach (VelocityChartItem item in Items)
            {
                CustomConsole.Write($"- Sprint {item.SprintNumber} - {item.Velocity:N4} SP/h - ");

                string chartBar = CreateChartBar(item);
                CustomConsole.WriteLine(ConsoleColor.DarkGreen, chartBar);
            }
        }

        private string CreateChartBar(VelocityChartItem item)
        {
            float value = item.Velocity;
            int chartValue = (int)Math.Round(value * ChartMaxValue / maxValue);

            return new string('═', chartValue);
        }
    }
}