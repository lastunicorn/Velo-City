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
    public class OfficialHolidayYearly : OfficialHoliday
    {
        public int? StartYear { get; set; }

        public int? EndYear { get; set; }

        public override bool Match(int year)
        {
            return (StartYear == null || year >= StartYear) && (EndYear == null || year <= EndYear);
        }

        public override bool Match(DateTime date)
        {
            return Date.Month == date.Month && Date.Day == date.Day;
        }

        public override bool Match(DateTime startDate, DateTime endDate)
        {
            int startYear = startDate.Year;
            DateTime firstInstanceDate = new(startYear, Date.Month, Date.Day);

            if (firstInstanceDate >= startDate && firstInstanceDate <= endDate)
                return true;

            int endYear = endDate.Year;
            DateTime lastInstanceDate = new(endYear, Date.Month, Date.Day);

            if (lastInstanceDate >= startDate && lastInstanceDate <= endDate)
                return true;

            if (endYear - startYear >= 2)
            {
                int middleYear = startDate.Year + 1;
                DateTime middleInstanceDate = new(middleYear, Date.Month, Date.Day);

                if (middleInstanceDate >= startDate && middleInstanceDate <= endDate)
                    return true;
            }

            return false;
        }

        public override OfficialHolidayInstance GetInstanceFor(int year)
        {
            bool isMatch = (StartYear == null || year >= StartYear) && (EndYear == null || year <= EndYear);

            if (!isMatch)
                return null;

            return new OfficialHolidayInstance
            {
                Date = new DateTime(year, Date.Month, Date.Day),
                Name = Name
            };
        }
    }
}