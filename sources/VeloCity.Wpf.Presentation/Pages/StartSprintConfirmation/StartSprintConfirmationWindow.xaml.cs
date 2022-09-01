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
using System.Windows.Input;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.StartSprintConfirmation
{
    /// <summary>
    /// Interaction logic for StartSprintConfirmationWindow.xaml
    /// </summary>
    public partial class StartSprintConfirmationWindow : Window
    {
        public StartSprintConfirmationWindow()
        {
            InitializeComponent();
            
            SourceInitialized += (x, y) =>
            {
                this.HideMinimizeAndMaximizeButtons();
            };
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Window parentWindow = Window.GetWindow(this);
                parentWindow?.DragMove();
            }
        }
    }
}