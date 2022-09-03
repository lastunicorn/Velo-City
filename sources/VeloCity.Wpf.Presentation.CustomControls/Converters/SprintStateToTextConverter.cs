﻿// Velo City
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

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls.Converters
{
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
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class SprintStateToGeometryConverter : IValueConverter
    {
        public Geometry UnknownGeometry { get; set; }
        
        public Brush UnknownBrush { get; set; }

        public Geometry NewGeometry { get; set; }
        
        public Brush NewBrush { get; set; }

        public Geometry InProgressGeometry { get; set; }
        
        public Brush InProgressBrush { get; set; }

        public Geometry ClosedGeometry { get; set; }
        
        public Brush ClosedBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SprintState sprintState)
            {
                return new GenericIcon()
                {
                    Geometry = sprintState switch
                    {
                        SprintState.Unknown => UnknownGeometry,
                        SprintState.New => NewGeometry,
                        SprintState.InProgress => InProgressGeometry,
                        SprintState.Closed => ClosedGeometry,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    Foreground = sprintState switch
                    {
                        SprintState.Unknown => UnknownBrush,
                        SprintState.New => NewBrush,
                        SprintState.InProgress => InProgressBrush,
                        SprintState.Closed => ClosedBrush,
                        _ => throw new ArgumentOutOfRangeException()
                    }
                };

                //return sprintState switch
                //{
                //    SprintState.Unknown => UnknownGeometry,
                //    SprintState.New => NewGeometry,
                //    SprintState.InProgress => InProgressGeometry,
                //    SprintState.Closed => ClosedGeometry,
                //    _ => throw new ArgumentOutOfRangeException()
                //};
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}