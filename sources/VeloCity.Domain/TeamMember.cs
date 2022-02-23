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

using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.VeloCity.Domain
{
    public class TeamMember
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int HoursPerDay { get; set; }

        public List<VacationDay> VacationDays { get; set; }

        public SprintMember ToSprintMember(Sprint sprint)
        {
            return new SprintMember
            {
                Name = Name,
                Days = CalculateDays(sprint).ToList()
            };
        }

        private IEnumerable<SprintMemberDay> CalculateDays(Sprint sprint)
        {
            return sprint.EnumerateAllDays()
                .Select(x => new SprintMemberDay
                {
                    Date = x.Date,
                    WorkHours = CalculateWorkHoursFor(x),
                    VacationHours = CalculateVacationHoursFor(x),
                    VacationReason = CalculateVacationReason(x),
                    VacationComments = GetVacationComments(x)
                });
        }

        private string GetVacationComments(SprintDay sprintDay)
        {
            VacationDay vacationDay = VacationDays.FirstOrDefault(x => x.Date == sprintDay.Date);

            return vacationDay?.Comments;
        }

        private VacationReason CalculateVacationReason(SprintDay sprintDay)
        {
            if (sprintDay.IsWeekEnd)
                return VacationReason.WeekEnd;

            if (sprintDay.IsOfficialHoliday)
                return VacationReason.OfficialHoliday;

            VacationDay vacationDay = VacationDays.FirstOrDefault(x => x.Date == sprintDay.Date);

            return vacationDay != null
                ? VacationReason.Vacation
                : VacationReason.None;
        }

        private int CalculateVacationHoursFor(SprintDay sprintDay)
        {
            int hoursPerDay = HoursPerDay == 0
                ? 8
                : HoursPerDay;

            if (sprintDay.IsFreeDay)
                return hoursPerDay;

            VacationDay vacationDay = VacationDays.FirstOrDefault(x => x.Date == sprintDay.Date);

            if (vacationDay == null)
                return 0;

            return vacationDay.HourCount ?? hoursPerDay;
        }

        public int CalculateWorkHoursFor(Sprint sprint)
        {
            return sprint.GetWorkDays()
                .Select(CalculateWorkHoursFor)
                .Sum();
        }

        public int CalculateWorkHoursFor(SprintDay sprintDay)
        {
            if (sprintDay.IsFreeDay)
                return 0;

            VacationDay vacationDay = VacationDays.FirstOrDefault(x => x.Date == sprintDay.Date);

            int workHours = HoursPerDay == 0
                ? 8
                : HoursPerDay;

            if (vacationDay != null)
            {
                workHours = vacationDay.HourCount == null
                    ? 0
                    : workHours - vacationDay.HourCount.Value;
            }

            return workHours;
        }
    }
}