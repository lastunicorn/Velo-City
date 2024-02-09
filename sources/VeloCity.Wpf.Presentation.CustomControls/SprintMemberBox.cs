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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class SprintMemberBox : Control
{
    #region MemberName

    public static readonly DependencyProperty MemberNameProperty = DependencyProperty.Register(
        nameof(MemberName),
        typeof(string),
        typeof(SprintMemberBox)
    );

    public string MemberName
    {
        get => (string)GetValue(MemberNameProperty);
        set => SetValue(MemberNameProperty, value);
    }

    #endregion

    #region ChartBarValue

    public static readonly DependencyProperty ChartBarValueProperty = DependencyProperty.Register(
        nameof(ChartBarValue),
        typeof(IChartBarValue),
        typeof(SprintMemberBox)
    );

    public IChartBarValue ChartBarValue
    {
        get => (IChartBarValue)GetValue(ChartBarValueProperty);
        set => SetValue(ChartBarValueProperty, value);
    }

    #endregion

    #region WorkHours

    public static readonly DependencyProperty WorkHoursProperty = DependencyProperty.Register(
        nameof(WorkHours),
        typeof(HoursValue),
        typeof(SprintMemberBox)
    );

    public HoursValue WorkHours
    {
        get => (HoursValue)GetValue(WorkHoursProperty);
        set => SetValue(WorkHoursProperty, value);
    }

    #endregion

    #region AbsenceHours

    public static readonly DependencyProperty AbsenceHoursProperty = DependencyProperty.Register(
        nameof(AbsenceHours),
        typeof(HoursValue),
        typeof(SprintMemberBox)
    );

    public HoursValue AbsenceHours
    {
        get => (HoursValue)GetValue(AbsenceHoursProperty);
        set => SetValue(AbsenceHoursProperty, value);
    }

    #endregion

    static SprintMemberBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SprintMemberBox), new FrameworkPropertyMetadata(typeof(SprintMemberBox)));
    }
}