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
// using System;

using System.Windows;
using System.Windows.Input;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles.Behaviors;

public class WindowBehavior
{
    public static readonly DependencyProperty IsWindowMoverProperty = DependencyProperty.RegisterAttached(
        "IsWindowMover",
        typeof(bool),
        typeof(WindowBehavior),
        new UIPropertyMetadata(false, HandleIsWindowMoverChanged));

    public static bool GetIsWindowMover(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsWindowMoverProperty);
    }

    public static void SetIsWindowMover(DependencyObject obj, bool value)
    {
        obj.SetValue(IsWindowMoverProperty, value);
    }

    private static void HandleIsWindowMoverChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is UIElement uiElement)
        {
            if ((bool)e.NewValue)
                uiElement.MouseDown += HandleMouseDown;
            else
                uiElement.MouseDown -= HandleMouseDown;
        }
    }

    private static void HandleMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not UIElement uiElement)
            return;

        if (e.ChangedButton != MouseButton.Left)
            return;

        Window parentWindow = Window.GetWindow(uiElement);
        parentWindow?.DragMove();
    }

    public static readonly DependencyProperty IsWindowMaximizerProperty = DependencyProperty.RegisterAttached(
        "IsWindowMaximizer",
        typeof(bool),
        typeof(WindowBehavior),
        new UIPropertyMetadata(false, HandleIsWindowMaximizerChanged));

    public static bool GetIsWindowMaximizer(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsWindowMaximizerProperty);
    }

    public static void SetIsWindowMaximizer(DependencyObject obj, bool value)
    {
        obj.SetValue(IsWindowMaximizerProperty, value);
    }

    private static void HandleIsWindowMaximizerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is UIElement uiElement)
        {
            if ((bool)e.NewValue)
                uiElement.MouseDown += HandleWindowMaximizerMouseDown;
            else
                uiElement.MouseDown -= HandleWindowMaximizerMouseDown;
        }
    }

    private static void HandleWindowMaximizerMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not UIElement uiElement)
            return;

        if (e.ChangedButton != MouseButton.Left)
            return;

        if (e.ClickCount != 2)
            return;

        Window parentWindow = Window.GetWindow(uiElement);

        if (parentWindow == null)
            return;

        parentWindow.WindowState = parentWindow.WindowState switch
        {
            WindowState.Normal => WindowState.Maximized,
            WindowState.Minimized => WindowState.Maximized,
            WindowState.Maximized => WindowState.Normal,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}