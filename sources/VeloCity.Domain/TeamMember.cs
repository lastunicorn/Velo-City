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

        public List<Employment> Employments { get; set; }

        public List<VacationDay> VacationDays { get; set; }

        public List<OvertimeDay> OvertimeDays { get; set; }

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
                Days = sprint.EnumerateAllDays()
                    .Select(ToSprintMemberDay)
                    .ToList()
            };
        }

        private SprintMemberDay ToSprintMemberDay(SprintDay sprintDay)
        {
            return new SprintMemberDay
            {
                Date = sprintDay.Date,
                WorkHours = CalculateWorkHoursFor(sprintDay),
                AbsenceHours = CalculateVacationHoursFor(sprintDay),
                AbsenceReason = CalculateVacationReason(sprintDay),
                AbsenceComments = GetVacationComments(sprintDay),
                OvertimeHours = CalculateOvertimeHours(),
                OvertimeComments = GetOvertimeComments()
            };
        }

        public int CalculateWorkHoursFor(SprintDay sprintDay)
        {
            int workHours = 0;

            Employment employment = GetEmploymentFor(sprintDay.Date);

            bool isEmployed = employment != null;
            if (isEmployed && sprintDay.IsWorkDay)
            {
                workHours = employment.HoursPerDay;

                VacationDay vacationDay = GetVacationFor(sprintDay.Date);

                bool hasVacation = vacationDay != null;
                if (hasVacation)
                {
                    bool isFullDayVacation = vacationDay.HourCount == null;

                    workHours -= isFullDayVacation
                        ? employment.HoursPerDay
                        : vacationDay.HourCount.Value;
                }

                OvertimeDay overtimeDay = GetOvertimeFor(sprintDay.Date);

                bool hasOvertime = overtimeDay != null;
                if (hasOvertime)
                    workHours += overtimeDay.HourCount;
            }

            return workHours;
        }

        public VacationDay GetVacationFor(DateTime date)
        {
            return VacationDays?.FirstOrDefault(x => x.Date == date);
        }

        public OvertimeDay GetOvertimeFor(DateTime date)
        {
            return OvertimeDays?.FirstOrDefault(x => x.Date == date);
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

        public Employment GetEmploymentFor(DateTime date)
        {
            return Employments?
                .FirstOrDefault(x => (x.StartDate == null || date >= x.StartDate) && (x.EndDate == null || date <= x.EndDate));
        }

        private AbsenceReason CalculateVacationReason(SprintDay sprintDay)
        {
            Employment employment = GetEmploymentFor(sprintDay.Date);

            bool isEmployed = employment != null;
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