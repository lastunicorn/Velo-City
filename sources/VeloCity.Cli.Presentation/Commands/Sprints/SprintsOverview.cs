// VeloCity
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

using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Cli.Application.PresentSprints;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprints;

internal class SprintsOverview
{
    private readonly DataGridFactory dataGridFactory;

    public List<SprintOverview> Items { get; set; }

    public SprintsOverview(DataGridFactory dataGridFactory)
    {
        this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
    }

    public void Display()
    {
        if (Items == null || Items.Count == 0)
            return;

        DataGrid dataGrid = dataGridFactory.Create();
        dataGrid.Title = $"The last {Items.Count} Sprints";
        dataGrid.Border.DisplayBorderBetweenRows = true;

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
            sprintOverview.Name
        };

        return new ContentCell(sprintNameLines);
    }

    private static ContentCell CreateInfoCell(SprintOverview sprintOverview)
    {
        List<string> sprintInfoLines = new()
        {
            $"{sprintOverview.StartDate:d} - {sprintOverview.EndDate:d}"
        };

        return new ContentCell(sprintInfoLines);
    }
}