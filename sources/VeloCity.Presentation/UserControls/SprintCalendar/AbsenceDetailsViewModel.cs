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

namespace DustInTheWind.VeloCity.Presentation.UserControls.SprintCalendar
{
    public class AbsenceDetailsViewModel
    {
        public List<TeamMemberAbsenceDetails> TeamMemberVacationDetails { get; }

        public List<OfficialHolidayAbsenceDetails> OfficialHolidays { get; }

        public AbsenceDetailsViewModel(List<SprintMemberDay> sprintMemberDays, SprintDay sprintDay)
        {
            if (sprintMemberDays == null) throw new ArgumentNullException(nameof(sprintMemberDays));
            if (sprintDay == null) throw new ArgumentNullException(nameof(sprintDay));

            if (!sprintDay.IsWeekEnd)
            {
                TeamMemberVacationDetails = sprintMemberDays
                    .Where(x => x.AbsenceHours > 0 || x.AbsenceReason == AbsenceReason.Contract)
                    .Select(x => new TeamMemberAbsenceDetails(x))
                    .ToList();

                string[] countries = sprintMemberDays
                    .Select(x =>
                    {
                        Employment employment = x.TeamMember.Employments?.GetEmploymentFor(x.SprintDay.Date);
                        return employment?.Country;
                    })
                    .Where(x => x != null)
                    .Distinct()
                    .ToArray();

                OfficialHolidays = sprintDay.OfficialHolidays
                    .Where(x => countries.Contains(x.Country))
                    .Select(x => new OfficialHolidayAbsenceDetails(x))
                    .ToList();
            }
        }

        public override string ToString()
        {
            IEnumerable<string> teamMembersAbsenceInfos = TeamMemberVacationDetails == null
                ? Enumerable.Empty<string>()
                : TeamMemberVacationDetails
                    .Select(x => x.ToString());

            IEnumerable<string> officialHolidaysAbsenceInfos = OfficialHolidays == null
                ? Enumerable.Empty<string>()
                : OfficialHolidays
                    .Select(x => x.ToString());

            IEnumerable<string> items = teamMembersAbsenceInfos
                .Concat(officialHolidaysAbsenceInfos);

            return string.Join(", ", items);
        }
    }
}