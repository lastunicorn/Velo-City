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

using DustInTheWind.VeloCity.Infrastructure;

namespace DustInTheWind.VeloCity.Tests.Unit.Infrastructure.DateMonthTests;

public class CompareToTests
{
    [Fact]
    public void HavingOneInstance_WhenComparedToItself_ThenReturnsZero()
    {
        DateMonth dateMonth = new(2022, 01);

        int actual = dateMonth.CompareTo(dateMonth);

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingTwoInstancesRepresentingSameMonth_WhenCompared_ThenReturnsZero()
    {
        DateMonth dateMonth1 = new(2022, 01);
        DateMonth dateMonth2 = new(2022, 01);

        int actual = dateMonth1.CompareTo(dateMonth2);

        actual.Should().Be(0);
    }

    [Fact]
    public void HavingOneInstanceRepresentingJanuaryAndSecondOneRepresentingFebruary_WhenCompared_ThenReturnsNegativeValue()
    {
        DateMonth dateMonth1 = new(2022, 01);
        DateMonth dateMonth2 = new(2022, 02);

        int actual = dateMonth1.CompareTo(dateMonth2);

        actual.Should().BeLessThan(0);
    }

    [Fact]
    public void HavingOneInstanceRepresentingFebruaryAndSecondOneRepresentingJanuary_WhenCompared_ThenReturnsPositiveValue()
    {
        DateMonth dateMonth1 = new(2022, 02);
        DateMonth dateMonth2 = new(2022, 01);

        int actual = dateMonth1.CompareTo(dateMonth2);

        actual.Should().BeGreaterThan(0);
    }

    [Fact]
    public void HavingTwoInstancesWithSameMonthButFirstWithLowerYearValue_WhenCompared_ThenReturnsNegativeValue()
    {
        DateMonth dateMonth1 = new(2021, 05);
        DateMonth dateMonth2 = new(2022, 05);

        int actual = dateMonth1.CompareTo(dateMonth2);

        actual.Should().BeLessThan(0);
    }

    [Fact]
    public void HavingTwoInstancesWithSameMonthButFirstWithHigherYearValue_WhenCompared_ThenReturnsPositiveValue()
    {
        DateMonth dateMonth1 = new(2024, 06);
        DateMonth dateMonth2 = new(2022, 06);

        int actual = dateMonth1.CompareTo(dateMonth2);

        actual.Should().BeGreaterThan(0);
    }
}