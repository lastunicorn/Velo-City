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
    internal static class EmploymentExtensions
    {
        public static IEnumerable<JEmployment> ToJEntities(this IEnumerable<Employment> employments)
        {
            return employments?
                .Select(x => x.ToJEntity());
        }

        public static JEmployment ToJEntity(this Employment employment)
        {
            return new JEmployment
            {
                StartDate = employment.TimeInterval.StartDate,
                EndDate = employment.TimeInterval.EndDate,
                HoursPerDay = employment.HoursPerDay,
                WeekDays = employment.EmploymentWeek.ToList(),
                Country = employment.Country
            };
        }

        public static IEnumerable<Employment> ToEntities(this IEnumerable<JEmployment> employments)
        {
            return employments?
                .Select(x => x.ToEntity());
        }

        public static Employment ToEntity(this JEmployment employment)
        {
            return new Employment
            {
                TimeInterval = new DateInterval(employment.StartDate, employment.EndDate),
                HoursPerDay = employment.HoursPerDay,
                EmploymentWeek = new EmploymentWeek(employment.WeekDays),
                Country = employment.Country
            };
        }
    }
}