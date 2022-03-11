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

namespace DustInTheWind.VeloCity.Domain
{
    public class Sprint
    {
        public int Number { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CommitmentStoryPoints { get; set; }

        public int ActualStoryPoints { get; set; }

        public List<OfficialHoliday> OfficialHolidays { get; set; }

        public SprintState State { get; set; }

        public int CountWorkDays()
        {
            return EnumerateWorkDays().Count();
        }

        public IEnumerable<DateTime> EnumerateWorkDates()
        {
            return EnumerateAllDays()
                .Where(x => x.IsWorkDay)
                .Select(x => x.Date);
        }

        public IEnumerable<SprintDay> EnumerateWorkDays()
        {
            return EnumerateAllDays()
                .Where(x => x.IsWorkDay);
        }

        public IEnumerable<SprintDay> EnumerateAllDays()
        {
            List<DateTime> officialHolidays = OfficialHolidays
                .Select(x => x.Date)
                .ToList();

            int totalDaysCount = (int)(EndDate.Date - StartDate.Date).TotalDays + 1;

            return Enumerable.Range(0, totalDaysCount)
                .Select(x =>
                {
                    DateTime date = StartDate.AddDays(x);
                    return new SprintDay
                    {
                        Date = date,
                        IsWeekEnd = date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
                        IsOfficialHoliday = officialHolidays.Contains(date)
                    };
                });
        }
    }
}