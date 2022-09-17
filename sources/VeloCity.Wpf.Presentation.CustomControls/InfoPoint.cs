// Velo City
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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls
{
    public class InfoPoint : ContentControl
    {
        public static readonly DependencyProperty IconStyleProperty = DependencyProperty.Register(
            nameof(IconStyle),
            typeof(Style),
            typeof(InfoPoint),
            new FrameworkPropertyMetadata(defaultValue: null, flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public Style IconStyle
        {
            get => (Style)GetValue(IconStyleProperty);
            set => SetValue(IconStyleProperty, value);
        }

        public static readonly DependencyProperty IconGeometryProperty = DependencyProperty.Register(
            nameof(IconGeometry),
            typeof(Geometry),
            typeof(InfoPoint),
            new FrameworkPropertyMetadata(defaultValue: null, flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public Geometry IconGeometry
        {
            get => (Geometry)GetValue(IconGeometryProperty);
            set => SetValue(IconGeometryProperty, value);
        }

        public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(
            nameof(IconForeground),
            typeof(Brush),
            typeof(InfoPoint),
            new FrameworkPropertyMetadata(defaultValue: null, flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public Brush IconForeground
        {
            get => (Brush)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }

        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register(
            nameof(IconBackground),
            typeof(Brush),
            typeof(InfoPoint),
            new FrameworkPropertyMetadata(defaultValue: null, flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public Brush IconBackground
        {
            get => (Brush)GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }

        static InfoPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InfoPoint), new FrameworkPropertyMetadata(typeof(InfoPoint)));
        }
    }
}