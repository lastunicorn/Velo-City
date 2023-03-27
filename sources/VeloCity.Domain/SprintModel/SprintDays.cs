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
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;

namespace DustInTheWind.VeloCity.Domain.SprintModel;

public class SprintDays
{
    private readonly Sprint sprint;
    private readonly List<OfficialHoliday> officialHolidays;

    public DateTime StartDate { get; }

    public DateTime EndDate { get; }

    public SprintDays(Sprint sprint, List<OfficialHoliday> officialHolidays)
    {
        this.sprint = sprint ?? throw new ArgumentNullException(nameof(sprint));
        this.officialHolidays = officialHolidays ?? throw new ArgumentNullException(nameof(officialHolidays));

        StartDate = sprint.StartDate;
        EndDate = sprint.EndDate;
    }

    public IEnumerable<SprintDay> EnumerateAllDays()
    {
        int totalDaysCount = (int)(sprint.EndDate.Date - sprint.StartDate.Date).TotalDays + 1;

        return Enumerable.Range(0, totalDaysCount)
            .Select(x =>
            {
                DateTime date = sprint.StartDate.AddDays(x);
                return ToSprintDay(date);
            });
    }

    private SprintDay ToSprintDay(DateTime date)
    {
        return new SprintDay
        {
            Date = date,
            OfficialHolidays = officialHolidays
                .Where(x => x.Match(date))
                .Select(x => x.GetInstanceFor(date.Year))
                .ToList()
        };
    }
}