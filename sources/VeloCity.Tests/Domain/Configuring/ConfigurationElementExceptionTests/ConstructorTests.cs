// Velo City
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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Ports.SettingsAccess;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Domain.Configuring.ConfigurationElementExceptionTests
{
    public class ConstructorTests
    {
        [Fact]
        public void WhenCreatingInstanceWithSpecificElementName_ThenMessageContainsTheProvidedName()
        {
            Exception innerException = new();
            ConfigurationElementException configurationElementException = new("element1", innerException);

            string expected = string.Format(DustInTheWind.VeloCity.Ports.SettingsAccess.Resources.ConfigurationElement_DefaultErrorMessage, "element1");
            configurationElementException.Message.Should().Be(expected);
        }

        [Fact]
        public void WhenCreatingInstanceWithNullElementName_ThenMessageContainsDefaultMessageWithoutElementName()
        {
            Exception innerException = new();
            ConfigurationElementException configurationElementException = new(null, innerException);

            string expected = string.Format(DustInTheWind.VeloCity.Ports.SettingsAccess.Resources.ConfigurationElement_DefaultErrorMessage, null as string);
            configurationElementException.Message.Should().Be(expected);
        }

        [Fact]
        public void WhenCreatingInstanceWithSpecificInnerException_ThenInnerExceptionIsTheProvidedOne()
        {
            Exception innerException = new();
            ConfigurationElementException configurationElementException = new("element1", innerException);

            configurationElementException.InnerException.Should().BeSameAs(innerException);
        }

        [Fact]
        public void WhenCreatingInstanceWithNullInnerException_ThenInnerExceptionIsTheProvidedOne()
        {
            ConfigurationElementException configurationElementException = new("element1", null);

            configurationElementException.InnerException.Should().BeNull();
        }
    }
}