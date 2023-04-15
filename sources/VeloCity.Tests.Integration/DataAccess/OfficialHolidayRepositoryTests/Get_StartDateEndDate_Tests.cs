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
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using DustInTheWind.VeloCity.Tests.Integration.TestUtils;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.OfficialHolidayRepositoryTests;

public class Get_StartDateEndDate_Tests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\OfficialHolidayRepositoryTests";

    [Fact]
    public async Task HavingEmptyDatabase_WhenGetByStartDateEndDate_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get.empty.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(startDate, endDate);

                officialHolidays.Should().BeEmpty();
            });
    }

    [Fact]
    public async Task HavingRecurrentHolidayInDatabaseAndDateIntervalNotMatchingIt_WhenGetByStartDateEndDate_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get.one-recurrent.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2023, 07, 01);
                DateTime endDate = new(2023, 10, 01);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(startDate, endDate);

                officialHolidays.Should().BeEmpty();
            });
    }

    [Fact]
    public async Task HavingRecurrentHolidayInDatabaseAndDateIntervalMatchingOneYear_WhenGetByStartDateEndDate_ThenReturnsOneItem()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get.one-recurrent.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2023, 05, 01);
                DateTime endDate = new(2023, 10, 01);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(startDate, endDate);

                officialHolidays.Should().HaveCount(1);
            });
    }

    [Fact]
    public async Task HavingRecurrentHolidayInDatabaseAndDateIntervalMatchingTwoYears_WhenGetByStartDateEndDate_ThenReturnsOneItem()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get.one-recurrent.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2022, 05, 01);
                DateTime endDate = new(2023, 10, 01);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(startDate, endDate);

                officialHolidays.Should().HaveCount(1);
            });
    }

    [Fact]
    public async Task HavingFixedHolidayInDatabaseAndDateIntervalMatchingItsYear_WhenGetByStartDateEndDate_ThenReturnsOneItem()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get.one-fixed.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2022, 02, 01);
                DateTime endDate = new(2022, 10, 01);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(startDate, endDate);

                officialHolidays.Should().HaveCount(1);
            });
    }

    [Fact]
    public async Task HavingFixedHolidayInDatabaseAndDateIntervalMatchingDayButNotYear_WhenGetByStartDateEndDate_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get.one-fixed.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                DateTime startDate = new(2024, 02, 01);
                DateTime endDate = new(2024, 10, 01);
                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.Get(startDate, endDate);

                officialHolidays.Should().BeEmpty();
            });
    }
}