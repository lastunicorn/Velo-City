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

        public List<VacationDay> VacationDays { get; set; }

        public List<Employment> Employments { get; set; }

        public int CalculateWorkHoursFor(Sprint sprint)
        {
            return sprint.EnumerateWorkDays()
                .Select(CalculateWorkHoursFor)
                .Sum();
        }

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
                    AbsenceHours = CalculateVacationHoursFor(x),
                    AbsenceReason = CalculateVacationReason(x),
                    AbsenceComments = GetVacationComments(x)
                });
        }

        public int CalculateWorkHoursFor(SprintDay sprintDay)
        {
            Employment employment = GetEmploymentFor(sprintDay.Date);

            bool isEmployed = employment != null;
            if (!isEmployed)
                return 0;

            if (sprintDay.IsFreeDay)
                return 0;

            VacationDay vacationDay = VacationDays.FirstOrDefault(x => x.Date == sprintDay.Date);

            if (vacationDay == null)
                return employment.HoursPerDay;

            return vacationDay.HourCount == null
                ? 0
                : employment.HoursPerDay - vacationDay.HourCount.Value;
        }

        private int CalculateVacationHoursFor(SprintDay sprintDay)
        {
            Employment employment = GetEmploymentFor(sprintDay.Date);

            bool isEmployed = employment != null;
            if (!isEmployed)
                return 0;

            if (sprintDay.IsFreeDay)
                return employment.HoursPerDay;

            VacationDay vacationDay = VacationDays.FirstOrDefault(x => x.Date == sprintDay.Date);

            if (vacationDay == null)
                return 0;

            return vacationDay.HourCount ?? employment.HoursPerDay;
        }

        private bool IsEmployed(DateTime date)
        {
            Employment employment = GetEmploymentFor(date);
            return employment != null;
        }

        private Employment GetEmploymentFor(DateTime date)
        {
            return Employments?
                .FirstOrDefault(x => (x.StartDate == null || date >= x.StartDate) && (x.EndDate == null || date <= x.EndDate));

        }

        private AbsenceReason CalculateVacationReason(SprintDay sprintDay)
        {
            bool isEmployed = IsEmployed(sprintDay.Date);
            if (!isEmployed)
                return AbsenceReason.Unemployed;

            if (sprintDay.IsWeekEnd)
                return AbsenceReason.WeekEnd;

            if (sprintDay.IsOfficialHoliday)
                return AbsenceReason.OfficialHoliday;

            VacationDay vacationDay = VacationDays.FirstOrDefault(x => x.Date == sprintDay.Date);

            return vacationDay != null
                ? AbsenceReason.Vacation
                : AbsenceReason.None;
        }

        private string GetVacationComments(SprintDay sprintDay)
        {
            VacationDay vacationDay = VacationDays.FirstOrDefault(x => x.Date == sprintDay.Date);

            return vacationDay?.Comments;
        }
    }
}