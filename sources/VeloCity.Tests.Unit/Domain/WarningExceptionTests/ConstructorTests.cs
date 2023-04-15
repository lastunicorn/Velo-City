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

using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.WarningExceptionTests;

public class ConstructorTests
{
    [Fact]
    public void WhenCreatingInstanceWithNullMessage_ThenMessageIsNotNull()
    {
        WarningException warningException = new(null);

        warningException.Message.Should().NotBeNull();
    }

    [Fact]
    public void WhenCreatingInstanceWithSpecificMessageText_ThenMessageIsThatText()
    {
        WarningException warningException = new("custom text");

        warningException.Message.Should().Be("custom text");
    }
}