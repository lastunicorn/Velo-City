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
using DustInTheWind.VeloCity.Domain.DataAccess;
using DustInTheWind.VeloCity.JsonFiles;

namespace DustInTheWind.VeloCity.DataAccess
{
    internal static class VacationDayExtensions
    {
        public static IEnumerable<JVacationDay> ToJEntities(this IEnumerable<Vacation> vacationDays)
        {
            return vacationDays?
                .Select(x => x.ToJEntity());
        }

        public static JVacationDay ToJEntity(this Vacation vacation)
        {
            switch (vacation)
            {
                case VacationOnce vacationOnce:
                    return new JVacationDay
                    {
                        Recurrence = JVacationRecurrence.Once,
                        Date = vacationOnce.Date,
                        HourCount = vacationOnce.HourCount,
                        Comments = vacationOnce.Comments
                    };

                case VacationDaily vacationDaily:
                    return new JVacationDay
                    {
                        Recurrence = JVacationRecurrence.Daily,
                        StartDate = vacationDaily.DateInterval.StartDate,
                        EndDate = vacationDaily.DateInterval.EndDate,
                        HourCount = vacationDaily.HourCount,
                        Comments = vacationDaily.Comments
                    };

                case VacationMonthly vacationMonthly:
                    return new JVacationDay
                    {
                        Recurrence = JVacationRecurrence.Monthly,
                        StartDate = vacationMonthly.DateInterval.StartDate,
                        EndDate = vacationMonthly.DateInterval.EndDate,
                        MonthDays = vacationMonthly.MonthDays,
                        HourCount = vacationMonthly.HourCount,
                        Comments = vacationMonthly.Comments
                    };

                case VacationWeekly vacationWeekly:
                    return new JVacationDay
                    {
                        Recurrence = JVacationRecurrence.Weekly,
                        StartDate = vacationWeekly.DateInterval.StartDate,
                        EndDate = vacationWeekly.DateInterval.EndDate,
                        WeekDays = vacationWeekly.WeekDays
                            .Select(x => x.ToJEntity())
                            .ToList(),
                        HourCount = vacationWeekly.HourCount,
                        Comments = vacationWeekly.Comments
                    };

                case VacationYearly vacationYearly:
                    return new JVacationDay
                    {
                        Recurrence = JVacationRecurrence.Yearly,
                        StartDate = vacationYearly.DateInterval.StartDate,
                        EndDate = vacationYearly.DateInterval.EndDate,
                        Dates = vacationYearly.Dates,
                        HourCount = vacationYearly.HourCount,
                        Comments = vacationYearly.Comments
                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(vacation));
            }
        }

        public static IEnumerable<Vacation> ToEntities(this IEnumerable<JVacationDay> vacationDays)
        {
            return vacationDays?
                .Select(x => x.ToEntity());
        }

        public static Vacation ToEntity(this JVacationDay vacationDay)
        {
            switch (vacationDay.Recurrence)
            {
                case JVacationRecurrence.Once:
                    if (vacationDay.Date == null)
                        throw new DataAccessException("Missing date for the vacation with recurrence 'once'.");

                    return new VacationOnce
                    {
                        Date = vacationDay.Date.Value,
                        HourCount = vacationDay.HourCount,
                        Comments = vacationDay.Comments
                    };

                case JVacationRecurrence.Daily:
                    return new VacationDaily
                    {
                        DateInterval = new DateInterval(vacationDay.StartDate, vacationDay.EndDate),
                        HourCount = vacationDay.HourCount,
                        Comments = vacationDay.Comments
                    };

                case JVacationRecurrence.Weekly:
                    return new VacationWeekly
                    {
                        DateInterval = new DateInterval(vacationDay.StartDate, vacationDay.EndDate),
                        WeekDays = vacationDay.WeekDays
                            .Select(x => x.ToEntity())
                            .ToList(),
                        HourCount = vacationDay.HourCount,
                        Comments = vacationDay.Comments
                    };

                case JVacationRecurrence.Monthly:
                    return new VacationMonthly
                    {
                        DateInterval = new DateInterval(vacationDay.StartDate, vacationDay.EndDate),
                        MonthDays = vacationDay.MonthDays,
                        HourCount = vacationDay.HourCount,
                        Comments = vacationDay.Comments
                    };

                case JVacationRecurrence.Yearly:
                    return new VacationYearly
                    {
                        DateInterval = new DateInterval(vacationDay.StartDate, vacationDay.EndDate),
                        Dates = vacationDay.Dates,
                        HourCount = vacationDay.HourCount,
                        Comments = vacationDay.Comments
                    };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}