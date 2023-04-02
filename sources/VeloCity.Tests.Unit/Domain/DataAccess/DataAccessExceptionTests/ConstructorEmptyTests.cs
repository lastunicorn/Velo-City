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

using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.DataAccess.DataAccessExceptionTests;

public class ConstructorEmptyTests
{
    [Fact]
    public void WhenCreatingInstanceWithoutParameters_ThenMessageIsTheDefaultOne()
    {
        DataAccessException dataAccessException = new();

        dataAccessException.Message.Should().Be(Resources.DataAccess_DefaultErrorMessage);
    }

    [Fact]
    public void WhenCreatingInstanceWithoutParameters_ThenInnerExceptionIsNull()
    {
        DataAccessException dataAccessException = new();

        dataAccessException.InnerException.Should().BeNull();
    }
}