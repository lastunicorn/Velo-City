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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Commands.Sprint.SprintOverview;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.Forecast
{
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
            DisplayOverviewNotes(command);
            DisplaySprintDetails(command.Sprints);
        }

        private void DisplayOverviewTable(ForecastCommand command)
        {
            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = "Forecast Overview";

            dataGrid.Rows.Add("Time Interval", $"{command.StartDate:d} - {command.EndDate:d}");
            dataGrid.Rows.Add("Total Work Days", command.Sprints.Sum(x => x.Days.Count) + " days");
            dataGrid.Rows.Add("Total Work Hours", $"{command.TotalWorkHours}");
            dataGrid.Rows.Add("Estimated Velocity", $"{command.EstimatedVelocity.ToStandardDigitsString()}");
            dataGrid.Rows.Add("Estimated Story Points", $"{command.EstimatedStoryPoints.ToStandardDigitsString()}");

            if (!command.EstimatedStoryPointsWithVelocityPenalties.IsNull)
                dataGrid.Rows.Add("Estimated Story Points (*)", $"{command.EstimatedStoryPointsWithVelocityPenalties.ToStandardDigitsString()}");

            dataGrid.Display();
        }

        private static void DisplayOverviewNotes(ForecastCommand command)
        {
            bool velocityPenaltiesExist = !command.EstimatedStoryPointsWithVelocityPenalties.IsNull;
            if (!velocityPenaltiesExist)
                return;

            NotesControl notesControl = new()
            {
                Notes = new List<NoteBase>
                {
                    new VelocityPenaltiesNote()
                }
            };
            notesControl.Display();
        }

        private void DisplaySprintDetails(List<SprintForecast> sprints)
        {
            foreach (SprintForecast sprint in sprints)
            {
                DisplaySprintDetails(sprint);
                DisplaySprintNotes(sprint);
            }
        }

        private void DisplaySprintDetails(SprintForecast sprint)
        {
            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = sprint.IsRealSprint
                ? $"Sprint {sprint.Number}"
                : "Presumed Sprint";

            dataGrid.Rows.Add("Time Interval", $"{sprint.StartDate:d} - {sprint.EndDate:d}");
            dataGrid.Rows.Add("Work Days", sprint.Days.Count + " days");
            dataGrid.Rows.Add("Work Hours", $"{sprint.TotalWorkHours}");
            dataGrid.Rows.Add("Estimated Story Points", $"{sprint.EstimatedStoryPoints.ToStandardDigitsString()}");

            if (!sprint.EstimatedStoryPointsWithVelocityPenalties.IsNull)
                dataGrid.Rows.Add("Estimated Story Points (*)", $"{sprint.EstimatedStoryPointsWithVelocityPenalties.ToStandardDigitsString()}");

            dataGrid.Display();
        }

        private static void DisplaySprintNotes(SprintForecast sprint)
        {
            bool velocityPenaltiesExist = !sprint.EstimatedStoryPointsWithVelocityPenalties.IsNull;
            if (!velocityPenaltiesExist)
                return;

            NotesControl notesControl = new()
            {
                Notes = new List<NoteBase>
                {
                    new VelocityPenaltiesNote()
                }
            };
            notesControl.Display();
        }
    }
}