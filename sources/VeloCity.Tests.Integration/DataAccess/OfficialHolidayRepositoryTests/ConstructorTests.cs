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

using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.JsonFiles;
using FluentAssertions;

namespace DustInTheWind.VeloCity.Tests.Integration.DataAccess.OfficialHolidayRepositoryTests;

public class ConstructorTests
{
    [Fact]
    public void HavingNullContext_WhenInstantiating_ThenThrows()
    {
        Action action = () =>
        {
            _ = new OfficialHolidayRepository(null);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingCorrectDependencies_WhenInstantiating_ThenDoesNotThrow()
    {
        JsonDatabase jsonDatabase = new();
        VeloCityDbContext veloCityDbContext = new(jsonDatabase);

        Action action = () =>
        {
            _ = new OfficialHolidayRepository(veloCityDbContext);
        };

        action.Should().NotThrow();
    }
}