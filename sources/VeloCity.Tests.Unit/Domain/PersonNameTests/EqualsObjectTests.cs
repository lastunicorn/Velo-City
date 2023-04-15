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

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.PersonNameTests;

public class EqualsObjectTests
{
    [Fact]
    public void HavingAPersonNameInstance_WhenComparedWithNull_ThenReturnsFalse()
    {
        PersonName personName = new();

        bool actual = personName.Equals(null as object);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingAPersonNameInstance_WhenComparedWithObject_ThenReturnsFalse()
    {
        PersonName personName = new();
        object obj = new();

        bool actual = personName.Equals(obj);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingAPersonNameInstance_WhenComparedWithAnotherParsonNameAsObject_ThenReturnsTrue()
    {
        PersonName personName = new();
        object obj = new PersonName();

        bool actual = personName.Equals(obj);

        actual.Should().BeTrue();
    }
}