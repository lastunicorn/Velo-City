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
using DustInTheWind.VeloCity.Application.SprintVelocity;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.SprintVelocity
{
    internal class SprintVelocityView
    {
        public void DisplaySprintVelocity(SprintVelocityResponse response)
        {
            Console.WriteLine($"{response.SprintName} ({response.StartDate:d} - {response.EndDate:d})");
            Console.WriteLine(new string('=', 79));

            Console.WriteLine($"Work Days: {response.WorkDays.Count} days");

            for (int i = 0; i < response.WorkDays.Count; i++)
            {
                DateTime dateTime = response.WorkDays[i];
                int dayIndex = i + 1;
                Console.WriteLine($"  {dayIndex:D2} - {dateTime:d} ({dateTime:dddd})");
            }

            Console.WriteLine($"Story Points: {response.TotalStoryPoints} SP");
            Console.WriteLine($"Actual Work Hours: {response.TotalWorkHours} h");
            Console.WriteLine($"Velocity: {response.Velocity} SP/h");

            foreach (SprintMember sprintMember in response.SprintMembers)
            {
                Console.WriteLine();
                Console.WriteLine($"{sprintMember.Name} - {sprintMember.DayInfo.Sum(x => x.WorkHours)} h");

                foreach (SprintMemberDay sprintMemberDay in sprintMember.DayInfo)
                    Console.WriteLine($"  - {sprintMemberDay.Date:d}: {sprintMemberDay.WorkHours} h ({sprintMemberDay.Date:dddd})");
            }
        }
    }
}