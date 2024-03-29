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

using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls.SprintCalendar;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Calendar;

public class CalendarView : IView<CalendarCommand>
{
    private readonly DataGridFactory dataGridFactory;

    public CalendarView(DataGridFactory dataGridFactory)
    {
        this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
    }

    public void Display(CalendarCommand command)
    {
        if (command.SprintCalendar != null)
            DisplaySprintCalendar(command.SprintCalendar);

        if (command.MonthCalendars != null)
            DisplayMonthCalendars(command.MonthCalendars);
    }

    private void DisplaySprintCalendar(SprintCalendar sprintCalendar)
    {
        SprintCalendarControl sprintCalendarControl = new(dataGridFactory)
        {
            ViewModel = new SprintCalendarViewModel(sprintCalendar.Days, sprintCalendar.SprintMembers)
            {
                Title = $"{sprintCalendar.SprintName} ({sprintCalendar.StartDate:d} - {sprintCalendar.EndDate:d})"
            }
        };

        sprintCalendarControl.Display();
    }

    private void DisplayMonthCalendars(List<MonthCalendar> monthCalendars)
    {
        foreach (MonthCalendar monthCalendar in monthCalendars)
            DisplayMonthCalendar(monthCalendar);
    }

    private void DisplayMonthCalendar(MonthCalendar monthCalendar)
    {
        List<SprintDay> monthDays = monthCalendar.EnumerateAllDays()
            .ToList();

        SprintCalendarControl sprintCalendarControl = new(dataGridFactory)
        {
            ViewModel = new SprintCalendarViewModel(monthDays, monthCalendar.MonthMembers)
            {
                Title = $"{monthCalendar.Year:D4} {monthCalendar.Month:D2}"
            }
        };
        sprintCalendarControl.Display();
    }
}