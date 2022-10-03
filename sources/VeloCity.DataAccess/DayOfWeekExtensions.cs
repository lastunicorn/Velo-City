// VeloCity
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
using DustInTheWind.VeloCity.JsonFiles;

namespace DustInTheWind.VeloCity.DataAccess
{
    internal static class DayOfWeekExtensions
    {
        public static JDayOfWeek ToJEntity(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Sunday => JDayOfWeek.Sunday,
                DayOfWeek.Monday => JDayOfWeek.Monday,
                DayOfWeek.Tuesday => JDayOfWeek.Tuesday,
                DayOfWeek.Wednesday => JDayOfWeek.Wednesday,
                DayOfWeek.Thursday => JDayOfWeek.Thursday,
                DayOfWeek.Friday => JDayOfWeek.Friday,
                DayOfWeek.Saturday => JDayOfWeek.Saturday,
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
            };
        }

        public static DayOfWeek ToEntity(this JDayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                JDayOfWeek.Sunday => DayOfWeek.Sunday,
                JDayOfWeek.Monday => DayOfWeek.Monday,
                JDayOfWeek.Tuesday => DayOfWeek.Tuesday,
                JDayOfWeek.Wednesday => DayOfWeek.Wednesday,
                JDayOfWeek.Thursday => DayOfWeek.Thursday,
                JDayOfWeek.Friday => DayOfWeek.Friday,
                JDayOfWeek.Saturday => DayOfWeek.Saturday,
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
            };
        }
    }
}