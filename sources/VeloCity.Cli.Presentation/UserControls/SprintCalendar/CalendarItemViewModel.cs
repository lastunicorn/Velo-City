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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Cli.Presentation.UserControls.SprintCalendar
{
    public class CalendarItemViewModel
    {
        public DateTime Date { get; }

        public bool IsWorkDay { get; set; }

        public HoursValue WorkHours { get; }

        public HoursValue AbsenceHours { get; }

        public AbsenceDetailsViewModel AbsenceDetails { get; }

        public bool IsHighlighted { get; set; }

        public CalendarItemViewModel(SprintDay sprintDay, List<SprintMemberDay> sprintMemberDays)
        {
            if (sprintMemberDays == null) throw new ArgumentNullException(nameof(sprintMemberDays));
            if (sprintDay == null) throw new ArgumentNullException(nameof(sprintDay));

            Date = sprintDay.Date;
            IsWorkDay = sprintMemberDays.Any(x => x.AbsenceReason is AbsenceReason.None or AbsenceReason.Vacation or AbsenceReason.OfficialHoliday);
            WorkHours = sprintMemberDays.Sum(x => x.WorkHours);
            AbsenceHours = sprintMemberDays
                .Where(x => x.AbsenceReason != AbsenceReason.WeekEnd)
                .Sum(x => x.AbsenceHours);
            AbsenceDetails = new AbsenceDetailsViewModel(sprintMemberDays, sprintDay);
        }
    }
}