﻿// VeloCity
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
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.SprintOverview;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls.Notes;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Forecast;

public class ForecastView : IView<ForecastCommand>
{
    private readonly DataGridFactory dataGridFactory;

    public ForecastView(DataGridFactory dataGridFactory)
    {
        this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
    }

    public void Display(ForecastCommand command)
    {
        DisplayOverviewTable(command);
        DisplaySprintDetails(command.Sprints);
    }

    private void DisplayOverviewTable(ForecastCommand command)
    {
        DataGrid dataGrid = dataGridFactory.Create();
        dataGrid.Title = "Forecast Overview";

        dataGrid.Rows.Add("Date Interval", $"{command.StartDate:d} - {command.EndDate:d}");
        dataGrid.Rows.Add(" ", " ");
        dataGrid.Rows.Add("Total Work Days", command.Sprints.Sum(x => x.WorkDaysCount) + " days");
        dataGrid.Rows.Add("Total Work Hours", $"{command.TotalWorkHours}");
        dataGrid.Rows.Add(" ", " ");
        dataGrid.Rows.Add("Estimated Velocity", $"{command.EstimatedVelocity.ToStandardDigitsString()}");
        dataGrid.Rows.Add("Estimated Story Points", $"{command.EstimatedStoryPoints.ToStandardDigitsString()}");

        if (!command.EstimatedStoryPointsWithVelocityPenalties.IsEmpty)
            dataGrid.Rows.Add("Estimated Story Points (*)", $"{command.EstimatedStoryPointsWithVelocityPenalties.ToStandardDigitsString()}");

        AddFooter(dataGrid, command);

        dataGrid.Display();
    }

    private static void AddFooter(DataGrid dataGrid, ForecastCommand command)
    {
        if (command.Notes?.Count > 0)
        {
            NotesControl notesControl = new()
            {
                Notes = command.Notes
            };

            dataGrid.FooterRow.FooterCell.Content = new MultilineText(notesControl.ToLines());
            dataGrid.FooterRow.FooterCell.ForegroundColor = ConsoleColor.DarkYellow;
        }
    }

    private void DisplaySprintDetails(List<SprintForecast> sprints)
    {
        foreach (SprintForecast sprint in sprints)
            DisplaySprintDetails(sprint);
    }

    private void DisplaySprintDetails(SprintForecast sprintForecast)
    {
        DataGrid dataGrid = dataGridFactory.Create();
        dataGrid.Title = sprintForecast.SprintName;

        dataGrid.Rows.Add("Date Interval", $"{sprintForecast.StartDate:d} - {sprintForecast.EndDate:d}");
        dataGrid.Rows.Add(" ", " ");
        dataGrid.Rows.Add("Work Days", sprintForecast.WorkDaysCount + " days");
        dataGrid.Rows.Add("Work Hours", $"{sprintForecast.TotalWorkHours}");
        dataGrid.Rows.Add(" ", " ");
        dataGrid.Rows.Add("Estimated Story Points", $"{sprintForecast.EstimatedStoryPoints.ToStandardDigitsString()}");

        if (!sprintForecast.EstimatedStoryPointsWithVelocityPenalties.IsEmpty)
            dataGrid.Rows.Add("Estimated Story Points (*)", $"{sprintForecast.EstimatedStoryPointsWithVelocityPenalties.ToStandardDigitsString()}");

        AddFooter(dataGrid, sprintForecast);

        dataGrid.Display();
    }

    private static void AddFooter(DataGrid dataGrid, SprintForecast sprint)
    {
        bool velocityPenaltiesExist = !sprint.EstimatedStoryPointsWithVelocityPenalties.IsEmpty;
        if (!velocityPenaltiesExist)
            return;

        NotesControl notesControl = new()
        {
            Notes = new List<NoteBase>
            {
                new VelocityPenaltiesNote()
            }
        };
        dataGrid.FooterRow.FooterCell.Content = new MultilineText(notesControl.ToLines());
        dataGrid.FooterRow.FooterCell.ForegroundColor = ConsoleColor.DarkYellow;
    }
}