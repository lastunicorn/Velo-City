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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberVacations;

public abstract class VacationInfo
{
    public int? HourCount { get; set; }

    public string Comments { get; set; }

    protected VacationInfo(Vacation vacation)
    {
        HourCount = vacation.HourCount;
        Comments = vacation.Comments;
    }

    public static VacationInfo From(Vacation vacation)
    {
        return vacation switch
        {
            SingleDayVacation vacationOnce => new VacationOnceInfo(vacationOnce),
            DailyVacation vacationDaily => new VacationDailyInfo(vacationDaily),
            WeeklyVacation vacationWeekly => new VacationWeeklyInfo(vacationWeekly),
            MonthlyVacation vacationMonthly => new VacationMonthlyInfo(vacationMonthly),
            YearlyVacation vacationYearly => new VacationYearlyInfo(vacationYearly),
            _ => throw new ArgumentOutOfRangeException(nameof(vacation))
        };
    }
}