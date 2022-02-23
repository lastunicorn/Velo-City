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
    internal static class VacationDayExtensions
    {
        public static IEnumerable<JVacationDay> ToJEntities(this IEnumerable<VacationDay> vacationDays)
        {
            return vacationDays?
                .Select(x => x.ToJEntity());
        }
        
        public static JVacationDay ToJEntity(this VacationDay vacationDay)
        {
            return new JVacationDay
            {
                Date = vacationDay.Date,
                HourCount = vacationDay.HourCount,
                Comments = vacationDay.Comments
            };
        }

        public static IEnumerable<VacationDay> ToEntities(this IEnumerable<JVacationDay> vacationDays)
        {
            return vacationDays?
                .Select(x => x.ToEntity());
        }
        
        public static VacationDay ToEntity(this JVacationDay vacationDay)
        {
            return new VacationDay
            {
                Date = vacationDay.Date,
                HourCount = vacationDay.HourCount,
                Comments = vacationDay.Comments
            };
        }
    }
}