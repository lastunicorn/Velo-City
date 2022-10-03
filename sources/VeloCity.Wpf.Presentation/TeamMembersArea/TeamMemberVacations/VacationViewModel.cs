// VeloCity
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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberVacations;

namespace DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.TeamMemberVacations
{
    public abstract class VacationViewModel
    {
        public HoursValue? HourCount { get; set; }

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
                        HourCount = vacationOnce.HourCount.HasValue
                            ? vacationOnce.HourCount
                            : (HoursValue?)null,
                        Comments = vacationOnce.Comments
                    };

                case VacationDailyInfo vacationDaily:
                    return new VacationDailyViewModel
                    {
                        DateInterval = vacationDaily.DateInterval,
                        HourCount = vacationDaily.HourCount.HasValue
                            ? vacationDaily.HourCount
                            : (HoursValue?)null,
                        Comments = vacationDaily.Comments
                    };

                case VacationWeeklyInfo vacationWeekly:
                    return new VacationWeeklyViewModel
                    {
                        WeekDays = vacationWeekly.WeekDays,
                        DateInterval = vacationWeekly.DateInterval,
                        HourCount = vacationWeekly.HourCount.HasValue
                            ? vacationWeekly.HourCount
                            : (HoursValue?)null,
                        Comments = vacationWeekly.Comments
                    };

                case VacationMonthlyInfo vacationMonthly:
                    return new VacationMonthlyViewModel
                    {
                        MonthDays = vacationMonthly.MonthDays,
                        DateInterval = vacationMonthly.DateInterval,
                        HourCount = vacationMonthly.HourCount.HasValue
                            ? vacationMonthly.HourCount
                            : (HoursValue?)null,
                        Comments = vacationMonthly.Comments
                    };

                case VacationYearlyInfo vacationYearly:
                    return new VacationYearlyViewModel
                    {
                        Dates = vacationYearly.Dates,
                        DateInterval = vacationYearly.DateInterval,
                        HourCount = vacationYearly.HourCount.HasValue
                            ? vacationYearly.HourCount
                            : (HoursValue?)null,
                        Comments = vacationYearly.Comments
                    };

                default:
                    throw new ArgumentOutOfRangeException(nameof(vacation));
            }
        }
    }
}