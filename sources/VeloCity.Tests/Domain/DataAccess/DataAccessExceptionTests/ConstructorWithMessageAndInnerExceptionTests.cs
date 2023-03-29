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
using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.Tests.Domain.DataAccess.DataAccessExceptionTests;

public class ConstructorWithMessageAndInnerExceptionTests
{
    [Fact]
    public void WhenCreatingInstanceWithSpecificMessageAndInnerException_ThenMessageIsTheProvidedOne()
    {
        Exception innerException = new();
        DataAccessException dataAccessException = new("custom message", innerException);

        dataAccessException.Message.Should().Be("custom message");
    }

    [Fact]
    public void WhenCreatingInstanceWithSpecificMessageAndInnerException_ThenInnerExceptionIsTheProvidedOne()
    {
        Exception innerException = new();
        DataAccessException dataAccessException = new("custom message", innerException);

        dataAccessException.InnerException.Should().BeSameAs(innerException);
    }
}