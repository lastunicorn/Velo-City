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

using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class ListItemStyleSelector : StyleSelector
{
    public Style FirstItemStyle { get; set; }

    public Style NormalItemStyle { get; set; }

    public Style LastItemStyle { get; set; }

    public override Style SelectStyle(object item, DependencyObject container)
    {
        ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
        int index = itemsControl.ItemContainerGenerator.IndexFromContainer(container);

        if (index == 0)
            return FirstItemStyle ?? NormalItemStyle ?? base.SelectStyle(item, container);

        if (index == itemsControl.Items.Count - 1)
            return LastItemStyle ?? NormalItemStyle ?? base.SelectStyle(item, container);

        return NormalItemStyle ?? base.SelectStyle(item, container);
    }
}