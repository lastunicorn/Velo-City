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

public class GetAllTests
{
    private const string DatabaseDirectoryPath = @"TestData\DataAccess\OfficialHolidayRepositoryTests";

    [Fact]
    public async Task HavingEmptyDatabase_WhenGetAll_ThenReturnsEmptyCollection()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-all.empty.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.GetAll();

                officialHolidays.Should().BeEmpty();
            });
    }

    [Fact]
    public async Task HavingDatabaseWithOneOfficialHoliday_WhenGetAll_ThenReturnsThatOfficialHoliday()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-all.one.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.GetAll();

                officialHolidays.Should().HaveCount(1);
            });
    }

    [Fact]
    public async Task HavingDatabaseWithTwoOfficialHoliday_WhenGetAll_ThenReturnsThoseOfficialHolidays()
    {
        await DatabaseTestContext
            .WithDatabase(DatabaseDirectoryPath, "db-get-all.two.json")
            .Execute(async context =>
            {
                OfficialHolidayRepository officialHolidayRepository = new(context.DbContext);

                IEnumerable<OfficialHoliday> officialHolidays = await officialHolidayRepository.GetAll();

                officialHolidays.Should().HaveCount(2);
            });
    }
}