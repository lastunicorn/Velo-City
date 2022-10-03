﻿// VeloCity
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
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintCalendar
{
    public class SprintCalendarItemViewModel : DataGridRowViewModel
    {
        private ChartBarValue<SprintCalendarItemViewModel> chartBarValue;

        public override bool IsSelectable => true;

        public override bool IsEnabled => IsWorkDay;

        public DateTime Date { get; }

        public bool IsWorkDay { get; }

        public HoursValue? WorkHours { get; }

        public bool HasWorkHours => WorkHours?.Value > 0;

        public ChartBarValue<SprintCalendarItemViewModel> ChartBarValue
        {
            get => IsWorkDay ? chartBarValue : null;
            set => chartBarValue = value;
        }

        public HoursValue? AbsenceHours { get; }

        public bool HasAbsenceHours => AbsenceHours?.Value > 0;

        public AbsenceDetailsViewModel AbsenceDetails { get; }

        //public List<OfficialHolidayAbsenceDetails> OfficialHolidays { get; }

        public SprintCalendarItemViewModel(SprintDay sprintDay, List<SprintMemberDay> sprintMemberDays)
        {
            if (sprintMemberDays == null) throw new ArgumentNullException(nameof(sprintMemberDays));
            if (sprintDay == null) throw new ArgumentNullException(nameof(sprintDay));

            Date = sprintDay.Date;
            IsWorkDay = sprintMemberDays.Any(x => x.AbsenceReason is AbsenceReason.None or AbsenceReason.Vacation or AbsenceReason.OfficialHoliday);
            WorkHours = IsWorkDay
                ? sprintMemberDays.Sum(x => x.WorkHours)
                : null;
            AbsenceHours = IsWorkDay
                ? sprintMemberDays
                    .Where(x => x.AbsenceReason != AbsenceReason.WeekEnd)
                    .Sum(x => x.AbsenceHours)
                : null;
            AbsenceDetails = new AbsenceDetailsViewModel(sprintMemberDays, sprintDay);

            //if (!sprintDay.IsWeekEnd)
            //{
            //    //TeamMemberVacationDetails = sprintMemberDays
            //    //    .Where(x => x.AbsenceHours > 0 || x.AbsenceReason == AbsenceReason.Contract)
            //    //    .Select(x => new TeamMemberAbsenceDetails(x))
            //    //    .ToList();

            //    string[] countries = sprintMemberDays
            //        .Select(x =>
            //        {
            //            Employment employment = x.TeamMember.Employments?.GetEmploymentFor(x.SprintDay.Date);
            //            return employment?.Country;
            //        })
            //        .Where(x => x != null)
            //        .Distinct()
            //        .ToArray();

            //    OfficialHolidays = sprintDay.OfficialHolidays
            //        .Where(x => countries.Contains(x.Country))
            //        .Select(x => new OfficialHolidayAbsenceDetails(x))
            //        .ToList();
            //}
        }
    }
}