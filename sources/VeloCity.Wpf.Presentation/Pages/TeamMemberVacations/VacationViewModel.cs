﻿// Velo City
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
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberVacations;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.TeamMemberVacations
{
    public abstract class VacationViewModel
    {
        public int? HourCount { get; set; }

        public string Comments { get; set; }

        public abstract DateTime? SignificantDate { get; }

        public abstract DateTime? StartDate { get; }

        public abstract DateTime? EndDate { get; }

        public static VacationViewModel From(VacationInfo vacation)
        {
            switch (vacation)
            {
                case VacationOnceInfo vacationOnce:
                    return new VacationOnceViewModel
                    {
                        Date = vacationOnce.Date,
                        HourCount = vacationOnce.HourCount,
                        Comments = vacationOnce.Comments
                    };

                case VacationDailyInfo vacationDaily:
                    return new VacationDailyViewModel
                    {
                        DateInterval = vacationDaily.DateInterval,
                        HourCount = vacationDaily.HourCount,
                        Comments = vacationDaily.Comments
                    };

                case VacationWeeklyInfo vacationWeekly:
                    return new VacationWeeklyViewModel
                    {
                        WeekDays = vacationWeekly.WeekDays,
                        DateInterval = vacationWeekly.DateInterval,
                        HourCount = vacationWeekly.HourCount,
                        Comments = vacationWeekly.Comments
                    };

                case VacationMonthlyInfo vacationMonthly:
                    return new VacationMonthlyViewModel
                    {
                        MonthDays = vacationMonthly.MonthDays,
                        DateInterval = vacationMonthly.DateInterval,
                        HourCount = vacationMonthly.HourCount,
                        Comments = vacationMonthly.Comments
                    };

                case VacationYearlyInfo vacationYearly:
                    return new VacationYearlyViewModel
                    {
                        Dates = vacationYearly.Dates,
                        DateInterval = vacationYearly.DateInterval,
                        HourCount = vacationYearly.HourCount,
                        Comments = vacationYearly.Comments
                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(vacation));
            }
        }
    }
}