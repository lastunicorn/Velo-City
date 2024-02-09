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
using DustInTheWind.VeloCity.ChartTools;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class ChartBar : Control
{
    #region Value

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(IChartBarValue),
        typeof(ChartBar)
    );

    public IChartBarValue Value
    {
        get => (IChartBarValue)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    #endregion

    #region Orientation

    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(ChartBar)
    );

    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    #endregion

    static ChartBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartBar), new FrameworkPropertyMetadata(typeof(ChartBar)));
    }
}