﻿// Velo City
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

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint.SprintCalendar
{
    public class CalendarItemViewModel
    {
        private readonly List<SprintMemberDay> sprintMemberDays;
        private readonly SprintDay sprintDay;
        private readonly int workHours;
        private readonly int absenceHours;

        public DateTime Date => sprintDay.Date;

        public HoursValue WorkHours => workHours;

        public ChartItem ChartItem => new()
        {
            MaxValue = workHours + absenceHours,
            Value = workHours
        };

        public HoursValue AbsenceHours => absenceHours;

        public VacationDetails VacationDetails
        {
            get
            {
                if (sprintDay.IsWeekEnd)
                {
                    return new VacationDetails
                    {
                        AllTeamAbsenceReason = AbsenceReason.WeekEnd
                    };
                }
                else if (sprintDay.IsOfficialHoliday)
                {
                    return new VacationDetails
                    {
                        AllTeamAbsenceReason = AbsenceReason.OfficialHoliday
                    };
                }
                else
                {
                    return new VacationDetails
                    {
                        TeamMembers = sprintMemberDays
                            .Where(x => x.AbsenceHours > 0)
                            .Select(x => new TeamMemberVacationDetails
                            {
                                Name = x.TeamMember.Name,
                                IsPartialVacation = x.WorkHours > 0
                            })
                            .ToList()
                    };
                }
            }
        }

        public CalendarItemViewModel(List<SprintMemberDay> sprintMemberDays, SprintDay sprintDay)
        {
            this.sprintMemberDays = sprintMemberDays ?? throw new ArgumentNullException(nameof(sprintMemberDays));
            this.sprintDay = sprintDay;

            workHours = sprintMemberDays.Sum(x => x.WorkHours);
            absenceHours = sprintDay.IsWeekEnd || sprintDay.IsOfficialHoliday
                ? 0
                : sprintMemberDays.Sum(x => x.AbsenceHours);
        }
    }
}