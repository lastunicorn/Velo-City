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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.UserAccess.Converters;

internal class StoryPointsToNumberConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is StoryPoints storyPoints)
            return storyPoints.Value;

        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is float floatValue)
            return (StoryPoints)floatValue;

        if (value is string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
                return StoryPoints.Empty;

            try
            {
                floatValue = float.Parse(stringValue);
                return (StoryPoints)floatValue;
            }
            catch
            {
                return null;
            }
        }

        return DependencyProperty.UnsetValue;
    }
}