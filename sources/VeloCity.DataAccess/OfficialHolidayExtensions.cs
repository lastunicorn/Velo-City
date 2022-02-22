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
using VeloCity.DataAccess.Jsonfiles;

namespace DustInTheWind.VeloCity.DataAccess
{
    internal static class OfficialHolidayExtensions
    {
        public static IEnumerable<JOfficialHoliday> ToJEntities(this IEnumerable<OfficialHoliday> officialHolidays)
        {
            return officialHolidays
                .Select(x => x.ToJEntity());
        }

        public static JOfficialHoliday ToJEntity(this OfficialHoliday officialHoliday)
        {
            return new JOfficialHoliday
            {
                Date = officialHoliday.Date,
                Name = officialHoliday.Name
            };
        }

        public static IEnumerable<OfficialHoliday> ToEntities(this IEnumerable<JOfficialHoliday> officialHolidays)
        {
            return officialHolidays
                .Select(x => x.ToEntity());
        }

        public static OfficialHoliday ToEntity(this JOfficialHoliday officialHoliday)
        {
            return new OfficialHoliday
            {
                Date = officialHoliday.Date,
                Name = officialHoliday.Name
            };
        }
    }
}