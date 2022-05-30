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
using System.Collections;
using System.Collections.Generic;

namespace DustInTheWind.VeloCity.Domain
{
    public class MonthEnumeration : IEnumerable<DateInterval>
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Count { get; set; }

        public IEnumerator<DateInterval> GetEnumerator()
        {
            return new MonthEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class MonthEnumerator : IEnumerator<DateInterval>
        {
            private DateTime date;
            private int actualCount;

            private readonly MonthEnumeration monthEnumeration;

            public MonthEnumerator(MonthEnumeration monthEnumeration)
            {
                this.monthEnumeration = monthEnumeration ?? throw new ArgumentNullException(nameof(monthEnumeration));
            }

            public DateInterval Current { get; private set; }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (monthEnumeration.Count != null && actualCount >= monthEnumeration.Count)
                    return false;

                if (date > monthEnumeration.EndDate)
                    return false;

                Current = CreateNextMonthInterval(date);

                date = Current.EndDate.Value.AddDays(1);
                actualCount++;

                return true;
            }

            public void Reset()
            {
                date = monthEnumeration.StartDate ?? DateTime.MinValue;
                actualCount = 0;
            }

            public void Dispose()
            {
            }

            private DateInterval CreateNextMonthInterval(DateTime startDate)
            {
                int daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                DateTime endDate = new(startDate.Year, startDate.Month, daysInMonth);

                if (endDate > monthEnumeration.EndDate)
                    endDate = monthEnumeration.EndDate.Value;

                return new DateInterval(startDate, endDate);
            }
        }
    }
}