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

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles.Behaviors;

public static class DataGridSelectionBehavior
{
    public static readonly DependencyProperty EnableSelectionProperty = DependencyProperty.RegisterAttached(
        "EnableSelection",
        typeof(bool),
        typeof(DataGridSelectionBehavior),
        new UIPropertyMetadata(true, OnEnableSelectionPropertyChangedCallback));

    public static bool GetEnableSelection(DependencyObject d)
    {
        return (bool)d.GetValue(EnableSelectionProperty);
    }

    public static void SetEnableSelection(DependencyObject d, bool value)
    {
        d.SetValue(EnableSelectionProperty, value);
    }

    private static void OnEnableSelectionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGrid dataGrid)
        {
            if (e.NewValue is false)
            {
                dataGrid.SelectionMode = DataGridSelectionMode.Single;
                dataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
                dataGrid.SelectedCellsChanged += HandleSelectedCellsChanged;
            }
            else
            {
                dataGrid.SelectedCellsChanged -= HandleSelectedCellsChanged;
            }
        }
    }

    private static void HandleSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
        if (sender is DataGrid dataGrid)
            dataGrid.UnselectAllCells();
    }
}