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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.Sprint.TeamOverview
{
    public class TeamMemberViewModel
    {
        private readonly SprintMember sprintMember;

        public PersonName Name => sprintMember.Name;

        public HoursValue WorkHours => sprintMember.WorkHours;

        public HoursValue AbsenceHours => sprintMember.Days
            .Where(x =>
            {
                bool isWeekDay = x.Date.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday);
                if (isWeekDay)
                    return true;

                bool hasWorkHoursInWeekEnd = x.WorkHours > 0;
                if (hasWorkHoursInWeekEnd)
                    return true;

                bool isOfficialHoliday = x.AbsenceReason == AbsenceReason.OfficialHoliday;
                return isOfficialHoliday;
            })
            .Where(x => x.AbsenceReason != AbsenceReason.OfficialHoliday)
            .Sum(x => x.AbsenceHours);

        public TeamMemberViewModel(SprintMember sprintMember)
        {
            this.sprintMember = sprintMember ?? throw new ArgumentNullException(nameof(sprintMember));
        }
    }
}