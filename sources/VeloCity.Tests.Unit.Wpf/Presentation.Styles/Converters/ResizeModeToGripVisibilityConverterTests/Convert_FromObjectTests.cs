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

using System.Windows;
using DustInTheWind.VeloCity.Wpf.Presentation.Styles.Converters;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Presentation.Styles.Converters.ResizeModeToGripVisibilityConverterTests;

public class Convert_FromObjectTests
{
    private readonly ResizeModeToGripVisibilityConverter converter;

    public Convert_FromObjectTests()
    {
        converter = new ResizeModeToGripVisibilityConverter();
    }

    [Fact]
    public void HavingNullValue_WhenConverting_ThenReturnsCollapsed()
    {
        object value = null;

        Visibility actualVisibility = (Visibility)converter.Convert(value, null, null, null);

        actualVisibility.Should().Be(Visibility.Collapsed);
    }

    [Fact]
    public void HavingObjectValue_WhenConverting_ThenReturnsCollapsed()
    {
        object value = new();

        Visibility actualVisibility = (Visibility)converter.Convert(value, null, null, null);

        actualVisibility.Should().Be(Visibility.Collapsed);
    }
}