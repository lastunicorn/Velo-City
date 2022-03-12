﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.VeloCity.Domain
{
    public class SprintDays
    {
        private readonly Sprint sprint;
        private readonly List<OfficialHoliday> officialHolidays;

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public SprintDays(Sprint sprint, List<OfficialHoliday> officialHolidays)
        {
            this.sprint = sprint ?? throw new ArgumentNullException(nameof(sprint));
            this.officialHolidays = officialHolidays ?? throw new ArgumentNullException(nameof(officialHolidays));

            StartDate = sprint.StartDate;
            EndDate = sprint.EndDate;
        }

        public IEnumerable<SprintDay> EnumerateAllDays()
        {
            List<DateTime> officialHolidayDates = officialHolidays
                .Select(x => x.Date)
                .ToList();

            int totalDaysCount = (int)(sprint.EndDate.Date - sprint.StartDate.Date).TotalDays + 1;

            return Enumerable.Range(0, totalDaysCount)
                .Select(x =>
                {
                    DateTime date = sprint.StartDate.AddDays(x);

                    return new SprintDay
                    {
                        Date = date,
                        IsOfficialHoliday = officialHolidayDates.Contains(date)
                    };
                });
        }

        public IEnumerable<DateTime> EnumerateWorkDates()
        {
            return EnumerateAllDays()
                .Where(x => x.IsWorkDay)
                .Select(x => x.Date)
                .ToList();
        }

        public IEnumerable<SprintDay> EnumerateWorkDays()
        {
            return EnumerateAllDays()
                .Where(x => x.IsWorkDay);
        }
    }
}