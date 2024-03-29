﻿// VeloCity
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

namespace DustInTheWind.VeloCity.Cli.Presentation.UserControls.SprintCalendar;

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
    }

    private void DisplayTable()
    {
        DataGrid dataGrid = dataGridFactory.Create();
        dataGrid.Title = ViewModel.Title;
        dataGrid.MaxWidth = 120;

        AddColumns(dataGrid);
        AddContentData(dataGrid);
        AddFooter(dataGrid);

        dataGrid.Display();
    }

    private void AddColumns(DataGrid dataGrid)
    {
        Column dateColumn = dataGrid.Columns.Add("Date");

        if (ViewModel.ContainsHighlightedItems)
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
    }

    private void AddContentData(DataGrid dataGrid)
    {
        SprintWorkChart chart = new(ViewModel.CalendarItems);
        using IEnumerator<ChartBarValue<CalendarItemViewModel>> chartBarEnumerator = chart.GetEnumerator();

        IEnumerable<ContentRow> rows = ViewModel.CalendarItems
            .Select(x =>
            {
                bool success = chartBarEnumerator.MoveNext();

                ChartBarValue<CalendarItemViewModel> chartBarValue = success
                    ? chartBarEnumerator.Current
                    : new ChartBarValue<CalendarItemViewModel>();

                return CreateContentRow(x, chartBarValue);
            });

        foreach (ContentRow dataRow in rows)
            dataGrid.Rows.Add(dataRow);
    }

    private ContentRow CreateContentRow(CalendarItemViewModel calendarItem, ChartBarValue<CalendarItemViewModel> chartBarValue)
    {
        ContentRow dataRow = new();

        ContentCell dateCell = CreateDateCell(calendarItem);
        dataRow.AddCell(dateCell);

        ContentCell workHoursCell = CreateWorkHoursCell(calendarItem);
        dataRow.AddCell(workHoursCell);

        ContentCell chartCell = CreateChartCell(chartBarValue);
        dataRow.AddCell(chartCell);

        ContentCell absenceCell = CreateAbsenceCell(calendarItem);
        dataRow.AddCell(absenceCell);

        ContentCell detailsCell = CreateDetailsCell(calendarItem);
        dataRow.AddCell(detailsCell);

        return dataRow;
    }

    private ContentCell CreateDateCell(CalendarItemViewModel calendarItem)
    {
        ContentCell cell = new();

        if (ViewModel.ContainsHighlightedItems)
        {
            string arrow = calendarItem.IsHighlighted ? "»" : " ";
            cell.Content = $"{arrow} {calendarItem.Date:d} ({calendarItem.Date:ddd})";
        }
        else
        {
            cell.Content = $"{calendarItem.Date:d} ({calendarItem.Date:ddd})";
        }

        if (!calendarItem.IsWorkDay)
            cell.ForegroundColor = ConsoleColor.DarkGray;

        return cell;
    }

    private static ContentCell CreateWorkHoursCell(CalendarItemViewModel calendarItem)
    {
        return new ContentCell
        {
            Content = calendarItem.IsWorkDay
                ? calendarItem.WorkHours.ToString()
                : string.Empty,
            ForegroundColor = calendarItem.WorkHours > 0
                ? ConsoleColor.Green
                : !calendarItem.IsWorkDay
                    ? ConsoleColor.DarkGray
                    : null
        };
    }

    private static ContentCell CreateChartCell(ChartBarValue<CalendarItemViewModel> chartBarValue)
    {
        return new ContentCell
        {
            Content = chartBarValue.ToString(),
            ForegroundColor = ConsoleColor.Green
        };
    }

    private static ContentCell CreateAbsenceCell(CalendarItemViewModel calendarItem)
    {
        return new ContentCell
        {
            Content = calendarItem.IsWorkDay
                ? calendarItem.AbsenceHours.ToString()
                : string.Empty,
            ForegroundColor = calendarItem.AbsenceHours > 0
                ? ConsoleColor.Yellow
                : !calendarItem.IsWorkDay
                    ? ConsoleColor.DarkGray
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