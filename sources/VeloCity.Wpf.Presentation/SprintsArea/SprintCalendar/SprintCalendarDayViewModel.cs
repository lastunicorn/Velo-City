﻿// VeloCity
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
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintCalendar;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintCalendar;

public class SprintCalendarDayViewModel : DataGridRowViewModel
{
    private ChartBarValue<SprintCalendarDayViewModel> chartBarValue;

    public override bool IsSelectable => true;

    public override bool IsEnabled => IsWorkDay;

    public DateTime Date { get; }

    public bool IsCurrentDay { get; }

    public bool IsWorkDay { get; }

    public HoursValue? WorkHours { get; }

    public bool HasWorkHours => WorkHours?.Value > 0;

    public ChartBarValue<SprintCalendarDayViewModel> ChartBarValue
    {
        get => IsWorkDay ? chartBarValue : null;
        set => chartBarValue = value;
    }

    public HoursValue? AbsenceHours { get; }

    public bool HasAbsenceHours => AbsenceHours?.Value > 0;

    public List<AbsenceDetailsViewModel> Absences { get; }

    public SprintCalendarDayViewModel(SprintCalendarDay sprintCalendarDay)
    {
        Date = sprintCalendarDay.Date;
        IsCurrentDay = sprintCalendarDay.IsCurrentDay;
        IsWorkDay = sprintCalendarDay.IsWorkDay;
        WorkHours = sprintCalendarDay.WorkHours;
        AbsenceHours = sprintCalendarDay.AbsenceHours;
        Absences = sprintCalendarDay.AbsenceGroups
            .OrderByDescending(x => x.OfficialHoliday?.HolidayCountry)
            .Select(x => new AbsenceDetailsViewModel
            {
                OfficialHolidayAbsences = x.OfficialHoliday != null
                    ? new ObservableCollection<OfficialHolidayViewModel>
                    {
                        new(x.OfficialHoliday)
                    }
                    : null,
                Text = sprintCalendarDay.IsWorkDay
                    ? null
                    : x.OfficialHoliday?.HolidayName,
                TeamMemberAbsences = x
                    .Select(z => new TeamMemberAbsenceViewModel(z))
                    .ToObservableCollection()
            })
            .ToList();
    }
}