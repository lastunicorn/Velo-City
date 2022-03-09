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