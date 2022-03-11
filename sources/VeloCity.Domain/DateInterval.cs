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

namespace DustInTheWind.VeloCity.Domain
{
    public readonly struct DateInterval
    {
        public DateTime? StartDate { get; }

        public DateTime? EndDate { get; }

        public bool IsAllTime => StartDate == null && EndDate == null;

        public DateInterval(DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate?.Date;
            EndDate = endDate?.Date;
        }

        public bool IsIntersecting(DateInterval dateInterval)
        {
            if (dateInterval.IsAllTime)
                return true;

            if (dateInterval.StartDate == null)
                return StartDate == null || dateInterval.EndDate >= StartDate;

            if (dateInterval.EndDate == null)
                return EndDate == null || dateInterval.StartDate <= EndDate;

            return IsInRange(dateInterval.EndDate.Value) || ((EndDate == null || dateInterval.EndDate > EndDate) && dateInterval.StartDate <= EndDate);
        }

        public bool IsIntersecting(DateTime? startDate, DateTime? endDate)
        {
            DateInterval dateInterval = new(startDate, endDate);
            return IsIntersecting(dateInterval);
        }

        public bool IsInRange(DateTime dateTime)
        {
            DateTime date = dateTime.Date;

            return (StartDate == null || date >= StartDate) &&
                   (EndDate == null || date <= EndDate);
        }

        public override string ToString()
        {
            string startDateString = StartDate?.ToString("d") ?? "<<<";
            string endDateString = EndDate?.ToString("d") ?? ">>>";
            return $"{startDateString} - {endDateString}";
        }
    }
}