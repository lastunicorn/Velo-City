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

using System.Globalization;
using DustInTheWind.VeloCity.Infrastructure;

namespace DustInTheWind.VeloCity.Tests.Infrastructure.DateMonthTests;

public class ToStringWithFormatAndCultureTests
{
    [Theory]
    [InlineData(2025, 04, "2025 04")]
    [InlineData(3458, 01, "3458 01")]
    [InlineData(1, 03, "0001 03")]
    [InlineData(100, 12, "0100 12")]
    public void HavingAnInstance_WhenSerializedWithEmptyFormatForRomanianCulture_ThenReturnsYearAndMonthAsNumbers(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        CultureInfo cultureInfo = new("ro-RO");
        string actual = dateMonth.ToString("", cultureInfo);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(2025, 04, "2025 04")]
    [InlineData(3458, 01, "3458 01")]
    [InlineData(1, 03, "0001 03")]
    [InlineData(100, 12, "0100 12")]
    public void HavingAnInstance_WhenSerializedWithNullFormatForRomanianCulture_ThenReturnsYearAndMonthAsNumbers(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        CultureInfo cultureInfo = new("ro-RO");
        string actual = dateMonth.ToString("", cultureInfo);

        actual.Should().Be(expected);
    }

    [Fact]
    public void HavingAnInstance_WhenSerializedWithInvalidFormatForRomanianCulture_ThenThrows()
    {
        DateMonth dateMonth = new(2025, 08);
        CultureInfo cultureInfo = new("ro-RO");

        Action action = () =>
        {
            _ = dateMonth.ToString("blabla", cultureInfo);
        };

        action.Should().Throw<FormatException>();
    }

    [Theory]
    [InlineData(2025, 04, "2025 04")]
    [InlineData(3458, 01, "3458 01")]
    [InlineData(1, 03, "0001 03")]
    [InlineData(100, 12, "0100 12")]
    public void HavingAnInstance_WhenSerializedWithShortNumberFormatForRomanianCulture_ThenReturnsYearAndMonthAsNumbers(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        CultureInfo cultureInfo = new("ro-RO");
        string actual = dateMonth.ToString("number", cultureInfo);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(2025, 04, "2025 apr.")]
    [InlineData(3458, 01, "3458 ian.")]
    [InlineData(1, 03, "0001 mar.")]
    [InlineData(100, 12, "0100 dec.")]
    public void HavingAnInstance_WhenSerializedWithShortNameFormatForRomanianCulture_ThenReturnsYearAsNumberAndMonthAsName(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        CultureInfo cultureInfo = new("ro-RO");
        string actual = dateMonth.ToString("short-name", cultureInfo);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(2025, 04, "2025 aprilie")]
    [InlineData(3458, 01, "3458 ianuarie")]
    [InlineData(1, 03, "0001 martie")]
    [InlineData(100, 12, "0100 decembrie")]
    public void HavingAnInstance_WhenSerializedWithLongNameFormatForRomanianCulture_ThenReturnsYearAsNumberAndMonthAsName(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        CultureInfo cultureInfo = new("ro-RO");
        string actual = dateMonth.ToString("long-name", cultureInfo);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(2025, 04, "2025 04")]
    [InlineData(3458, 01, "3458 01")]
    [InlineData(1, 03, "0001 03")]
    [InlineData(100, 12, "0100 12")]
    public void HavingAnInstance_WhenSerializedWithShortNumberFormatForUSCulture_ThenReturnsYearAndMonthAsNumbers(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        CultureInfo cultureInfo = new("en-US");
        string actual = dateMonth.ToString("number", cultureInfo);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(2025, 04, "2025 Apr")]
    [InlineData(3458, 01, "3458 Jan")]
    [InlineData(1, 03, "0001 Mar")]
    [InlineData(100, 12, "0100 Dec")]
    public void HavingAnInstance_WhenSerializedWithShortNameFormatForUSCulture_ThenReturnsYearAsNumberAndMonthAsName(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        CultureInfo cultureInfo = new("en-US");
        string actual = dateMonth.ToString("short-name", cultureInfo);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(2025, 04, "2025 April")]
    [InlineData(3458, 01, "3458 January")]
    [InlineData(1, 03, "0001 March")]
    [InlineData(100, 12, "0100 December")]
    public void HavingAnInstance_WhenSerializedWithLongNameFormatForUSCulture_ThenReturnsYearAsNumberAndMonthAsName(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        CultureInfo cultureInfo = new("en-US");
        string actual = dateMonth.ToString("long-name", cultureInfo);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(2025, 04, "2025 aprilie")]
    [InlineData(3458, 01, "3458 ianuarie")]
    [InlineData(1, 03, "0001 martie")]
    [InlineData(100, 12, "0100 decembrie")]
    public void HavingAnInstance_WhenSerializedWithNullCulture_ThenUsesCurrentCulture(int year, int month, string expected)
    {
        DateMonth dateMonth = new(year, month);

        string actual = CultureSpecific.RunUsingCulture(new CultureInfo("ro-RO"), () =>
        {
            return dateMonth.ToString("long-name", null);
        });

        actual.Should().Be(expected);
    }
}