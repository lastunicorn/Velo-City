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
// using System;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class ValueBlock : ContentControl
{
    public static readonly DependencyProperty DescriptionContentProperty = DependencyProperty.Register(
        nameof(DescriptionContent),
        typeof(object),
        typeof(ValueBlock));

    public object DescriptionContent
    {
        get => GetValue(DescriptionContentProperty);
        set => SetValue(DescriptionContentProperty, value);
    }

    public static readonly DependencyProperty DescriptionFontSizeProperty = DependencyProperty.Register(
        nameof(DescriptionFontSize),
        typeof(double),
        typeof(ValueBlock),
        new FrameworkPropertyMetadata(16d, FrameworkPropertyMetadataOptions.Inherits));

    [TypeConverter(typeof(FontSizeConverter))]
    [Localizability(LocalizationCategory.None)]
    public double DescriptionFontSize
    {
        get => (double)GetValue(DescriptionFontSizeProperty);
        set => SetValue(DescriptionFontSizeProperty, value);
    }

    static ValueBlock()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueBlock), new FrameworkPropertyMetadata(typeof(ValueBlock)));
    }
}