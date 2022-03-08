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

using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.JsonFiles;

namespace DustInTheWind.VeloCity.DataAccess
{
    internal static class OvertimeDayExtensions
    {
        public static IEnumerable<JOvertimeDay> ToJEntities(this IEnumerable<OvertimeDay> overtimeDays)
        {
            return overtimeDays?
                .Select(x => x.ToJEntity());
        }
        
        public static JOvertimeDay ToJEntity(this OvertimeDay overtimeDay)
        {
            return new JOvertimeDay
            {
                Date = overtimeDay.Date,
                HourCount = overtimeDay.HourCount,
                Comments = overtimeDay.Comments
            };
        }

        public static IEnumerable<OvertimeDay> ToEntities(this IEnumerable<JOvertimeDay> overtimeDays)
        {
            return overtimeDays?
                .Select(x => x.ToEntity());
        }
        
        public static OvertimeDay ToEntity(this JOvertimeDay overtimeDay)
        {
            return new OvertimeDay
            {
                Date = overtimeDay.Date,
                HourCount = overtimeDay.HourCount,
                Comments = overtimeDay.Comments
            };
        }
    }
}