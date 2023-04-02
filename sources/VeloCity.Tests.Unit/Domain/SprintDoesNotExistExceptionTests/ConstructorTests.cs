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

using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.SprintDoesNotExistExceptionTests;

public class ConstructorTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(-10)]
    public void WhenCreatingInstanceWithSpecificSprintNumber_ThenMessageContainsThatSprintNumber(int sprintNumber)
    {
        SprintDoesNotExistException sprintDoesNotExistException = new(sprintNumber);

        string expected = string.Format(Resources.SprintDoesNotExist_DefaultErrorMessage, sprintNumber);
        sprintDoesNotExistException.Message.Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(-10)]
    public void WhenCreatingInstanceWithSpecificSprintNumber_ThenInnerExceptionIsNull(int sprintNumber)
    {
        SprintDoesNotExistException sprintDoesNotExistException = new(sprintNumber);

        sprintDoesNotExistException.InnerException.Should().BeNull();
    }
}