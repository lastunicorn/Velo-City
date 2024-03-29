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

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles.Converters;

public class SprintStateToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SprintState sprintState)
        {
            return sprintState switch
            {
                SprintState.Unknown => "Unknown",
                SprintState.New => "New",
                SprintState.InProgress => "In Progress",
                SprintState.Closed => "Closed",
                _ => throw new ArgumentOutOfRangeException("The value must be a SprintState.", nameof(value))
            };
        }

        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}