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

namespace DustInTheWind.VeloCity.Domain
{
    public class Employment
    {
        public DateInterval TimeInterval { get; set; }

        public int HoursPerDay { get; set; }

        public List<DayOfWeek> WeekDays { get; set; }

        public string Country { get; set; }

        public bool IsDateInRange(DateTime dateTime)
        {
            return TimeInterval.ContainsDate(dateTime);
        }

        public bool MatchDayOfWeek(DayOfWeek dayOfWeek)
        {
            return WeekDays == null || WeekDays.Count == 0 || WeekDays.Contains(dayOfWeek);
        }

        public bool ContinuesWith(Employment employment)
        {
            if (employment == null) throw new ArgumentNullException(nameof(employment));

            if (TimeInterval.EndDate == null || TimeInterval.EndDate == DateTime.MaxValue.Date || employment.TimeInterval.StartDate == null)
                return false;

            return TimeInterval.EndDate.Value.Date.AddDays(1) == employment.TimeInterval.StartDate.Value;
        }
    }
}