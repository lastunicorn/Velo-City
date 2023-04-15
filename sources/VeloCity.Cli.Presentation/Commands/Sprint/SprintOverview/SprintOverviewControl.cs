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

using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls.Notes;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.SprintOverview;

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
    }

    private void DisplayOverviewTable()
    {
        DataGrid dataGrid = dataGridFactory.Create();
        dataGrid.Title = ViewModel.SprintName;

        dataGrid.Columns.Add("q");
        dataGrid.Columns.Add("a").CellHorizontalAlignment = HorizontalAlignment.Stretch;
        dataGrid.HeaderRow.IsVisible = false;

        AddContent(dataGrid);
        AddFooter(dataGrid);

        dataGrid.Display();
    }

    private void AddContent(DataGrid dataGrid)
    {
        dataGrid.Rows.Add("Time Interval", $"{ViewModel.StartDate:d} - {ViewModel.EndDate:d}");
        dataGrid.Rows.Add("State", ViewModel.State);
        dataGrid.Rows.Add(" ", " ");
        dataGrid.Rows.Add("Work Days", ViewModel.WorkDaysCount + " days");
        dataGrid.Rows.Add("Total Work Hours", $"{ViewModel.TotalWorkHours}");
        dataGrid.Rows.Add(" ", " ");
        dataGrid.Rows.Add("Estimated Story Points", $"{ViewModel.EstimatedStoryPoints.ToStandardDigitsString()}");

        if (!ViewModel.EstimatedStoryPointsWithVelocityPenalties.IsEmpty)
            dataGrid.Rows.Add("Estimated Story Points (*)", $"{ViewModel.EstimatedStoryPointsWithVelocityPenalties.ToStandardDigitsString()}");

        dataGrid.Rows.Add("Estimated Velocity", $"{ViewModel.EstimatedVelocity.ToStandardDigitsString()}");
        dataGrid.Rows.Add("Commitment Story Points", $"{ViewModel.CommitmentStoryPoints.ToStandardDigitsString()}");
        dataGrid.Rows.Add(" ", " ");
        dataGrid.Rows.Add("Actual Story Points", $"{ViewModel.ActualStoryPoints.ToStandardDigitsString()}");
        dataGrid.Rows.Add("Actual Velocity", $"{ViewModel.ActualVelocity.ToStandardDigitsString()}");
    }

    private void AddFooter(DataGrid dataGrid)
    {
        if (ViewModel.Notes is { Count: > 0 })
        {
            NotesControl notesControl = new()
            {
                Notes = ViewModel.Notes
            };
            dataGrid.FooterRow.FooterCell.Content = new MultilineText(notesControl.ToLines());
            dataGrid.FooterRow.FooterCell.ForegroundColor = ConsoleColor.DarkYellow;
        }
    }
}