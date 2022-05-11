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
using System.Globalization;

namespace DustInTheWind.VeloCity.Presentation.Commands.Vacations
{
    public readonly struct DateTimeMonth : IComparable<DateTimeMonth>, IFormattable
    {
        public int Year { get; }

        public int Month { get; }

        public DateTimeMonth(int year, int month)
        {
            if (month is <= 0 or > 12) throw new ArgumentOutOfRangeException(nameof(month));

            Year = year;
            Month = month;
        }

        public DateTimeMonth(DateTime dateTime)
        {
            Year = dateTime.Year;
            Month = dateTime.Month;
        }

        public DateTimeMonth(DateTime? dateTime)
        {
            Year = dateTime?.Year ?? 1;
            Month = dateTime?.Month ?? 1;
        }

        public int CompareTo(DateTimeMonth other)
        {
            int yearComparison = Year.CompareTo(other.Year);
            if (yearComparison != 0) return yearComparison;
            return Month.CompareTo(other.Month);
        }

        public DateTimeMonth AddMonths(int count)
        {
            if (count == 0)
                return new DateTimeMonth(Year, Month);

            int totalMonthsToAdd = (Month - 1) + count;

            int newYear = Year + (totalMonthsToAdd / 12);
            int newMonth = totalMonthsToAdd % 12;

            if (totalMonthsToAdd < 0)
            {
                newYear -= 1;
                newMonth += 12;
            }

            return new DateTimeMonth(newYear, newMonth + 1);
        }

        public override string ToString()
        {
            return ToString("number", CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "number";

            formatProvider ??= CultureInfo.CurrentCulture;

            switch (format)
            {
                case "number":
                    return $"{Year:D4} {Month:D2}";

                case "short-name":
                    return Year.ToString("D4", formatProvider) + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);

                case "long-name":
                    return Year.ToString("D4", formatProvider) + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Month);

                default:
                    throw new FormatException($"The {format} format string is not supported.");
            }
        }

        public static bool operator >(DateTimeMonth dateTimeMonth, DateTime dateTime)
        {
            int yearComparison = dateTimeMonth.Year.CompareTo(dateTime.Year);

            if (yearComparison > 0)
                return true;

            if (yearComparison < 0)
                return false;

            int monthComparison = dateTimeMonth.Month.CompareTo(dateTime.Month);
            return monthComparison > 0;
        }

        public static bool operator <(DateTimeMonth dateTimeMonth, DateTime dateTime)
        {
            int yearComparison = dateTimeMonth.Year.CompareTo(dateTime.Year);

            if (yearComparison < 0)
                return true;

            if (yearComparison > 0)
                return false;

            int monthComparison = dateTimeMonth.Month.CompareTo(dateTime.Month);
            return monthComparison < 0;
        }

        public static bool operator >=(DateTimeMonth dateTimeMonth, DateTime dateTime)
        {
            int yearComparison = dateTimeMonth.Year.CompareTo(dateTime.Year);

            if (yearComparison > 0)
                return true;

            if (yearComparison < 0)
                return false;

            int monthComparison = dateTimeMonth.Month.CompareTo(dateTime.Month);
            return monthComparison >= 0;
        }

        public static bool operator <=(DateTimeMonth dateTimeMonth, DateTime dateTime)
        {
            int yearComparison = dateTimeMonth.Year.CompareTo(dateTime.Year);

            if (yearComparison < 0)
                return true;

            if (yearComparison > 0)
                return false;

            int monthComparison = dateTimeMonth.Month.CompareTo(dateTime.Month);
            return monthComparison <= 0;
        }
    }
}