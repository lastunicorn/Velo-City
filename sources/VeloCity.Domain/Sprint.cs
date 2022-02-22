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
        public int Id { get; set; }

        public string Name { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        public int StoryPoints { get; set; }

        public List<OfficialHoliday> OfficialFreeDays { get; set; }

        public IEnumerable<DateTime> CalculateWorkDays()
        {
            List<DateTime> officialFreeDays = OfficialFreeDays
                .Select(x => x.Date)
                .ToList();

            int totalDaysCount = (int)(EndDate.Date - StartDate.Date).TotalDays + 1;

            return Enumerable.Range(0, totalDaysCount)
                .Select(x => StartDate.AddDays(x))
                .Where(x =>
                {
                    bool isWeekEnd = x.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
                    if (isWeekEnd)
                        return false;

                    bool isOfficialFreeDay = officialFreeDays.Contains(x);
                    if (isOfficialFreeDay)
                        return false;

                    return true;
                });
        }
    }
}