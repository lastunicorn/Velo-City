// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using System.Collections.ObjectModel;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;

public class AbsenceGroupCollection : Collection<AbsenceGroup>
{
    public AbsenceGroupCollection(IEnumerable<AbsenceGroup> items)
    {
        if (items != null)
        {
            IEnumerable<AbsenceGroup> itemsNotNull = items.Where(x => x != null);

            foreach (AbsenceGroup absenceGroup in itemsNotNull)
                Items.Add(absenceGroup);
        }
    }

    public void Add(TeamMemberAbsence teamMemberAbsence, string teamMemberCountry)
    {
        IEnumerable<AbsenceGroup> absenceGroupsForTeamMember = Items
            .Where(x => x.OfficialHoliday?.HolidayCountry == teamMemberCountry);

        bool isAdded = false;

        foreach (AbsenceGroup absenceGroup in absenceGroupsForTeamMember)
        {
            absenceGroup.Add(teamMemberAbsence);
            isAdded = true;
        }

        if (!isAdded)
        {
            AbsenceGroup defaultAbsenceGroup = GetOrCreateDefaultAbsenceGroup();
            defaultAbsenceGroup.Add(teamMemberAbsence);
        }
    }

    private AbsenceGroup GetOrCreateDefaultAbsenceGroup()
    {
        AbsenceGroup absenceGroup = Items.FirstOrDefault(x => x.OfficialHoliday == null);

        if (absenceGroup == null)
        {
            absenceGroup = new AbsenceGroup();
            Items.Add(absenceGroup);
        }

        return absenceGroup;
    }
}