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
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.Sprint.SprintOverview
{
    internal class SprintOverviewControl : Control
    {
        private readonly DataGridFactory dataGridFactory;

        public SprintOverviewViewModel ViewModel { get; set; }

        public SprintOverviewControl(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        protected override void DoDisplay()
        {
            DisplayOverviewTable();
            DisplayNotes();
        }

        private void DisplayOverviewTable()
        {
            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = ViewModel.SprintName;

            dataGrid.Rows.Add("Start Date", ViewModel.StartDate.ToString("d"));
            dataGrid.Rows.Add("End Date", ViewModel.EndDate.ToString("d"));
            dataGrid.Rows.Add("State", ViewModel.State);
            dataGrid.Rows.Add(" ", " ");
            dataGrid.Rows.Add("Work Days", ViewModel.WorkDays + " days");
            dataGrid.Rows.Add("Total Work Hours", $"{ViewModel.TotalWorkHours} h");
            dataGrid.Rows.Add(" ", " ");
            dataGrid.Rows.Add("Estimated Story Points", $"{ViewModel.EstimatedStoryPoints.ToStandardDigitsString()}");

            if (!ViewModel.EstimatedStoryPointsWithVelocityPenalties.IsNull)
                dataGrid.Rows.Add("Estimated Story Points (*)", $"{ViewModel.EstimatedStoryPointsWithVelocityPenalties}");

            dataGrid.Rows.Add("Estimated Velocity", $"{ViewModel.EstimatedVelocity.ToStandardDigitsString()}");
            dataGrid.Rows.Add("Commitment Story Points", $"{ViewModel.CommitmentStoryPoints}");
            dataGrid.Rows.Add(" ", " ");
            dataGrid.Rows.Add("Actual Story Points", $"{ViewModel.ActualStoryPoints}");
            dataGrid.Rows.Add("Actual Velocity", $"{ViewModel.ActualVelocity.ToStandardDigitsString()}");

            dataGrid.Display();
        }

        private void DisplayNotes()
        {
            NotesControl notesControl = new()
            {
                Notes = ViewModel.Notes
            };
            notesControl.Display();
        }
    }
}