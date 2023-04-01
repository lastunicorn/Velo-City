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

using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles.Behaviors;

public class WindowButtonBehavior
{
    public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.RegisterAttached(
        "IsDefault",
        typeof(bool),
        typeof(WindowButtonBehavior),
        new UIPropertyMetadata(false, HandleIsDefaultChanged));

    public static bool GetIsDefault(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsDefaultProperty);
    }

    public static void SetIsDefault(DependencyObject obj, bool value)
    {
        obj.SetValue(IsDefaultProperty, value);
    }

    private static void HandleIsDefaultChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is Button button)
        {
            if ((bool)e.NewValue)
                button.Click += HandleClick;
            else
                button.Click -= HandleClick;
        }
    }

    private static void HandleClick(object sender, RoutedEventArgs e)
    {
        if (sender is not UIElement uiElement)
            return;

        Window parentWindow = Window.GetWindow(uiElement);

        if (parentWindow != null)
            parentWindow.DialogResult = true;
    }

    public static readonly DependencyProperty IsCloseButtonProperty = DependencyProperty.RegisterAttached(
        "IsCloseButton",
        typeof(bool),
        typeof(WindowButtonBehavior),
        new UIPropertyMetadata(false, HandleIsCloseButtonChanged));

    public static bool GetIsCloseButton(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsCloseButtonProperty);
    }

    public static void SetIsCloseButton(DependencyObject obj, bool value)
    {
        obj.SetValue(IsCloseButtonProperty, value);
    }

    private static void HandleIsCloseButtonChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is Button button)
        {
            if ((bool)e.NewValue)
                button.Click += HandleCloseButtonClick;
            else
                button.Click -= HandleCloseButtonClick;
        }
    }

    private static void HandleCloseButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is not UIElement uiElement)
            return;

        Window parentWindow = Window.GetWindow(uiElement);

        if (parentWindow != null)
            parentWindow.DialogResult = false;
    }
}