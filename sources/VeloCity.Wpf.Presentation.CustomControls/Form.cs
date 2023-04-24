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
using System.Windows.Controls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class Form : ItemsControl
{
    #region ItemSpace

    public static readonly DependencyProperty ItemSpaceProperty = DependencyProperty.Register(
        nameof(ItemSpace),
        typeof(double),
        typeof(Form),
        new PropertyMetadata(30d, PropertyChangedCallback)
    );

    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Form form && e.NewValue is double newDoubleValue)
        {
            form.FirstMargin = new Thickness(0);
            form.NormalMargin = new Thickness(0, newDoubleValue, 0, 0);
            form.LastMargin = new Thickness(0, newDoubleValue, 0, 0);
        }
    }

    public double ItemSpace
    {
        get => (double)GetValue(ItemSpaceProperty);
        set => SetValue(ItemSpaceProperty, value);
    }

    #endregion

    #region FirstMargin

    private static readonly DependencyPropertyKey FirstMarginPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(FirstMargin),
        typeof(Thickness),
        typeof(Form),
        new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
    );

    public static readonly DependencyProperty FirstMarginProperty = FirstMarginPropertyKey.DependencyProperty;

    public Thickness FirstMargin
    {
        get => (Thickness)GetValue(FirstMarginProperty);
        private set => SetValue(FirstMarginPropertyKey, value);
    }

    #endregion

    #region NormalMargin

    private static readonly DependencyPropertyKey NormalMarginPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(NormalMargin),
        typeof(Thickness),
        typeof(Form),
        new FrameworkPropertyMetadata(new Thickness(0, 30, 0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
    );

    public static readonly DependencyProperty NormalMarginProperty = NormalMarginPropertyKey.DependencyProperty;

    public Thickness NormalMargin
    {
        get => (Thickness)GetValue(NormalMarginProperty);
        private set => SetValue(NormalMarginPropertyKey, value);
    }

    #endregion

    #region LastMargin

    private static readonly DependencyPropertyKey LastMarginPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(LastMargin),
        typeof(Thickness),
        typeof(Form),
        new FrameworkPropertyMetadata(new Thickness(0, 30, 0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
    );

    public static readonly DependencyProperty LastMarginProperty = LastMarginPropertyKey.DependencyProperty;

    public Thickness LastMargin
    {
        get => (Thickness)GetValue(LastMarginProperty);
        private set => SetValue(LastMarginPropertyKey, value);
    }

    #endregion

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return false;
    }

    static Form()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Form), new FrameworkPropertyMetadata(typeof(Form)));
    }
}