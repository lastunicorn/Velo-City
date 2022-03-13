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
using DustInTheWind.VeloCity.Application.PresentVacations;

namespace DustInTheWind.VeloCity.Presentation.Commands.Vacations
{
    public class TeamMemberVacationViewModel
    {
        public string PersonName { get; }

        public List<VacationViewModel> Vacations { get; }

        public SortedList<DateTime, List<VacationViewModel>> VacationsMyMonth
        {
            get
            {
                Dictionary<DateTime, List<VacationViewModel>> vacationByMonth = Vacations
                    .GroupBy(x =>
                    {
                        DateTime? dateTime = CalculateSignificantDateFor(x);
                        return new DateTime(dateTime?.Year ?? 1, dateTime?.Month ?? 1, 1);
                    })
                    .OrderByDescending(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return new SortedList<DateTime, List<VacationViewModel>>(vacationByMonth);
            }
        }

        public TeamMemberVacationViewModel(TeamMemberVacation teamMemberVacation)
        {
            if (teamMemberVacation == null) throw new ArgumentNullException(nameof(teamMemberVacation));

            PersonName = teamMemberVacation.PersonName;
            Vacations = teamMemberVacation.Vacations
                .Select(VacationViewModel.From)
                .ToList();
        }

        private static DateTime? CalculateSignificantDateFor(VacationViewModel vacationViewModel)
        {
            switch (vacationViewModel)
            {
                case VacationOnceViewModel vacationOnceViewModel:
                    return vacationOnceViewModel.Date;
                
                case VacationDailyViewModel vacationDailyViewModel:
                    return vacationDailyViewModel.DateInterval.StartDate;
                
                case VacationWeeklyViewModel vacationWeeklyViewModel:
                    return vacationWeeklyViewModel.DateInterval.StartDate;
                
                case VacationMonthlyViewModel vacationMonthlyViewModel:
                    return vacationMonthlyViewModel.DateInterval.StartDate;
                
                case VacationYearlyViewModel vacationYearlyViewModel:
                    return vacationYearlyViewModel.DateInterval.StartDate;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(vacationViewModel));
            }
        }
    }
}