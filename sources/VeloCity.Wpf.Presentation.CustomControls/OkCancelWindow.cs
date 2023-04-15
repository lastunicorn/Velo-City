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

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class OkCancelWindow : Window
{
    public static readonly DependencyProperty TitleIconProperty = DependencyProperty.Register(
        nameof(TitleIcon),
        typeof(object),
        typeof(OkCancelWindow),
        new PropertyMetadata(HandleTitleIconPropertyChanged)
    );

    private static void HandleTitleIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is OkCancelWindow okCancelWindow)
        {
            okCancelWindow.IsTitleIconVisible = e.NewValue != null;
        }
    }

    public object TitleIcon
    {
        get => GetValue(TitleIconProperty);
        set => SetValue(TitleIconProperty, value);
    }

    public static readonly DependencyPropertyKey IsTitleIconVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsTitleIconVisible),
        typeof(bool),
        typeof(OkCancelWindow),
        new PropertyMetadata(false)
    );

    public static readonly DependencyProperty IsTitleIconVisibleProperty = IsTitleIconVisiblePropertyKey.DependencyProperty;

    public bool IsTitleIconVisible
    {
        get => (bool)GetValue(IsTitleIconVisibleProperty);
        private set => SetValue(IsTitleIconVisiblePropertyKey, value);
    }

    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
        nameof(Subtitle),
        typeof(string),
        typeof(OkCancelWindow)
    );

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public static readonly DependencyProperty OkButtonContentProperty = DependencyProperty.Register(
        nameof(OkButtonContent),
        typeof(object),
        typeof(OkCancelWindow),
        new PropertyMetadata("Ok")
    );

    public object OkButtonContent
    {
        get => GetValue(OkButtonContentProperty);
        set => SetValue(OkButtonContentProperty, value);
    }

    public static readonly DependencyProperty IsOkButtonVisibleProperty = DependencyProperty.Register(
        nameof(IsOkButtonVisible),
        typeof(bool),
        typeof(OkCancelWindow),
        new PropertyMetadata(true)
    );

    public bool IsOkButtonVisible
    {
        get => (bool)GetValue(IsOkButtonVisibleProperty);
        set => SetValue(IsOkButtonVisibleProperty, value);
    }

    public static readonly DependencyProperty CancelButtonContentProperty = DependencyProperty.Register(
        nameof(CancelButtonContent),
        typeof(object),
        typeof(OkCancelWindow),
        new PropertyMetadata("Cancel")
    );

    public object CancelButtonContent
    {
        get => GetValue(CancelButtonContentProperty);
        set => SetValue(CancelButtonContentProperty, value);
    }

    public static readonly DependencyProperty IsCancelButtonVisibleProperty = DependencyProperty.Register(
        nameof(IsCancelButtonVisible),
        typeof(bool),
        typeof(OkCancelWindow),
        new PropertyMetadata(true)
    );

    public bool IsCancelButtonVisible
    {
        get => (bool)GetValue(IsCancelButtonVisibleProperty);
        set => SetValue(IsCancelButtonVisibleProperty, value);
    }

    public static readonly DependencyProperty IsFooterVisibleProperty = DependencyProperty.Register(
        nameof(IsFooterVisible),
        typeof(bool),
        typeof(OkCancelWindow),
        new PropertyMetadata(true)
    );

    public bool IsFooterVisible
    {
        get => (bool)GetValue(IsFooterVisibleProperty);
        set => SetValue(IsFooterVisibleProperty, value);
    }

    public static readonly DependencyProperty IsXButtonVisibleProperty = DependencyProperty.Register(
        nameof(IsXButtonVisible),
        typeof(bool),
        typeof(OkCancelWindow),
        new PropertyMetadata(false)
    );

    public bool IsXButtonVisible
    {
        get => (bool)GetValue(IsXButtonVisibleProperty);
        set => SetValue(IsXButtonVisibleProperty, value);
    }

    static OkCancelWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(OkCancelWindow), new FrameworkPropertyMetadata(typeof(OkCancelWindow)));
    }
}