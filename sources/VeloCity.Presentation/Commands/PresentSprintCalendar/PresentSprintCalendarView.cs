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
using System.Text;
using DustInTheWind.VeloCity.Application.PresentSprintCalendar;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentSprintCalendar
{
    public class PresentSprintCalendarView
    {
        public void Display(PresentSprintCalendarResponse response)
        {
            Console.WriteLine($"{response.SprintName} ({response.StartDate:d} - {response.EndDate:d})");
            Console.WriteLine(new string('=', 79));

            Console.WriteLine($"Sprint Days: {response.Days.Count} days");

            for (int i = 0; i < response.Days.Count; i++)
            {
                SprintDay sprintDay = response.Days[i];
                int dayIndex = i + 1;

                StringBuilder sb = new();
                sb.Append($"  - day {dayIndex:D2}: {sprintDay.Date:d} ({sprintDay.Date:dddd})");

                if (sprintDay.IsWeekEnd)
                    sb.Append(", week-end");

                if (sprintDay.IsOfficialHoliday)
                    sb.Append(", official holiday");

                Console.WriteLine(sb);
            }
        }
    }
}