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
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Unit.Domain.TeamMemberModel.EmploymentTests;

public class DoesContinueWithTests
{
    [Fact]
    public void HavingEmployment_WhenCheckIfContinuesWithNull_ThenThrows()
    {
        Employment employment = new();

        Action action = () => employment.DoesContinueWith(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingInfiniteEmployment_WhenCheckingIfItContinuesWithStartFiniteEmployment_ReturnsFalse()
    {
        Employment employment = new();

        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(5400, 12, 14))
        };
        bool actual = employment.DoesContinueWith(employment2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingEmploymentWithMaximumEndDate_WhenCheckingIfItContinuesWithFiniteEmployment_ReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(null, DateTime.MaxValue)
        };

        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(5400, 12, 14))
        };
        bool actual = employment.DoesContinueWith(employment2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingFiniteEmployment_WhenCheckingIfItContinuesWithStartInfiniteEmployment_ReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04))
        };

        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(null, new DateTime(5400, 12, 14))
        };
        bool actual = employment.DoesContinueWith(employment2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingFiniteEmployment_WhenCheckingIfItContinuesWithEmploymentStartingInTheFuture_ReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04))
        };

        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(5400, 12, 14))
        };
        bool actual = employment.DoesContinueWith(employment2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingFiniteEmployment_WhenCheckingIfItContinuesWithEmploymentStartingDuringInterval_ReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04))
        };

        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(1950, 12, 14))
        };
        bool actual = employment.DoesContinueWith(employment2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingFiniteEmployment_WhenCheckingIfItContinuesWithEmploymentStartingBeforeInterval_ReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04))
        };

        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(1800, 12, 14))
        };
        bool actual = employment.DoesContinueWith(employment2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingFiniteEmployment_WhenCheckingIfItContinuesWithEmploymentStartingFromTheEndDayOfTheInterval_ReturnsFalse()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04))
        };

        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2002, 08, 04))
        };
        bool actual = employment.DoesContinueWith(employment2);

        actual.Should().BeFalse();
    }

    [Fact]
    public void HavingFiniteDateEmployment_WhenCheckingIfItContinuesWithEmploymentStartingNextDayAfterInterval_ReturnsTrue()
    {
        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(1900, 07, 28), new DateTime(2002, 08, 04))
        };

        Employment employment2 = new()
        {
            TimeInterval = new DateInterval(new DateTime(2002, 08, 05))
        };
        bool actual = employment.DoesContinueWith(employment2);

        actual.Should().BeTrue();
    }
}