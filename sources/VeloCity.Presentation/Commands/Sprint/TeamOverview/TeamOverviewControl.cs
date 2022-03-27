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
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Presentation.Commands.Sprint.SprintCalendar;
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.Sprint.TeamOverview
{
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
            DisplayNotes();
        }

        private void DisplayOverviewTable()
        {
            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = "Team Members";

            dataGrid.Columns.Add("Name");

            Column workHoursColumn = new("Work", HorizontalAlignment.Right);
            dataGrid.Columns.Add(workHoursColumn);

            dataGrid.Columns.Add(string.Empty);

            Column absenceHoursColumn = new("Absence", HorizontalAlignment.Right);
            dataGrid.Columns.Add(absenceHoursColumn);

            Chart chart = CreateChart();
            using IEnumerator<ChartBar> chartBarEnumerator = chart.GetEnumerator();

            IEnumerable<ContentRow> rows = ViewModel.TeamMembers
                .Select(x =>
                {
                    bool success = chartBarEnumerator.MoveNext();

                    ChartBar chartBar = success
                        ? chartBarEnumerator.Current
                        : new ChartBar();

                    return CreateContentRow(x, chartBar);
                });

            foreach (ContentRow row in rows)
                dataGrid.Rows.Add(row);

            dataGrid.Display();
        }

        private Chart CreateChart()
        {
            Chart chart = new();

            IEnumerable<ChartBar> chartBars = ViewModel.TeamMembers
                .Select(x => new ChartBar
                {
                    MaxValue = x.WorkHours + x.AbsenceHours,
                    Value = x.WorkHours
                });

            foreach (ChartBar chartBar in chartBars)
                chart.Add(chartBar);

            return chart;
        }

        private static ContentRow CreateContentRow(TeamMemberViewModel teamMember, ChartBar chartBar)
        {
            ContentRow dataRow = new();

            ContentCell nameCell = CreateNameCell(teamMember);
            dataRow.AddCell(nameCell);

            ContentCell workHoursCell = CreateWorkHoursCell(teamMember);
            dataRow.AddCell(workHoursCell);

            ContentCell chartCell = CreateChartCell(chartBar);
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

        private static ContentCell CreateChartCell(ChartBar chartBar)
        {
            return new ContentCell
            {
                Content = chartBar.ToString(),
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

        private static void DisplayNotes()
        {
            NotesControl notesControl = new()
            {
                Notes = new List<NoteBase> { new TeamDetailsNote() }
            };
            notesControl.Display();
        }
    }
}