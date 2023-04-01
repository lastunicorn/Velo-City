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

public class CalendarDate : Control
{
    #region Value

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(DateTime),
        typeof(CalendarDate)
    );

    public DateTime Value
    {
        get => (DateTime)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    #endregion

    #region ShowWeekDay

    public static readonly DependencyProperty ShowWeekDayProperty = DependencyProperty.Register(
        nameof(ShowWeekDay),
        typeof(bool),
        typeof(CalendarDate)
    );

    public bool ShowWeekDay
    {
        get => (bool)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    #endregion

    #region MonthFontSize

    public static readonly DependencyProperty MonthFontSizeProperty = DependencyProperty.Register(
        nameof(MonthFontSize),
        typeof(double),
        typeof(CalendarDate)
    );

    public double MonthFontSize
    {
        get => (double)GetValue(MonthFontSizeProperty);
        set => SetValue(MonthFontSizeProperty, value);
    }

    #endregion

    #region WeekFontSize

    public static readonly DependencyProperty WeekFontSizeProperty = DependencyProperty.Register(
        nameof(WeekFontSize),
        typeof(double),
        typeof(CalendarDate)
    );

    public double WeekFontSize
    {
        get => (double)GetValue(WeekFontSizeProperty);
        set => SetValue(WeekFontSizeProperty, value);
    }

    #endregion

    static CalendarDate()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarDate), new FrameworkPropertyMetadata(typeof(CalendarDate)));
    }
}