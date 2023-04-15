// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Application.PresentSprints;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprints;

public class PresentSprintsView : IView<PresentSprintsCommand>
{
    private readonly DataGridFactory dataGridFactory;

    public PresentSprintsView(DataGridFactory dataGridFactory)
    {
        this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
    }

    public void Display(PresentSprintsCommand command)
    {
        bool sprintsExist = command.SprintOverviews is { Count: > 0 };

        if (sprintsExist)
        {
            DisplaySprints(command.SprintOverviews);
            DisplayVelocityChart(command.SprintOverviews);
            DisplayCommitmentChart(command.SprintOverviews);
            DisplaySprintsSizeChart(command.SprintOverviews);
        }
        else
        {
            Console.WriteLine("There are no sprints.");
        }
    }

    private void DisplaySprints(IEnumerable<SprintOverview> sprintOverviews)
    {
        SprintsOverview sprintsOverview = new(dataGridFactory)
        {
            Items = sprintOverviews.ToList()
        };

        sprintsOverview.Display();
    }

    private static void DisplayVelocityChart(IEnumerable<SprintOverview> sprintOverviews)
    {
        VelocityChartControl velocityChartControl = new()
        {
            Margin = "0 1 0 0",
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

    private static void DisplaySprintsSizeChart(IEnumerable<SprintOverview> sprintOverviews)
    {
        SprintsSizeChartControl sprintsSizeChartControl = new()
        {
            Margin = "0 1 0 0",
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

    private static void DisplayCommitmentChart(IEnumerable<SprintOverview> sprintOverviews)
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