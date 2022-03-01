using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Application.PresentSprints;

namespace DustInTheWind.VeloCity.Presentation.UserControls
{
    internal class SprintsOverview
    {
        public List<SprintOverview> Items { get; set; }

        public void Display()
        {
            if (Items == null || Items.Count == 0)
                return;

            DataGrid dataGrid = new()
            {
                Title = $"The last {Items.Count} Sprints",
                TitleRow =
                {
                    ForegroundColor = ConsoleColor.Black,
                    BackgroundColor = ConsoleColor.DarkGray
                },
                Border =
                {
                    DisplayBorderBetweenRows = true
                }
            };

            IEnumerable<ContentRow> rows = Items
                .Select(CreateRow);

            foreach (ContentRow row in rows)
                dataGrid.Rows.Add(row);

            dataGrid.Display();
        }

        private static ContentRow CreateRow(SprintOverview sprintOverview)
        {
            ContentCell sprintNameCell = CreateNameCell(sprintOverview);
            ContentCell sprintInfoCell = CreateInfoCell(sprintOverview);

            return new ContentRow(sprintNameCell, sprintInfoCell);
        }

        private static ContentCell CreateNameCell(SprintOverview sprintOverview)
        {
            List<string> sprintNameLines = new()
            {
                sprintOverview.Name,
                $"({sprintOverview.StartDate:d} - {sprintOverview.EndDate:d})"
            };

            return new ContentCell(sprintNameLines);
        }

        private static ContentCell CreateInfoCell(SprintOverview sprintOverview)
        {
            List<string> sprintInfoLines = new()
            {
                $"Total Work Hours: {sprintOverview.TotalWorkHours} h",
                $"Actual Story Points: {sprintOverview.ActualStoryPoints} SP",
                $"Actual Velocity: {sprintOverview.ActualVelocity} SP/h"
            };

            return new ContentCell(sprintInfoLines);
        }
    }
}