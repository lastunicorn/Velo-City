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

using System.Text;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Vacations;

public abstract class VacationViewModel
{
    public int? HourCount { get; set; }

    public string Comments { get; set; }

    public abstract DateTime? SignificantDate { get; }

    public abstract DateTime? StartDate { get; }

    public abstract DateTime? EndDate { get; }

    protected VacationViewModel(Vacation vacationOnce)
    {
        HourCount = vacationOnce.HourCount;
        Comments = vacationOnce.Comments;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        string dateAsString = RenderDate();
        sb.Append($"{dateAsString}");

        if (HourCount != null)
            sb.Append($" ({HourCount}h)");

        if (Comments != null)
            sb.Append($" - {Comments}");

        return sb.ToString();
    }

    protected abstract string RenderDate();

    public static VacationViewModel From(Vacation vacation)
    {
        return vacation switch
        {
            VacationOnce vacationOnce => new VacationOnceViewModel(vacationOnce),
            VacationDaily vacationDaily => new VacationDailyViewModel(vacationDaily),
            VacationWeekly vacationWeekly => new VacationWeeklyViewModel(vacationWeekly),
            VacationMonthly vacationMonthly => new VacationMonthlyViewModel(vacationMonthly),
            VacationYearly vacationYearly => new VacationYearlyViewModel(vacationYearly),
            _ => throw new ArgumentOutOfRangeException(nameof(vacation))
        };
    }
}