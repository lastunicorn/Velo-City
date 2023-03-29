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

using DustInTheWind.VeloCity.Wpf.Presentation.Styles.Converters;

namespace DustInTheWind.VeloCity.Tests.Wpf.Presentation.Styles.Converters.BooleanToVisibilityInverseConverterTests;

public class ConvertBack_FromObjectTests
{
    private readonly BooleanToVisibilityInverseConverter converter;

    public ConvertBack_FromObjectTests()
    {
        converter = new BooleanToVisibilityInverseConverter();
    }

    [Fact]
    public void HavingNullValue_WhenConvertingBack_ThenReturnsFalse()
    {
        object value = null;

        bool actualBool = (bool)converter.ConvertBack(value, null, null, null);

        actualBool.Should().Be(false);
    }

    [Fact]
    public void HavingObjectValue_WhenConvertingBack_ThenReturnsFalse()
    {
        object value = new();

        bool actualBool = (bool)converter.ConvertBack(value, null, null, null);

        actualBool.Should().Be(false);
    }
}