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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Vacations;

public class VacationDailyViewModel : VacationViewModel
{
    public DateInterval DateInterval { get; set; }

    public override DateTime? SignificantDate => DateInterval.StartDate;

    public override DateTime? StartDate => DateInterval.StartDate;

    public override DateTime? EndDate => DateInterval.EndDate;

    public VacationDailyViewModel(DailyVacation dailyVacation)
        : base(dailyVacation)
    {
        DateInterval = dailyVacation.DateInterval;
    }

    protected override string RenderDate()
    {
        return $"[{DateInterval}]";
    }
}