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
        #region IconGeometry Property

        public static readonly DependencyProperty IconGeometryProperty = DependencyProperty.Register(
            nameof(IconGeometry),
            typeof(Geometry),
            typeof(InfoPoint)
        );

        public Geometry IconGeometry
        {
            get => (Geometry)GetValue(IconGeometryProperty);
            set => SetValue(IconGeometryProperty, value);
        }

        #endregion

        #region IconWidth Property

        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(
            nameof(IconWidth),
            typeof(double),
            typeof(InfoPoint),
            new FrameworkPropertyMetadata(defaultValue: 12d, flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public double IconWidth
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        #endregion

        #region IconHeight Property

        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(
            nameof(IconHeight),
            typeof(double),
            typeof(InfoPoint),
            new FrameworkPropertyMetadata(defaultValue: 12d, flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public double IconHeight
        {
            get => (double)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        #endregion

        #region IconForeground Property

        public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(
            nameof(IconForeground),
            typeof(Brush),
            typeof(InfoPoint)
        );

        public Brush IconForeground
        {
            get => (Brush)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }

        #endregion

        #region IconBackground Property

        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register(
            nameof(IconBackground),
            typeof(Brush),
            typeof(InfoPoint)
        );

        public Brush IconBackground
        {
            get => (Brush)GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }

        #endregion

        #region InfoForeground Property

        public static readonly DependencyProperty InfoForegroundProperty = DependencyProperty.Register(
            nameof(InfoForeground),
            typeof(Brush),
            typeof(InfoPoint)
        );

        public Brush InfoForeground
        {
            get => (Brush)GetValue(InfoForegroundProperty);
            set => SetValue(InfoForegroundProperty, value);
        }

        #endregion

        #region InfoBackground Property

        public static readonly DependencyProperty InfoBackgroundProperty = DependencyProperty.Register(
            nameof(InfoBackground),
            typeof(Brush),
            typeof(InfoPoint)
        );

        public Brush InfoBackground
        {
            get => (Brush)GetValue(InfoBackgroundProperty);
            set => SetValue(InfoBackgroundProperty, value);
        }

        #endregion

        #region InfoBorderThickness Property

        public static readonly DependencyProperty InfoBorderThicknessProperty = DependencyProperty.Register(
            nameof(InfoBorderThickness),
            typeof(Thickness),
            typeof(InfoPoint)
        );

        public Thickness InfoBorderThickness
        {
            get => (Thickness)GetValue(InfoBorderThicknessProperty);
            set => SetValue(InfoBorderThicknessProperty, value);
        }

        #endregion

        #region InfoBorderBrush Property

        public static readonly DependencyProperty InfoBorderBrushProperty = DependencyProperty.Register(
            nameof(InfoBorderBrush),
            typeof(Brush),
            typeof(InfoPoint)
        );

        public Brush InfoBorderBrush
        {
            get => (Brush)GetValue(InfoBorderBrushProperty);
            set => SetValue(InfoBorderBrushProperty, value);
        }

        #endregion

        #region InfoContent Property

        public static readonly DependencyProperty InfoContentProperty = DependencyProperty.Register(
            nameof(InfoContent),
            typeof(object),
            typeof(InfoPoint),
            new FrameworkPropertyMetadata(defaultValue: null, flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public object InfoContent
        {
            get => GetValue(InfoContentProperty);
            set => SetValue(InfoContentProperty, value);
        }

        #endregion

        #region SpaceBetweenContentAndIcon Property

        public static readonly DependencyProperty SpaceBetweenContentAndIconProperty = DependencyProperty.Register(
            nameof(SpaceBetweenContentAndIcon),
            typeof(double),
            typeof(InfoPoint),
            new FrameworkPropertyMetadata(defaultValue: 8d, flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, HandleSpaceBetweenContentAndIconPropertyChanged)
        );

        private static void HandleSpaceBetweenContentAndIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is InfoPoint infoPoint && e.NewValue is double newValue)
            {
                Thickness margin = infoPoint.Content == null
                    ? new Thickness(0)
                    : new Thickness(newValue, 0, 0, 0);

                d.SetValue(IconMarginPropertyKey, margin);
            }
        }

        public double SpaceBetweenContentAndIcon
        {
            get => (double)GetValue(SpaceBetweenContentAndIconProperty);
            set => SetValue(SpaceBetweenContentAndIconProperty, value);
        }

        #endregion

        #region IconMargin Property

        private static readonly DependencyPropertyKey IconMarginPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IconMargin),
            typeof(Thickness),
            typeof(InfoPoint),
            new FrameworkPropertyMetadata(defaultValue: new Thickness(0), flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public static readonly DependencyProperty IconMarginProperty = IconMarginPropertyKey.DependencyProperty;

        public Thickness IconMargin => (Thickness)GetValue(IconMarginProperty);

        #endregion

        #region IsInfoVisible Property

        public static readonly DependencyProperty IsInfoVisibleProperty = DependencyProperty.Register(
            nameof(IsInfoVisible),
            typeof(bool),
            typeof(InfoPoint)
        );

        public bool IsInfoVisible
        {
            get => (bool)GetValue(IsInfoVisibleProperty);
            set => SetValue(IsInfoVisibleProperty, value);
        }

        #endregion

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            Thickness margin = newContent == null
                ? new Thickness(0)
                : new Thickness(SpaceBetweenContentAndIcon, 0, 0, 0);

            SetValue(IconMarginPropertyKey, margin);
        }

        static InfoPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InfoPoint), new FrameworkPropertyMetadata(typeof(InfoPoint)));
        }
    }
}