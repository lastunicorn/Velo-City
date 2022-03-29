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
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.Sprint.SprintCalendar
{
    internal class SprintCalendarControl : Control
    {
        private readonly DataGridFactory dataGridFactory;

        public SprintCalendarViewModel ViewModel { get; set; }

        public SprintCalendarControl(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        protected override void DoDisplay()
        {
            if (!ViewModel.IsVisible)
                return;

            DisplayTable();
            DisplayNotes();
        }

        private void DisplayTable()
        {
            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = "Sprint Calendar";

            Column dateColumn = dataGrid.Columns.Add("Date");
            dateColumn.HeaderCell.PaddingLeft = 3;
            
            Column workColumn = new("Work")
            {
                CellHorizontalAlignment = HorizontalAlignment.Right
            };
            dataGrid.Columns.Add(workColumn);

            dataGrid.Columns.Add(string.Empty);

            Column vacationColumn = new("Absence")
            {
                CellHorizontalAlignment = HorizontalAlignment.Right
            };
            dataGrid.Columns.Add(vacationColumn);

            dataGrid.Columns.Add("Absence Details");

            Chart chart = CreateChart();
            using IEnumerator<ChartBar> chartBarEnumerator = chart.GetEnumerator();

            IEnumerable<ContentRow> rows = ViewModel.CalendarItems
                .Select(x =>
                {
                    bool success = chartBarEnumerator.MoveNext();

                    ChartBar chartBar = success
                        ? chartBarEnumerator.Current
                        : new ChartBar();

                    return CreateContentRow(x, chartBar);
                });

            foreach (ContentRow dataRow in rows)
                dataGrid.Rows.Add(dataRow);

            dataGrid.Display();
        }

        private Chart CreateChart()
        {
            Chart chart = new();

            IEnumerable<ChartBar> chartBars = ViewModel.CalendarItems
                .Select(x => new ChartBar
                {
                    MaxValue = x.WorkHours + x.AbsenceHours,
                    Value = x.WorkHours
                });

            foreach (ChartBar chartBar in chartBars)
                chart.Add(chartBar);

            return chart;
        }

        private static ContentRow CreateContentRow(CalendarItemViewModel calendarItem, ChartBar chartBar)
        {
            ContentRow dataRow = new();

            string arrow = calendarItem.IsToday ? "»" : " ";
            dataRow.AddCell($"{arrow} {calendarItem.Date:d} ({calendarItem.Date:ddd})");

            ContentCell workHoursCell = CreateWorkHoursCell(calendarItem);
            dataRow.AddCell(workHoursCell);

            ContentCell chartCell = CreateChartCell(chartBar);
            dataRow.AddCell(chartCell);

            ContentCell absenceCell = CreateAbsenceCell(calendarItem);
            dataRow.AddCell(absenceCell);

            ContentCell detailsCell = CreateDetailsCell(calendarItem);
            dataRow.AddCell(detailsCell);

            return dataRow;
        }

        private static ContentCell CreateWorkHoursCell(CalendarItemViewModel calendarItem)
        {
            return new ContentCell
            {
                Content = calendarItem.WorkHours.ToString(),
                ForegroundColor = calendarItem.WorkHours > 0
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

        private static ContentCell CreateAbsenceCell(CalendarItemViewModel calendarItem)
        {
            return new ContentCell
            {
                Content = calendarItem.AbsenceHours.ToString(),
                ForegroundColor = calendarItem.AbsenceHours > 0
                    ? ConsoleColor.Yellow
                    : null
            };
        }

        private static ContentCell CreateDetailsCell(CalendarItemViewModel calendarItem)
        {
            return new ContentCell
            {
                Content = calendarItem.AbsenceDetails.ToString(),
                ForegroundColor = ConsoleColor.Yellow
            };
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