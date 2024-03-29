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

namespace DustInTheWind.VeloCity.Domain.OfficialHolidayModel;

public abstract class OfficialHoliday
{
    public DateTime Date { get; set; }

    public string Name { get; set; }

    public string Country { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public abstract bool Match(int year);

    public abstract bool Match(DateTime date);

    public abstract bool Match(DateTime startDate, DateTime endDate);

    public abstract OfficialHolidayInstance GetInstanceFor(int year);

    public IEnumerable<OfficialHolidayInstance> GetInstancesFor(DateTime startDate, DateTime endDate)
    {
        for (int year = startDate.Year; year <= endDate.Year; year++)
        {
            OfficialHolidayInstance officialHolidayInstance = GetInstanceFor(year);

            if (officialHolidayInstance.Date >= startDate && officialHolidayInstance.Date <= endDate)
                yield return officialHolidayInstance;
        }
    }

    public override string ToString()
    {
        return $"{Date:d} - {Name}";
    }
}