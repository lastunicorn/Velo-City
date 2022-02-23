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
using DustInTheWind.ConsoleTools;
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    public class AnalyzeSprintView
    {
        public void Display(AnalyzeSprintResponse response)
        {
            Console.WriteLine($"{response.SprintName} ({response.StartDate:d} - {response.EndDate:d})");
            Console.WriteLine(new string('=', 79));

            WorkDaysControl workDaysControl = new()
            {
                Days = response.WorkDays
            };
            workDaysControl.Display();

            CustomConsole.WriteEmphasized("Total Work Hours: ");
            CustomConsole.WriteLine($"{response.TotalWorkHours}h");

            CustomConsole.WriteLine();

            CustomConsole.WriteEmphasized("Estimated Story Points: ");
            CustomConsole.WriteLine($"{response.EstimatedStoryPoints} SP");

            CustomConsole.WriteEmphasized("Estimated Velocity: ");
            CustomConsole.WriteLine($"{response.EstimatedVelocity} SP/h");

            CustomConsole.WriteEmphasized("Commitment Story Points: ");
            CustomConsole.WriteLine($"{response.CommitmentStoryPoints} SP");

            CustomConsole.WriteLine();

            CustomConsole.WriteEmphasized("Actual Story Points: ");
            CustomConsole.WriteLine($"{response.ActualStoryPoints} SP");

            CustomConsole.WriteEmphasized("Actual Velocity: ");
            CustomConsole.WriteLine($"{response.ActualVelocity} SP/h");

            Console.WriteLine();
            Console.WriteLine(new string('-', 79));

            foreach (SprintMember sprintMember in response.SprintMembers)
            {
                DisplaySprintMemberDetails(sprintMember);
            }
        }

        private static void DisplaySprintMemberDetails(SprintMember sprintMember)
        {
            Console.WriteLine();

            int totalWorkHours = sprintMember.Days.Sum(x => x.WorkHours);
            CustomConsole.WriteLineEmphasized($"{sprintMember.Name} - {totalWorkHours}h");

            foreach (SprintMemberDay sprintMemberDay in sprintMember.Days)
            {
                DisplaySprintMemberDayDetails(sprintMemberDay);
            }
        }

        private static void DisplaySprintMemberDayDetails(SprintMemberDay sprintMemberDay)
        {
            StringBuilder sb = new();
            string workHours = sprintMemberDay.WorkHours == 0
                ? "-"
                : sprintMemberDay.WorkHours.ToString();
            string vacationHours = sprintMemberDay.VacationHours == 0
                ? "-"
                : sprintMemberDay.VacationHours.ToString();
            sb.Append($"  - {sprintMemberDay.Date:d}: {workHours}h / {vacationHours}h ({sprintMemberDay.Date:dddd})");

            if (sprintMemberDay.VacationHours > 0)
            {
                sb.Append($" - {sprintMemberDay.VacationReason}");

                if (sprintMemberDay.VacationComments != null)
                    sb.Append($" - {sprintMemberDay.VacationComments}");
            }

            switch (sprintMemberDay.VacationReason)
            {
                case VacationReason.None:
                    CustomConsole.WriteLineSuccess(sb.ToString());
                    break;

                case VacationReason.WeekEnd:
                case VacationReason.OfficialHoliday:
                    break;

                case VacationReason.Vacation:
                    CustomConsole.WriteLineWarning(sb.ToString());
                    break;

                default:
                    CustomConsole.WriteLine(sb.ToString());
                    break;
            }
        }
    }
}