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

namespace DustInTheWind.VeloCity.Domain
{
    public class SprintMemberDay
    {
        public DateTime Date { get; set; }

        public int WorkHours { get; set; }
        
        public int AbsenceHours { get; set; }
        
        public AbsenceReason AbsenceReason { get; set; }
        
        public string AbsenceComments { get; set; }

        public int OvertimeHours { get; set; }
        
        public string OvertimeComments { get; set; }

        //public SprintMemberDay(SprintDay sprintDay, TeamMember teamMember)
        //{
        //    if (sprintDay == null) throw new ArgumentNullException(nameof(sprintDay));
        //    if (teamMember == null) throw new ArgumentNullException(nameof(teamMember));

        //    Date = sprintDay.Date;
        
        //    int workHours = 0;

        //    Employment employment = teamMember.GetEmploymentFor(sprintDay.Date);

        //    bool isEmployed = employment != null;
        //    if (isEmployed && sprintDay.IsWorkDay)
        //    {
        //        workHours = employment.HoursPerDay;

        //        VacationDay vacationDay = teamMember.GetVacationFor(sprintDay.Date);

        //        bool hasVacation = vacationDay != null;
        //        if (hasVacation)
        //        {
        //            bool isFullDayVacation = vacationDay.HourCount == null;

        //            workHours -= isFullDayVacation
        //                ? employment.HoursPerDay
        //                : vacationDay.HourCount.Value;
        //        }

        //        OvertimeDay overtimeDay = teamMember.GetOvertimeFor(sprintDay.Date);

        //        bool hasOvertime = overtimeDay != null;
        //        if (hasOvertime)
        //            workHours += overtimeDay.HourCount;
        //    }

        //    return workHours;
        //}
    }
}