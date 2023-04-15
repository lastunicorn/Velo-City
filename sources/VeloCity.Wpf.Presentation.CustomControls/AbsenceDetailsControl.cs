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

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class AbsenceDetailsControl : ItemsControl
{
    #region OfficialHolidays

    public static readonly DependencyProperty OfficialHolidaysProperty = DependencyProperty.Register(
        nameof(OfficialHolidays),
        typeof(IEnumerable),
        typeof(AbsenceDetailsControl),
        new PropertyMetadata(HandleOfficialHolidaysChanged)
    );

    public IEnumerable OfficialHolidays
    {
        get => (IEnumerable)GetValue(OfficialHolidaysProperty);
        set => SetValue(OfficialHolidaysProperty, value);
    }

    private static void HandleOfficialHolidaysChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AbsenceDetailsControl absenceDetailsControl)
        {
            if (e.OldValue is INotifyCollectionChanged oldItems and IEnumerable enumerableOldItems)
            {
                oldItems.CollectionChanged += (sender, e1) =>
                {
                    absenceDetailsControl.HasOfficialHolidays = enumerableOldItems.GetEnumerator().MoveNext();
                };

                absenceDetailsControl.HasOfficialHolidays = enumerableOldItems.GetEnumerator().MoveNext();
            }

            if (e.NewValue is INotifyCollectionChanged newItems and IEnumerable enumerableNewItems)
            {
                newItems.CollectionChanged += (sender, e1) =>
                {
                    absenceDetailsControl.HasOfficialHolidays = enumerableNewItems.GetEnumerator().MoveNext();
                };

                absenceDetailsControl.HasOfficialHolidays = enumerableNewItems.GetEnumerator().MoveNext();
            }
        }
    }

    #endregion

    #region HasOfficialHolidays

    private static readonly DependencyPropertyKey HasOfficialHolidaysPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(HasOfficialHolidays),
        typeof(bool),
        typeof(AbsenceDetailsControl),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
    );

    public static readonly DependencyProperty HasOfficialHolidaysProperty = HasOfficialHolidaysPropertyKey.DependencyProperty;

    public bool HasOfficialHolidays
    {
        get => (bool)GetValue(HasOfficialHolidaysProperty);
        private set => SetValue(HasOfficialHolidaysPropertyKey, value);
    }

    #endregion

    #region OfficialHolidayItemTemplate

    public static readonly DependencyProperty OfficialHolidayItemTemplateProperty = DependencyProperty.Register(
        nameof(OfficialHolidayItemTemplate),
        typeof(DataTemplate),
        typeof(AbsenceDetailsControl),
        new FrameworkPropertyMetadata(null, OnOfficialHolidayItemTemplateChanged));

    [Bindable(true)]
    public DataTemplate OfficialHolidayItemTemplate
    {
        get => (DataTemplate)GetValue(OfficialHolidayItemTemplateProperty);
        set => SetValue(OfficialHolidayItemTemplateProperty, value);
    }

    private static void OnOfficialHolidayItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is AbsenceDetailsControl absenceDetailsControl)
        {
            absenceDetailsControl.OnOfficialHolidayItemTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }
    }

    #endregion

    #region Text

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(AbsenceDetailsControl)
    );

    public string Text
    {
        get => (string)GetValue(OfficialHolidaysProperty);
        set => SetValue(OfficialHolidaysProperty, value);
    }

    #endregion

    protected virtual void OnOfficialHolidayItemTemplateChanged(DataTemplate oldItemTemplate, DataTemplate newItemTemplate)
    {
    }

    static AbsenceDetailsControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(AbsenceDetailsControl), new FrameworkPropertyMetadata(typeof(AbsenceDetailsControl)));
    }
}