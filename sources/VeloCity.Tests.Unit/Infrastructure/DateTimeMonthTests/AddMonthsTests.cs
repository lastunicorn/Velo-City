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

using DustInTheWind.VeloCity.Infrastructure;

namespace DustInTheWind.VeloCity.Tests.Unit.Infrastructure.DateTimeMonthTests;

public class AddMonthsTests
{
    [Fact]
    public void HavingMonth_WhenSubtractingZeroMonths_ThenReturnsSameMonth()
    {
        DateTimeMonth dateTimeMonth = new(2022, 02);

        DateTimeMonth actual = dateTimeMonth.AddMonths(0);

        actual.Year.Should().Be(2022);
        actual.Month.Should().Be(02);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    public void HavingAMonthLessThan12_WhenAddingOneMonth_ThenReturnsTheNextMonthFromTheSameYear(int currentMonth)
    {
        DateTimeMonth dateTimeMonth = new(2022, currentMonth);

        DateTimeMonth actual = dateTimeMonth.AddMonths(1);

        actual.Year.Should().Be(2022);
        actual.Month.Should().Be(currentMonth + 1);
    }

    [Fact]
    public void HavingADecemberMonth_WhenAddingOneMonth_ThenReturnsTheJanuaryFromTheNextYear()
    {
        DateTimeMonth dateTimeMonth = new(2022, 12);

        DateTimeMonth actual = dateTimeMonth.AddMonths(1);

        actual.Year.Should().Be(2023);
        actual.Month.Should().Be(01);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    public void HavingAMonthGreaterThan1_WhenSubtractingOneMonth_ThenReturnsThePreviousMonthFromTheSameYear(int currentMonth)
    {
        DateTimeMonth dateTimeMonth = new(2022, currentMonth);

        DateTimeMonth actual = dateTimeMonth.AddMonths(-1);

        actual.Year.Should().Be(2022);
        actual.Month.Should().Be(currentMonth - 1);
    }

    [Fact]
    public void HavingAJanuaryMonth_WhenSubtractingOneMonth_ThenReturnsTheDecemberFromThePreviousYear()
    {
        DateTimeMonth dateTimeMonth = new(2022, 01);

        DateTimeMonth actual = dateTimeMonth.AddMonths(-1);

        actual.Year.Should().Be(2021);
        actual.Month.Should().Be(12);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public void HavingAMonth_WhenAddingTwoMonths_ThenReturnsTheSecondNextMonthFromTheSameYear(int currentMonth)
    {
        DateTimeMonth dateTimeMonth = new(2022, currentMonth);

        DateTimeMonth actual = dateTimeMonth.AddMonths(2);

        actual.Year.Should().Be(2022);
        actual.Month.Should().Be(currentMonth + 2);
    }

    [Fact]
    public void HavingANovemberMonth_WhenAddingTwoMonths_ThenReturnsTheJanuaryFromTheNextYear()
    {
        DateTimeMonth dateTimeMonth = new(2022, 11);

        DateTimeMonth actual = dateTimeMonth.AddMonths(2);

        actual.Year.Should().Be(2023);
        actual.Month.Should().Be(01);
    }

    [Fact]
    public void HavingADecemberMonth_WhenAddingTwoMonths_ThenReturnsTheFebruaryFromTheNextYear()
    {
        DateTimeMonth dateTimeMonth = new(2022, 12);

        DateTimeMonth actual = dateTimeMonth.AddMonths(2);

        actual.Year.Should().Be(2023);
        actual.Month.Should().Be(02);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    public void HavingAMonth_WhenSubtractingTwoMonths_ThenReturnsTheSecondPreviousMonthFromTheSameYear(int currentMonth)
    {
        DateTimeMonth dateTimeMonth = new(2022, currentMonth);

        DateTimeMonth actual = dateTimeMonth.AddMonths(-2);

        actual.Year.Should().Be(2022);
        actual.Month.Should().Be(currentMonth - 2);
    }

    [Fact]
    public void HavingAFebruaryMonth_WhenSubtractingTwoMonths_ThenReturnsTheDecemberFromThePreviousYear()
    {
        DateTimeMonth dateTimeMonth = new(2022, 02);

        DateTimeMonth actual = dateTimeMonth.AddMonths(-2);

        actual.Year.Should().Be(2021);
        actual.Month.Should().Be(12);
    }

    [Fact]
    public void HavingAJanuaryMonth_WhenSubtractingTwoMonths_ThenReturnsTheNovemberFromThePreviousYear()
    {
        DateTimeMonth dateTimeMonth = new(2022, 01);

        DateTimeMonth actual = dateTimeMonth.AddMonths(-2);

        actual.Year.Should().Be(2021);
        actual.Month.Should().Be(11);
    }

    [Fact]
    public void HavingAJuneMonth_WhenAddingTwentyMonths_ThenReturnsTheFebruaryFromTheSecondNextYear()
    {
        DateTimeMonth dateTimeMonth = new(2022, 06);

        DateTimeMonth actual = dateTimeMonth.AddMonths(20);

        actual.Year.Should().Be(2024);
        actual.Month.Should().Be(02);
    }

    [Fact]
    public void HavingAFebruaryMonth_WhenSubtractingTwentyMonths_ThenReturnsTheJuneFromTheSecondPreviousYear()
    {
        DateTimeMonth dateTimeMonth = new(2022, 02);

        DateTimeMonth actual = dateTimeMonth.AddMonths(-20);

        actual.Year.Should().Be(2020);
        actual.Month.Should().Be(06);
    }
}