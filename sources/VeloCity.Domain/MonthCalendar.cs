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
using DustInTheWind.VeloCity.Domain.DataAccess;

namespace DustInTheWind.VeloCity.Domain
{
    public class MonthCalendar
    {
        private readonly IUnitOfWork unitOfWork;

        public int Year { get; }

        public int Month { get; }

        public List<SprintDay> Days { get; }

        public MonthCalendar(IUnitOfWork unitOfWork, DateTime startDate, DateTime endDate)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            Year = startDate.Year;
            Month = startDate.Month;
            Days = EnumerateAllDays(startDate, endDate).ToList();
        }

        private IEnumerable<SprintDay> EnumerateAllDays(DateTime startDate, DateTime endDate)
        {
            int totalDaysCount = (int)(endDate.Date - startDate.Date).TotalDays + 1;

            return Enumerable.Range(0, totalDaysCount)
                .Select(x =>
                {
                    DateTime date = startDate.AddDays(x);
                    return ToSprintDay(date);
                });
        }

        private SprintDay ToSprintDay(DateTime date)
        {
            List<OfficialHoliday> officialHolidays = unitOfWork.OfficialHolidayRepository.GetAll()
                .ToList();

            return new SprintDay
            {
                Date = date,
                OfficialHolidays = officialHolidays
                    .Where(x => x.Match(date))
                    .Select(x => x.GetInstanceFor(date.Year))
                    .ToList()
            };
        }
    }
}