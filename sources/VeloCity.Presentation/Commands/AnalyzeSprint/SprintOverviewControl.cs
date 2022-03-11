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
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    internal class SprintOverviewControl : Control
    {
        public AnalyzeSprintCommand Command { get; set; }

        protected override void DoDisplay()
        {
            DisplayOverviewTable();
            DisplayNotes();
        }

        private void DisplayOverviewTable()
        {
            DataGrid dataGrid = new()
            {
                Title = $"{Command.SprintName} ({Command.StartDate:d} - {Command.EndDate:d})",
                TitleRow =
                {
                    ForegroundColor = ConsoleColor.Black,
                    BackgroundColor = ConsoleColor.DarkGray
                }
            };

            dataGrid.Rows.Add("State", RenderState());
            dataGrid.Rows.Add("Work Days", Command.WorkDays?.Count + " days");
            dataGrid.Rows.Add("Total Work Hours", $"{Command.TotalWorkHours} h");

            string estimatedStoryPointsString = Command.EstimatedStoryPoints == null
                ? "-"
                : Command.EstimatedStoryPoints.ToString();
            dataGrid.Rows.Add("Estimated Story Points", $"{estimatedStoryPointsString} SP");

            string estimatedVelocityString = Command.EstimatedVelocity == null
                ? "-"
                : Command.EstimatedVelocity.ToString();
            dataGrid.Rows.Add("Estimated Velocity", $"{estimatedVelocityString} SP/h");
            dataGrid.Rows.Add("Commitment Story Points", $"{Command.CommitmentStoryPoints} SP");

            dataGrid.Rows.Add("Actual Story Points", $"{Command.ActualStoryPoints} SP");
            dataGrid.Rows.Add("Actual Velocity", $"{Command.ActualVelocity} SP/h");

            dataGrid.Display();
        }

        private string RenderState()
        {
            return Command.SprintState switch
            {
                SprintState.Unknown => "unknown",
                SprintState.New => "new",
                SprintState.InProgress => "in progress",
                SprintState.Closed => "closed",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void DisplayNotes()
        {
            CustomConsole.WriteLine();
            CustomConsole.WriteLine(ConsoleColor.DarkYellow, "Notes:");

            bool previousSprintsExist = Command.PreviousSprints != null && Command.PreviousSprints.Count > 0;

            if (previousSprintsExist)
            {
                string previousSprints = string.Join(", ", Command.PreviousSprints);
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, $"  - The estimations were calculated based on previous {Command.PreviousSprints.Count} closed sprints: {previousSprints}");
            }
            else
            {
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, "  - Could not calculate an estimation because no previous closed sprints exist.");
            }

            if (Command.ExcludesSprints is { Count: > 0 })
            {
                string excludedSprints = string.Join(",", Command.ExcludesSprints);
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, $"  - Excluded sprints: {excludedSprints} (These sprints were excluded from the velocity calculation algorithm.)");
            }
        }
    }
}