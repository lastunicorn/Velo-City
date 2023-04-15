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
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls.Notes;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.TeamOverview;

internal class TeamOverviewControl : Control
{
    private readonly DataGridFactory dataGridFactory;

    public TeamOverviewViewModel ViewModel { get; set; }

    public TeamOverviewControl(DataGridFactory dataGridFactory)
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
        dataGrid.Title = "Team Members";

        AddColumns(dataGrid);
        AddContentData(dataGrid);
        AddFooter(dataGrid);

        dataGrid.Display();
    }

    private static void AddColumns(DataGrid dataGrid)
    {
        dataGrid.Columns.Add("Name");

        Column workHoursColumn = new("Work", HorizontalAlignment.Right);
        dataGrid.Columns.Add(workHoursColumn);

        dataGrid.Columns.Add(string.Empty);

        Column absenceHoursColumn = new("Absence", HorizontalAlignment.Right);
        dataGrid.Columns.Add(absenceHoursColumn);
    }

    private void AddContentData(DataGrid dataGrid)
    {
        TeamMembersChart chart = new(ViewModel.TeamMembers);
        using IEnumerator<ChartBarValue<TeamMemberViewModel>> chartBarEnumerator = chart.GetEnumerator();

        IEnumerable<ContentRow> rows = ViewModel.TeamMembers
            .Select(x =>
            {
                bool success = chartBarEnumerator.MoveNext();

                ChartBarValue<TeamMemberViewModel> chartBarValue = success
                    ? chartBarEnumerator.Current
                    : new ChartBarValue<TeamMemberViewModel>();

                return CreateContentRow(x, chartBarValue);
            });

        foreach (ContentRow row in rows)
            dataGrid.Rows.Add(row);
    }

    private static ContentRow CreateContentRow(TeamMemberViewModel teamMember, ChartBarValue<TeamMemberViewModel> chartBarValue)
    {
        ContentRow dataRow = new();

        ContentCell nameCell = CreateNameCell(teamMember);
        dataRow.AddCell(nameCell);

        ContentCell workHoursCell = CreateWorkHoursCell(teamMember);
        dataRow.AddCell(workHoursCell);

        ContentCell chartCell = CreateChartCell(chartBarValue);
        dataRow.AddCell(chartCell);

        ContentCell absenceCell = CreateAbsenceCell(teamMember);
        dataRow.AddCell(absenceCell);

        return dataRow;
    }

    private static ContentCell CreateNameCell(TeamMemberViewModel teamMember)
    {
        return new ContentCell
        {
            Content = teamMember.Name.FullName
        };
    }

    private static ContentCell CreateWorkHoursCell(TeamMemberViewModel teamMember)
    {
        return new ContentCell
        {
            Content = teamMember.WorkHours.ToString(),
            ForegroundColor = teamMember.WorkHours > 0
                ? ConsoleColor.Green
                : null
        };
    }

    private static ContentCell CreateChartCell(ChartBarValue<TeamMemberViewModel> chartBarValue)
    {
        return new ContentCell
        {
            Content = chartBarValue.ToString(),
            ForegroundColor = ConsoleColor.Green
        };
    }

    private static ContentCell CreateAbsenceCell(TeamMemberViewModel teamMember)
    {
        return new ContentCell
        {
            Content = teamMember.AbsenceHours.ToString(),
            ForegroundColor = teamMember.AbsenceHours > 0
                ? ConsoleColor.Yellow
                : null
        };
    }

    private void AddFooter(DataGrid dataGrid)
    {
        if (ViewModel.Notes is { Count: > 0 })
        {
            NotesControl notesControl = new()
            {
                Notes = ViewModel.Notes
            };
            dataGrid.FooterRow.FooterCell.Content = notesControl.ToString();
            dataGrid.FooterRow.FooterCell.ForegroundColor = ConsoleColor.DarkYellow;
        }
    }
}