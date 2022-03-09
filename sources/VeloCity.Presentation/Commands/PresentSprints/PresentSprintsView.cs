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
using DustInTheWind.VeloCity.Application.PresentSprints;
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentSprints
{
    public class PresentSprintsView
    {
        public void Display(PresentSprintsResponse response)
        {
            bool sprintsExist = response.SprintOverviews is { Count: > 0 };

            if (sprintsExist)
            {
                DisplaySprints(response.SprintOverviews);
                DisplayVelocityChart(response.SprintOverviews);
                DisplayCommitmentChart(response.SprintOverviews);
                DisplaySprintsDimensionChart(response.SprintOverviews);
            }
            else
            {
                Console.WriteLine("There are no sprints.");
            }
        }

        private static void DisplaySprints(IEnumerable<SprintOverview> sprintOverviews)
        {
            SprintsOverview sprintsOverview = new()
            {
                Items = sprintOverviews.ToList()
            };

            sprintsOverview.Display();
        }

        private static void DisplayVelocityChart(IEnumerable<SprintOverview> sprintOverviews)
        {
            Console.WriteLine();

            VelocityChartControl velocityChartControl = new()
            {
                Items = sprintOverviews
                    .Select(x => new VelocityChartItem
                    {
                        SprintNumber = x.SprintNumber,
                        Velocity = x.ActualVelocity
                    })
                    .ToList()
            };

            velocityChartControl.Display();
        }

        private static void DisplaySprintsDimensionChart(IEnumerable<SprintOverview> sprintOverviews)
        {
            Console.WriteLine();

            SprintsSizeChartControl sprintsSizeChartControl = new()
            {
                Items = sprintOverviews
                    .Select(x => new SprintsSizeChartItem
                    {
                        SprintNumber = x.SprintNumber,
                        TotalWorkHours = x.TotalWorkHours
                    })
                    .ToList()
            };

            sprintsSizeChartControl.Display();
        }

        private static void DisplayCommitmentChart(List<SprintOverview> sprintOverviews)
        {
            Console.WriteLine();

            CommitmentChartControl commitmentChartControl = new()
            {
                Items = sprintOverviews
                    .Select(x => new CommitmentChartItem
                    {
                        SprintNumber = x.SprintNumber,
                        CommitmentStoryPoints = x.CommitmentStoryPoints,
                        ActualStoryPoints = x.ActualStoryPoints
                    })
                    .ToList()
            };

            commitmentChartControl.Display();
        }
    }
}