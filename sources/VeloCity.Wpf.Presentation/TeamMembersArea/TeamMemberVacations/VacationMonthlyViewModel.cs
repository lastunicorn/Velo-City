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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.TeamMemberVacations
{
    public class VacationMonthlyViewModel : VacationViewModel
    {
        public List<int> MonthDays { get; set; }

        public DateInterval DateInterval { get; set; }

        public override DateTime? SignificantDate => DateInterval.StartDate;

        public override DateTime? StartDate => DateInterval.StartDate;

        public override DateTime? EndDate => DateInterval.EndDate;

        public override string ToString()
        {
            string monthDaysString = MonthDays == null || MonthDays.Count == 0
                ? "<none>"
                : string.Join(", ", MonthDays);

            return $"Each {monthDaysString} of the month between [{DateInterval}]" + (Comments == null ? string.Empty : " - " + Comments);
        }
    }
}