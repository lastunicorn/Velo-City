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

namespace DustInTheWind.VeloCity.Domain
{
    public class TeamMember
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int HoursPerDay { get; set; }

        public List<Vacation> Vacations { get; set; }

        public SprintMember ToSprintMember(Sprint sprint)
        {
            return new SprintMember
            {
                Name = Name,
                DayInfo = CalculateDays(sprint).ToList()
            };
        }

        private IEnumerable<SprintMemberDay> CalculateDays(Sprint sprint)
        {
            return sprint.CalculateWorkDays()
                .Select(x => new SprintMemberDay
                {
                    Date = x,
                    WorkHours = CalculateHoursFor(x)
                })
                .Where(x => x.WorkHours > 0);
        }

        public int CalculateHoursFor(Sprint sprint)
        {
            return sprint.CalculateWorkDays()
                .Select(CalculateHoursFor)
                .Sum();
        }

        public int CalculateHoursFor(DateTime dateTime)
        {
            int hours = HoursPerDay == 0
                ? 8
                : HoursPerDay;

            Vacation vacation = Vacations.FirstOrDefault(x => x.Date == dateTime);

            if (vacation != null)
            {
                hours = vacation.HourCount == null
                    ? 0
                    : hours - vacation.HourCount.Value;
            }

            return hours;
        }
    }
}