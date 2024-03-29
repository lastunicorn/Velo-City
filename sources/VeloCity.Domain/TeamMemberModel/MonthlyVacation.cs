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

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class MonthlyVacation : Vacation
{
    private List<int> monthDays;
    private DateInterval dateInterval;

    public List<int> MonthDays
    {
        get => monthDays;
        set
        {
            monthDays = value;
            OnChanged();
        }
    }

    public DateInterval DateInterval
    {
        get => dateInterval;
        set
        {
            dateInterval = value;
            OnChanged();
        }
    }

    public override bool Match(DateTime date)
    {
        if (!DateInterval.ContainsDate(date))
            return false;

        return MonthDays?.Contains(date.Day) ?? false;
    }
}