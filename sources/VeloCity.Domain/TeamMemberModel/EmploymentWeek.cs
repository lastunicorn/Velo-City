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

using System.Collections;

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class EmploymentWeek : IEnumerable<DayOfWeek>
{
    private readonly SortedSet<DayOfWeek> workDays = new();

    public bool IsDefault { get; private init; }

    public static EmploymentWeek NewDefault
    {
        get
        {
            IEnumerable<DayOfWeek> days = GetDefaultWorkDays();
            return new EmploymentWeek(days)
            {
                IsDefault = true
            };
        }
    }

    public EmploymentWeek()
    {
    }

    public EmploymentWeek(IEnumerable<DayOfWeek> workDays)
    {
        if (workDays == null)
            return;

        foreach (DayOfWeek dayOfWeek in workDays)
            this.workDays.Add(dayOfWeek);
    }

    private static IEnumerable<DayOfWeek> GetDefaultWorkDays()
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