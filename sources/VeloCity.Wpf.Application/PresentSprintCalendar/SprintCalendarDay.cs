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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;

public class SprintCalendarDay
{
    public DateTime Date { get; }

    public bool IsCurrentDay { get; private set; }

    public bool IsWorkDay { get; private set; }

    public HoursValue? WorkHours { get; private set; }

    public HoursValue? AbsenceHours { get; private set; }

    public AbsenceGroupCollection AbsenceGroups { get; }

    public SprintCalendarDay(SprintDay sprintDay)
    {
        if (sprintDay == null) throw new ArgumentNullException(nameof(sprintDay));

        Date = sprintDay.Date;

        AbsenceGroups = sprintDay.OfficialHolidays
            .Select(x => new AbsenceGroup
            {
                OfficialHoliday = new OfficialHolidayDto(x)
            })
            .ToAbsenceGroupCollection();
    }

    internal void AddSprintMemberDay(SprintMemberDay sprintMemberDay)
    {
        IsWorkDay = IsWorkDay || sprintMemberDay.IsWorkDay;

        WorkHours += sprintMemberDay.WorkHours;

        if (sprintMemberDay.AbsenceReason != AbsenceReason.WeekEnd)
            AbsenceHours += sprintMemberDay.AbsenceHours;

        if (sprintMemberDay.AbsenceHours > 0 || sprintMemberDay.AbsenceReason == AbsenceReason.Contract)
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

    public void SetAsCurrentDay()
    {
        IsCurrentDay = true;
    }
}