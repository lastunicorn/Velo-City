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
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.VeloCity.Domain;

public class MonthCalendar
{
    private readonly DateTime startDate;
    private readonly DateTime endDate;

    public int Year { get; }

    public int Month { get; }

    public List<OfficialHoliday> OfficialHolidays { get; set; }

    public List<TeamMember> TeamMembers { get; set; }

    public IEnumerable<MonthMember> MonthMembers
    {
        get
        {
            return TeamMembers
                .Select(x => new MonthMember(x, this))
                .OrderBy(x => x.TeamMember.Employments.GetLastEmploymentBatch()?.StartDate)
                .ThenBy(x => x.Name);
        }
    }

    public MonthCalendar(DateTime startDate, DateTime endDate)
    {
        this.startDate = startDate;
        this.endDate = endDate;

        Year = startDate.Year;
        Month = startDate.Month;
    }

    public IEnumerable<SprintDay> EnumerateAllDays()
    {
        return EnumerateDays(startDate, endDate);
    }

    private IEnumerable<SprintDay> EnumerateDays(DateTime startDate, DateTime endDate)
    {
        int totalDaysCount = (int)(endDate.Date - startDate.Date).TotalDays + 1;

        return Enumerable.Range(0, totalDaysCount)
            .Select(x =>
            {
                DateTime date = startDate.AddDays(x);
                return ToSprintDay(date);
            });
    }

    private SprintDay ToSprintDay(DateTime date)
    {
        return new SprintDay
        {
            Date = date,
            OfficialHolidays = OfficialHolidays
                .Where(x => x.Match(date))
                .Select(x => x.GetInstanceFor(date.Year))
                .ToList()
        };
    }
}