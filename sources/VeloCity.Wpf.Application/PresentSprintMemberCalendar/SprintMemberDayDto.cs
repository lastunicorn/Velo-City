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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintMemberCalendar;

public class SprintMemberDayDto
{
    public int TeamMemberId { get; }

    public DateTime Date { get; }

    public bool IsCurrentDay { get; }

    public bool IsWorkDay { get; }

    public HoursValue? WorkHours { get; }

    public HoursValue? AbsenceHours { get; }

    public AbsenceReason AbsenceReason { get; }

    public string AbsenceComments { get; }

    public bool CanAddVacation { get; }

    public bool CanRemoveVacation { get; }

    public SprintMemberDayDto(SprintMemberDay sprintMemberDay, DateTime currentDate)
    {
        TeamMemberId = sprintMemberDay.TeamMember?.Id ?? -1;
        Date = sprintMemberDay.SprintDay.Date;
        IsCurrentDay = sprintMemberDay.SprintDay.Date == currentDate;
        IsWorkDay = sprintMemberDay.IsWorkDay;

        if (IsWorkDay)
        {
            WorkHours = sprintMemberDay.WorkHours;
            AbsenceHours = sprintMemberDay.AbsenceHours;
        }

        AbsenceReason = sprintMemberDay.AbsenceReason;
        AbsenceComments = sprintMemberDay.AbsenceComments;

        CanAddVacation = IsWorkDay && WorkHours > 0;
        CanRemoveVacation = IsWorkDay && AbsenceHours > 0 && sprintMemberDay.AbsenceReason == AbsenceReason.Vacation;
    }
}