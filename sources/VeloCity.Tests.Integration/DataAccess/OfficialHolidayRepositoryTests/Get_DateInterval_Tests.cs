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

using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.OfficialHolidayRepositoryTests;

public class Get_DateInterval_Tests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\OfficialHolidayRepositoryTests";

    [Fact]
    public async Task HavingEmptyDatabase_WhenGetByDateInterval_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date-interval.empty.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;
                DateInterval dateInterval = new(startDate, endDate);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(dateInterval);

                officialHolidays.Should().BeEmpty();
            });
    }

    [Fact]
    public async Task HavingRecurrentHolidayInDatabaseAndDateIntervalNotMatchingIt_WhenGetByDateInterval_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date-interval.one-recurrent.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2023, 07, 01);
                DateTime endDate = new(2023, 10, 01);
                DateInterval dateInterval = new(startDate, endDate);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(dateInterval);

                officialHolidays.Should().BeEmpty();
            });
    }

    [Fact]
    public async Task HavingRecurrentHolidayInDatabaseAndDateIntervalMatchingOneYear_WhenGetByDateInterval_ThenReturnsOneItem()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date-interval.one-recurrent.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2023, 05, 01);
                DateTime endDate = new(2023, 10, 01);
                DateInterval dateInterval = new(startDate, endDate);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(dateInterval);

                officialHolidays.Should().HaveCount(1);
            });
    }

    [Fact]
    public async Task HavingRecurrentHolidayInDatabaseAndDateIntervalMatchingTwoYears_WhenGetByDateInterval_ThenReturnsOneItem()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date-interval.one-recurrent.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2022, 05, 01);
                DateTime endDate = new(2023, 10, 01);
                DateInterval dateInterval = new(startDate, endDate);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(dateInterval);

                officialHolidays.Should().HaveCount(1);
            });
    }

    [Fact]
    public async Task HavingFixedHolidayInDatabaseAndDateIntervalMatchingItsYear_WhenGetByDateInterval_ThenReturnsOneItem()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date-interval.one-fixed.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2022, 02, 01);
                DateTime endDate = new(2022, 10, 01);
                DateInterval dateInterval = new(startDate, endDate);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(dateInterval);

                officialHolidays.Should().HaveCount(1);
            });
    }

    [Fact]
    public async Task HavingFixedHolidayInDatabaseAndDateIntervalMatchingDayButNotYear_WhenGetByDateInterval_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date-interval.one-fixed.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2024, 02, 01);
                DateTime endDate = new(2024, 10, 01);
                DateInterval dateInterval = new(startDate, endDate);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(dateInterval);

                officialHolidays.Should().BeEmpty();
            });
    }

    [Fact]
    public async Task HavingMultipleHolidaysInDatabaseAndInfiniteDateInterval_WhenGetByDateInterval_ThenReturnsAllItems()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-by-date-interval.two.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);
                
                DateInterval dateInterval = DateInterval.FullInfinite;
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(dateInterval);

                officialHolidays.Should().HaveCount(2);
            });
    }
}