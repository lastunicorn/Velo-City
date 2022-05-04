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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.Vacations
{
    public class TeamMemberVacationViewModel
    {
        public PersonName PersonName { get; }

        public List<VacationViewModel> Vacations { get; }

        public SortedList<DateTime, List<VacationViewModel>> VacationsMyMonth { get; }

        public TeamMemberVacationViewModel(TeamMemberVacations teamMemberVacations)
        {
            if (teamMemberVacations == null) throw new ArgumentNullException(nameof(teamMemberVacations));

            PersonName = teamMemberVacations.PersonName;
            Vacations = teamMemberVacations.Vacations
                .Select(VacationViewModel.From)
                .ToList();
            VacationsMyMonth = GroupVacationsByMonth();
        }

        private SortedList<DateTime, List<VacationViewModel>> GroupVacationsByMonth()
        {
            Dictionary<DateTime, List<VacationViewModel>> vacationByMonth = Vacations
                .GroupBy(x =>
                {
                    DateTime? dateTime = x.SignificantDate;

                    int year = dateTime?.Year ?? 1;
                    int month = dateTime?.Month ?? 1;

                    return new DateTime(year, month, 1);
                })
                .OrderByDescending(x => x.Key)
                .ToDictionary(x => x.Key, x => x.ToList());

            return new SortedList<DateTime, List<VacationViewModel>>(vacationByMonth);
        }
    }
}