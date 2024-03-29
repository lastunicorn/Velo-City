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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.DataAccess;

internal class OfficialHolidayRepository : IOfficialHolidayRepository
{
    private readonly VeloCityDbContext dbContext;

    public OfficialHolidayRepository(VeloCityDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<IEnumerable<OfficialHoliday>> GetAll()
    {
        IEnumerable<OfficialHoliday> items = dbContext.OfficialHolidays;
        return Task.FromResult(items);
    }

    public Task<IEnumerable<OfficialHoliday>> Get(DateTime startDate, DateTime endDate)
    {
        IEnumerable<OfficialHoliday> officialHolidays = dbContext.OfficialHolidays
            .Where(x => x.Match(startDate, endDate));

        return Task.FromResult(officialHolidays);
    }

    public Task<IEnumerable<OfficialHoliday>> Get(DateInterval dateInterval)
    {
        DateTime calculatedStartDate = dateInterval.StartDate ?? DateTime.MinValue;
        DateTime calculatedEndDate = dateInterval.EndDate ?? DateTime.MaxValue;

        IEnumerable<OfficialHoliday> officialHolidays = dbContext.OfficialHolidays
            .Where(x => x.Match(calculatedStartDate, calculatedEndDate));

        return Task.FromResult(officialHolidays);
    }

    public Task<IEnumerable<OfficialHoliday>> Get(DateTime startDate, DateTime endDate, string country)
    {
        IEnumerable<OfficialHoliday> officialHolidays = dbContext.OfficialHolidays
            .Where(x => string.Equals(x.Country, country, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => x.Match(startDate, endDate));

        return Task.FromResult(officialHolidays);
    }

    public Task<IEnumerable<OfficialHoliday>> GetByYear(int year)
    {
        IEnumerable<OfficialHoliday> officialHolidays = dbContext.OfficialHolidays
            .Where(x => x.Match(year));

        return Task.FromResult(officialHolidays);
    }

    public Task<IEnumerable<OfficialHoliday>> GetByYear(int year, string country)
    {
        IEnumerable<OfficialHoliday> officialHolidays = dbContext.OfficialHolidays
            .Where(x => string.Equals(x.Country, country, StringComparison.InvariantCultureIgnoreCase))
            .Where(x => x.Match(year));

        return Task.FromResult(officialHolidays);
    }
}