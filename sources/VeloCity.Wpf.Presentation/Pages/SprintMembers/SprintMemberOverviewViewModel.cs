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
using System.Threading.Tasks;
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintMemberCalendar;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintMembers
{
    public class SprintMemberOverviewViewModel : DataGridRowViewModel
    {
        private readonly SprintMember sprintMember;

        public override bool IsSelectable => true;

        public PersonName Name { get; }

        public HoursValue WorkHours { get; }

        public bool HasWorkHours => WorkHours.Value > 0;

        public HoursValue AbsenceHours { get; }

        public bool HasAbsenceHours => AbsenceHours.Value > 0;

        public ChartBar ChartBar { get; set; }

        public SprintMemberCalendarViewModel SprintMemberCalendarViewModel { get; }

        public SprintMemberOverviewViewModel(SprintMember sprintMember)
        {
            this.sprintMember = sprintMember ?? throw new ArgumentNullException(nameof(sprintMember));

            Name = sprintMember.Name;
            WorkHours = sprintMember.WorkHours;
            AbsenceHours = sprintMember.Days
                .Where(IsAbsenceDay)
                .Sum(x => x.AbsenceHours);

            SprintMemberCalendarViewModel = new SprintMemberCalendarViewModel(sprintMember);
        }

        private static bool IsAbsenceDay(SprintMemberDay sprintMemberDay)
        {
            bool isWeekEnd = sprintMemberDay.SprintDay.Date.DayOfWeek is (DayOfWeek.Saturday or DayOfWeek.Sunday);
            if (!isWeekEnd)
                return true;

            bool hasWorkHoursInWeekEnd = sprintMemberDay.WorkHours > 0;
            if (hasWorkHoursInWeekEnd)
                return true;

            bool isOfficialHolidayInWeekEnd = sprintMemberDay.AbsenceReason == AbsenceReason.OfficialHoliday;
            return isOfficialHolidayInWeekEnd;
        }
    }
}