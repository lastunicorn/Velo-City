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
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles.Behaviors
{
    internal static class PopupBehavior
    {
        private static readonly PopupPool Popups = new();

        public static readonly DependencyProperty PopupProperty = DependencyProperty.RegisterAttached(
            "Popup",
            typeof(Popup),
            typeof(PopupBehavior),
            new UIPropertyMetadata(default(Popup), OnPopupPropertyChanged));

        public static Popup GetPopup(DependencyObject d)
        {
            return (Popup)d.GetValue(PopupProperty);
        }

        public static void SetPopup(DependencyObject d, bool value)
        {
            d.SetValue(PopupProperty, value);
        }

        private static void OnPopupPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uiElement)
            {
                if (e.NewValue is Popup popup)
                {
                    Popups.Set(uiElement, popup);
                    popup.PlacementTarget = uiElement;
                    uiElement.MouseLeftButtonDown += UiElementOnMouseLeftButtonDown;
                }
                else
                {
                    uiElement.MouseLeftButtonDown -= UiElementOnMouseLeftButtonDown;
                    Popups.Remove(uiElement);
                }
            }
        }

        private static void UiElementOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement uiElement)
            {
                Popup popup = Popups.Get(uiElement);

                if (popup != null)
                    popup.IsOpen = true;
            }
        }
    }
}