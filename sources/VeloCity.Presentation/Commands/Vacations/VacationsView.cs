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
using System.Linq;
using System.Text;
using DustInTheWind.VeloCity.Application.PresentVacations;

namespace DustInTheWind.VeloCity.Presentation.Commands.Vacations
{
    public class VacationsView
    {
        public void Display(PresentVacationsResponse response)
        {
            foreach (TeamMemberVacation teamMemberVacation in response.TeamMemberVacations)
            {
                Console.WriteLine();
                Console.WriteLine(new string('-', 79));
                Console.WriteLine(teamMemberVacation.PersonName);
                Console.WriteLine(new string('-', 79));

                //var groupedVacations = teamMemberVacation.Vacations
                //    .GroupBy(x => new
                //    {
                //        Year = x.Date.Year,
                //        Month = x.Date.Month
                //    })
                //    .OrderByDescending(x => x.Key.Year)
                //    .ThenByDescending(x => x.Key.Month);

                //foreach (var group in groupedVacations)
                //{
                //    Console.WriteLine($"{group.Key.Year}.{group.Key.Month}");

                //    foreach (VacationInfo vacationInfo in group)
                //    {
                //        StringBuilder sb = new();
                //        sb.Append($"  - {vacationInfo.Date:d}");

                //        if (vacationInfo.HourCount != null)
                //            sb.Append($" ({vacationInfo.HourCount}h)");

                //        if (vacationInfo.Comments != null)
                //            sb.Append($" - {vacationInfo.Comments}");

                //        Console.WriteLine(sb);
                //    }
                //}


                int currentYear = -1;
                int currentMonth = -1;
                foreach (VacationInfo vacationInfo in teamMemberVacation.Vacations)
                {
                    if (currentYear != vacationInfo.Date.Year || currentMonth != vacationInfo.Date.Month)
                    {
                        currentYear = vacationInfo.Date.Year;
                        currentMonth = vacationInfo.Date.Month;

                        Console.WriteLine($"{currentYear}.{currentMonth}");
                    }

                    StringBuilder sb = new();
                    sb.Append($"  - {vacationInfo.Date:d}");

                    if (vacationInfo.HourCount != null)
                        sb.Append($" ({vacationInfo.HourCount}h)");

                    if (vacationInfo.Comments != null)
                        sb.Append($" - {vacationInfo.Comments}");

                    Console.WriteLine(sb);
                }
            }
        }
    }
}