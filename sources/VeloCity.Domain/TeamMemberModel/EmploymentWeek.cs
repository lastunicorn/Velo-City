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
using System.Collections;
using System.Collections.Generic;

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class EmploymentWeek : IEnumerable<DayOfWeek>
{
    private readonly SortedSet<DayOfWeek> workDays;

    public bool IsDefault { get; }

    public EmploymentWeek()
    {
        workDays = GetDefaultWorkDays();
        IsDefault = true;
    }

    public EmploymentWeek(IEnumerable<DayOfWeek> workDays)
    {
        if (workDays == null)
        {
            this.workDays = GetDefaultWorkDays();
            IsDefault = true;
        }
        else
        {
            this.workDays = new SortedSet<DayOfWeek>(workDays);
        }
    }

    private static SortedSet<DayOfWeek> GetDefaultWorkDays()
    {
        return new SortedSet<DayOfWeek>
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };
    }

    public bool IsWorkDay(DayOfWeek dayOfWeek)
    {
        return workDays.Contains(dayOfWeek);
    }

    public IEnumerator<DayOfWeek> GetEnumerator()
    {
        return workDays.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return string.Join(", ", workDays);
    }
}