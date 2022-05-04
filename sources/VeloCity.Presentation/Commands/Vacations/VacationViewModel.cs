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
using System.Text;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.Vacations
{
    public abstract class VacationViewModel
    {
        public int? HourCount { get; set; }

        public string Comments { get; set; }

        public abstract DateTime? SignificantDate { get; }

        public override string ToString()
        {
            StringBuilder sb = new();
            string dateAsString = RenderDate();
            sb.Append($"{dateAsString}");

            if (HourCount != null)
                sb.Append($" ({HourCount}h)");

            if (Comments != null)
                sb.Append($" - {Comments}");

            return sb.ToString();
        }

        protected abstract string RenderDate();

        public static VacationViewModel From(Vacation vacation)
        {
            switch (vacation)
            {
                case VacationOnce vacationOnce:
                    return new VacationOnceViewModel
                    {
                        Date = vacationOnce.Date,
                        HourCount = vacationOnce.HourCount,
                        Comments = vacationOnce.Comments
                    };

                case VacationDaily vacationDaily:
                    return new VacationDailyViewModel
                    {
                        DateInterval = vacationDaily.DateInterval,
                        HourCount = vacationDaily.HourCount,
                        Comments = vacationDaily.Comments
                    };

                case VacationWeekly vacationWeekly:
                    return new VacationWeeklyViewModel
                    {
                        WeekDays = vacationWeekly.WeekDays,
                        DateInterval = vacationWeekly.DateInterval,
                        HourCount = vacationWeekly.HourCount,
                        Comments = vacationWeekly.Comments
                    };

                case VacationMonthly vacationMonthly:
                    return new VacationMonthlyViewModel
                    {
                        MonthDays = vacationMonthly.MonthDays,
                        DateInterval = vacationMonthly.DateInterval,
                        HourCount = vacationMonthly.HourCount,
                        Comments = vacationMonthly.Comments
                    };

                case VacationYearly vacationYearly:
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