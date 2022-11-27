// VeloCity
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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar
{
    public class SprintCalendarDay
    {
        public DateTime Date { get; }

        public bool IsCurrentDay { get; set; }

        public bool IsWorkDay { get; }

        public HoursValue? WorkHours { get; }

        public HoursValue? AbsenceHours { get; }
        
        public AbsenceGroupCollection AbsenceGroups { get; }

        public SprintCalendarDay(SprintDay sprintDay, List<SprintMemberDay> sprintMemberDays, DateTime currentDate)
        {
            if (sprintMemberDays == null) throw new ArgumentNullException(nameof(sprintMemberDays));
            if (sprintDay == null) throw new ArgumentNullException(nameof(sprintDay));

            Date = sprintDay.Date;
            IsCurrentDay = sprintDay.Date == currentDate;

            IsWorkDay = sprintMemberDays.Any(x => x.IsWorkDay);

            WorkHours = IsWorkDay
                ? sprintMemberDays.Sum(x => x.WorkHours)
                : null;

            AbsenceHours = IsWorkDay
                ? sprintMemberDays
                    .Where(x => x.AbsenceReason != AbsenceReason.WeekEnd)
                    .Sum(x => x.AbsenceHours)
                : null;
            
            AbsenceGroups = sprintDay.OfficialHolidays
                .Select(x => new AbsenceGroup
                {
                    OfficialHoliday = new OfficialHolidayDto(x)
                })
                .ToAbsenceGroupCollection();

            IEnumerable<SprintMemberDay> sprintMemberDaysWithAbsences = sprintMemberDays
                .Where(x => x.AbsenceHours > 0 || x.AbsenceReason == AbsenceReason.Contract);

            foreach (SprintMemberDay sprintMemberDay in sprintMemberDaysWithAbsences)
            {
                TeamMemberAbsence teamMemberAbsence = new()
                {
                    Name = sprintMemberDay.TeamMember.Name.ShortName,
                    IsPartialVacation = sprintMemberDay.WorkHours > 0,
                    IsMissingByContract = sprintMemberDay.AbsenceReason == AbsenceReason.Contract,
                    AbsenceHours = sprintMemberDay.AbsenceHours
                };

                string teamMemberCountry = sprintMemberDay.GetCountry();
                AbsenceGroups.Add(teamMemberAbsence, teamMemberCountry);
            }
        }
    }
}