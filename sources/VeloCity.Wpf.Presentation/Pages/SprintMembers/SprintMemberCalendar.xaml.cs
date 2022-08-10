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
using System.Windows.Input;
using System.Windows.Media;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintMembers
{
    /// <summary>
    /// Interaction logic for SprintMemberCalendar.xaml
    /// </summary>
    public partial class SprintMemberCalendar : UserControl
    {
        public SprintMemberCalendar()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MouseWheelEventArgs eventArg = new(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = e.Source
            };

            DependencyObject uiElement = (DependencyObject)sender;
            ScrollViewer parent = FindAncestor<ScrollViewer>(uiElement);

            if (parent != null)
            {
                parent.RaiseEvent(eventArg);
                e.Handled = true;
            }
        }

        public static T FindAncestor<T>(DependencyObject obj)
            where T : DependencyObject
        {
            if (obj != null)
            {
                DependencyObject dependObj = obj;
                do
                {
                    dependObj = GetParent(dependObj);

                    if (dependObj is T dependencyObject)
                        return dependencyObject;
                }
                while (dependObj != null);
            }

            return null;
        }

        public static DependencyObject GetParent(DependencyObject obj)
        {
            if (obj == null)
                return null;

            if (obj is ContentElement element)
            {
                DependencyObject parent = ContentOperations.GetParent(element);

                if (parent != null)
                    return parent;

                if (element is FrameworkContentElement contentElement)
                    return contentElement.Parent;

                return null;
            }

            return VisualTreeHelper.GetParent(obj);
        }
    }
}