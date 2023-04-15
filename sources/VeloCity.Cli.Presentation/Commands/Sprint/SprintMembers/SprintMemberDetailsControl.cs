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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.SprintMembers;

internal class SprintMemberDetailsControl : Control
{
    private readonly DataGridFactory dataGridFactory;

    public SprintMember SprintMember { get; set; }

    public SprintMemberDetailsControl(DataGridFactory dataGridFactory)
    {
        this.dataGridFactory = dataGridFactory;
    }

    protected override void DoDisplay()
    {
        DataGrid dataGrid = CreateEmptyDataGrid();

        IEnumerable<SprintMemberDataGridRow> contentRowSelect = CreateRows();

        foreach (SprintMemberDataGridRow contentRow in contentRowSelect)
            dataGrid.Rows.Add(contentRow);

        dataGrid.Display();
    }

    private DataGrid CreateEmptyDataGrid()
    {
        DataGrid dataGrid = dataGridFactory.Create();

        HoursValue totalWorkHours = SprintMember.Days
            .Sum(x => x.WorkHours);

        dataGrid.Title = $"{SprintMember.Name} - {totalWorkHours:0}";

        dataGrid.Columns.Add("Date");

        Column workColumn = new("Work")
        {
            CellHorizontalAlignment = HorizontalAlignment.Right
        };
        dataGrid.Columns.Add(workColumn);

        Column absenceColumn = new("Absence")
        {
            CellHorizontalAlignment = HorizontalAlignment.Right
        };
        dataGrid.Columns.Add(absenceColumn);

        dataGrid.Columns.Add("Details");

        return dataGrid;
    }

    private IEnumerable<SprintMemberDataGridRow> CreateRows()
    {
        return SprintMember.Days
            .Select(x => new SprintMemberDataGridRow(x));
    }
}