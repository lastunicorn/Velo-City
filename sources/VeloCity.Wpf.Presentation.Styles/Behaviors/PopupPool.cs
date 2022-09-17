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

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles.Behaviors
{
    internal class PopupPool
    {
        private readonly Dictionary<UIElement, Popup> popups = new();

        public Popup Get(UIElement uiElement)
        {
            lock (popups)
            {
                return popups.ContainsKey(uiElement)
                    ? popups[uiElement]
                    : null;
            }
        }

        public void Set(UIElement uiElement, Popup popup)
        {
            lock (popups)
            {
                if (popups.ContainsKey(uiElement))
                    popups[uiElement] = popup;
                else
                    popups.Add(uiElement, popup);
            }
        }

        public void Remove(UIElement uiElement)
        {
            lock (popups)
            {
                if (popups.ContainsKey(uiElement))
                    popups.Remove(uiElement);
            }
        }
    }
}