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
using System.Windows.Data;
using System.Windows.Input;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls.Behaviors
{
    public static class InputBindingsManager
    {
        public static readonly DependencyProperty UpdateSourceOnEnterKeyPressedProperty = DependencyProperty.RegisterAttached(
            "UpdateSourceOnEnterKeyPressed",
            typeof(bool),
            typeof(InputBindingsManager),
            new PropertyMetadata(OnUpdateSourceOnEnterKeyPressedPropertyChanged));

        public static void SetUpdateSourceOnEnterKeyPressed(DependencyObject obj, bool value)
        {
            obj.SetValue(UpdateSourceOnEnterKeyPressedProperty, value);
        }

        public static bool GetUpdateSourceOnEnterKeyPressed(DependencyObject obj)
        {
            return (bool)obj.GetValue(UpdateSourceOnEnterKeyPressedProperty);
        }

        private static void OnUpdateSourceOnEnterKeyPressedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not UIElement element)
                return;

            if (e.OldValue is true)
                element.PreviewKeyDown -= HandlePreviewKeyDown;

            if (e.NewValue is true)
                element.PreviewKeyDown += HandlePreviewKeyDown;
        }

        private static void HandlePreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoUpdateSource(e.Source);
            }
        }

        private static void DoUpdateSource(object source)
        {
            if (source is not UIElement uiElement)
                return;

            DependencyProperty property = TextBox.TextProperty;

            BindingExpression binding = BindingOperations.GetBindingExpression(uiElement, property);
            binding?.UpdateSource();
        }
    }
}