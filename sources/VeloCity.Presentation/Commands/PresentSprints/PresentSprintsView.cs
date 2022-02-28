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
using System.Text;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Application.PresentSprints;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentSprints
{
    public class PresentSprintsView
    {
        public void Display(PresentSprintsResponse response)
        {
            DataGrid dataGrid = new()
            {
                Title = $"The last {response.SprintOverviews?.Count} Sprints",
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

            if (response.SprintOverviews != null)
            {
                foreach (SprintOverview sprintOverview in response.SprintOverviews)
                {
                    List<string> sprintNameLines = new()
                    {
                        sprintOverview.Name,
                        $"({sprintOverview.StartDate:d} - {sprintOverview.EndDate:d})"
                    };
                    ContentCell sprintNameCell = new(sprintNameLines);


                    List<string> sprintInfoLines = new()
                    {
                        $"Total Work Hours: {sprintOverview.TotalWorkHours} h",
                        $"Actual Story Points: {sprintOverview.ActualStoryPoints} SP",
                        $"Actual Velocity: {sprintOverview.ActualVelocity} SP/h"
                    };

                    ContentCell sprintInfoCell = new(sprintInfoLines);

                    dataGrid.Rows.Add(sprintNameCell, sprintInfoCell);
                }
            }

            dataGrid.Display();
        }
    }
}