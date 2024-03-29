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

using DustInTheWind.VeloCity.Ports.SettingsAccess;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.Configuring.ConfigurationOpenExceptionTests;

public class ConstructorTests
{
    [Fact]
    public void WhenCreatingInstanceWithNullInnerException_ThenMessageContainsDefaultText()
    {
        ConfigurationOpenException configurationOpenException = new(null);

        configurationOpenException.Message.Should().Be(Resources.ConfigurationOpen_DefaultErrorMessage);
    }

    [Fact]
    public void WhenCreatingInstanceWithNullInnerException_ThenInnerExceptionIsNull()
    {
        ConfigurationOpenException configurationOpenException = new(null);

        configurationOpenException.InnerException.Should().BeNull();
    }

    [Fact]
    public void WhenCreatingInstanceWithSpecificInnerException_ThenMessageContainsDefaultText()
    {
        Exception innerException = new();
        ConfigurationOpenException configurationOpenException = new(innerException);

        configurationOpenException.Message.Should().Be(Resources.ConfigurationOpen_DefaultErrorMessage);
    }

    [Fact]
    public void WhenCreatingInstanceWithSpecificInnerException_ThenInnerExceptionIsThatException()
    {
        Exception innerException = new();
        ConfigurationOpenException configurationOpenException = new(innerException);

        configurationOpenException.InnerException.Should().BeSameAs(innerException);
    }
}