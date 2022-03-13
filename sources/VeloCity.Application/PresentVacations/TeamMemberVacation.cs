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

namespace DustInTheWind.VeloCity.Application.PresentVacations
{
    public class TeamMemberVacation
    {
        public string PersonName { get; set; }

        public List<Vacation> Vacations { get; set; }

        public SortedList<DateTime, List<Vacation>> VacationsMyMonth
        {
            get
            {
                Dictionary<DateTime, List<Vacation>> vacationByMonth = Vacations
                    .GroupBy(x =>
                    {
                        DateTime? dateTime = CalculateSignificantDateFor(x);
                        return new DateTime(dateTime?.Year ?? 1, dateTime?.Month ?? 1, 1);
                    })
                    .OrderByDescending(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return new SortedList<DateTime, List<Vacation>>(vacationByMonth);
            }
        }

        private static DateTime? CalculateSignificantDateFor(Vacation vacation)
        {
            switch (vacation)
            {
                case VacationOnce vacationOnce:
                    return vacationOnce.Date;
                
                case VacationDaily vacationDaily:
                    return vacationDaily.DateInterval.StartDate;
                
                case VacationWeekly vacationWeekly:
                    return vacationWeekly.DateInterval.StartDate;
                
                case VacationMonthly vacationMonthly:
                    return vacationMonthly.DateInterval.StartDate;
                
                case VacationYearly vacationYearly:
                    return vacationYearly.DateInterval.StartDate;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(vacation));
            }
        }
    }
}