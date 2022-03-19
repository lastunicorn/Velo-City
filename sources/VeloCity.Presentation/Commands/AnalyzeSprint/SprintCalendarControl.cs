﻿// Velo City
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
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
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

            dataGrid.Columns.Add("Date");

            Column workColumn = new("Work")
            {
                CellHorizontalAlignment = HorizontalAlignment.Right
            };
            dataGrid.Columns.Add(workColumn);

            dataGrid.Columns.Add(string.Empty);

            Column vacationColumn = new("Vacation")
            {
                CellHorizontalAlignment = HorizontalAlignment.Right
            };
            dataGrid.Columns.Add(vacationColumn);

            dataGrid.Columns.Add("Vacation Details");

            foreach (CalendarItemViewModel calendarItem in ViewModel.CalendarItems)
            {
                ContentRow dataRow = CreateContentRow(calendarItem);
                dataGrid.Rows.Add(dataRow);
            }

            dataGrid.Display();
        }

        private void DisplayNotes()
        {
            if (ViewModel.IsPartialVacationNoteVisible)
            {
                CustomConsole.WriteLine();
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, "Notes:");
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, "  - (*) = partial day vacation");
            }
        }

        private static ContentRow CreateContentRow(CalendarItemViewModel calendarItem)
        {
            ContentRow dataRow = new();

            dataRow.AddCell($"{calendarItem.Date}");

            ContentCell workHoursCell = CreateWorkHoursCell(calendarItem);
            dataRow.AddCell(workHoursCell);

            ContentCell chartCell = CreateChartCell(calendarItem);
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

        private static ContentCell CreateChartCell(CalendarItemViewModel calendarItem)
        {
            ChartBar chartBar = new()
            {
                MaxValue = calendarItem.ChartItem.MaxValue,
                Value = calendarItem.ChartItem.Value
            };

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
            List<string> absentTeamMemberNames = calendarItem.VacationDetails
                .Select(x => x.IsPartialVacation
                    ? x.Name + "(*)"
                    : x.Name)
                .ToList();

            return new ContentCell
            {
                Content = string.Join(", ", absentTeamMemberNames),
                ForegroundColor = ConsoleColor.Yellow
            };
        }
    }
}