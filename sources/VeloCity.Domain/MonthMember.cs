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

namespace DustInTheWind.VeloCity.Domain
{
    public class MonthMember
    {
        private readonly MonthCalendar monthCalendar;

        public PersonName Name => TeamMember.Name;

        public TeamMember TeamMember { get; }

        public SprintMemberDayCollection Days { get; }

        public bool IsEmployed => Days
            .Any(x => x.AbsenceReason != AbsenceReason.Unemployed);

        public HoursValue WorkHours => Days
            .Select(x => x.WorkHours)
            .Sum(x => x.Value);

        public MonthMember(TeamMember teamMember, MonthCalendar monthCalendar)
        {
            this.monthCalendar = monthCalendar ?? throw new ArgumentNullException(nameof(monthCalendar));
            TeamMember = teamMember ?? throw new ArgumentNullException(nameof(teamMember));

            IEnumerable<SprintMemberDay> sprintMemberDays = monthCalendar.EnumerateAllDays()
                .Select(x => new SprintMemberDay(TeamMember, x));

            Days = new SprintMemberDayCollection(sprintMemberDays);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}