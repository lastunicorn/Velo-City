﻿// VeloCity
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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DatabaseEditing;

namespace DustInTheWind.VeloCity.Tests.Domain.DatabaseEditing.DatabaseFileNotFoundExceptionTests;

public class ConstructorTests
{
    [Fact]
    public void WhenCreatingInstanceWithNullFilePath_ThenMessageContainsEmptyFilePath()
    {
        DatabaseFileNotFoundException databaseFileNotFoundException = new(null);

        string expected = string.Format(Resources.DatabaseFileNotFound_DefaultErrorMessage, null as string);
        databaseFileNotFoundException.Message.Should().Be(expected);
    }

    [Fact]
    public void WhenCreatingInstanceWithSpecificFilePath_ThenMessageContainsThatFilePath()
    {
        DatabaseFileNotFoundException databaseFileNotFoundException = new("custom file path");

        string expected = string.Format(Resources.DatabaseFileNotFound_DefaultErrorMessage, "custom file path");
        databaseFileNotFoundException.Message.Should().Be(expected);
    }
}