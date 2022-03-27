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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.JsonFiles;

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
            switch (officialHoliday)
            {
                case OfficialHolidayOnce officialHolidayOnce:
                    return new JOfficialHoliday
                    {
                        Date = officialHolidayOnce.Date,
                        Name = officialHolidayOnce.Name,
                        Description = officialHolidayOnce.Description
                    };

                case OfficialHolidayYearly officialHolidayYearly:
                    return new JOfficialHoliday
                    {
                        Date = officialHolidayYearly.Date,
                        Name = officialHolidayYearly.Name,
                        StartYear = officialHolidayYearly.StartYear,
                        EndYear = officialHolidayYearly.EndYear,
                        Description = officialHolidayYearly.Description
                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(officialHoliday));
            }
        }

        public static IEnumerable<OfficialHoliday> ToEntities(this IEnumerable<JOfficialHoliday> officialHolidays)
        {
            return officialHolidays
                .Select(x => x.ToEntity());
        }

        public static OfficialHoliday ToEntity(this JOfficialHoliday officialHoliday)
        {
            switch (officialHoliday.Recurrence)
            {
                case JOfficialHolidayRecurrence.Once:
                    return new OfficialHolidayOnce
                    {
                        Date = officialHoliday.Date,
                        Name = officialHoliday.Name,
                        Description = officialHoliday.Description
                    };

                case JOfficialHolidayRecurrence.Yearly:
                    return new OfficialHolidayYearly
                    {
                        Date = officialHoliday.Date,
                        Name = officialHoliday.Name,
                        StartYear = officialHoliday.StartYear,
                        EndYear = officialHoliday.EndYear,
                        Description = officialHoliday.Description
                    };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}