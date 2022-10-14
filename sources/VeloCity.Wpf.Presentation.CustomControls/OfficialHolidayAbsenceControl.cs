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
// using System;

using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls
{
    public class OfficialHolidayAbsenceControl : ContentControl
    {
        public static readonly DependencyProperty HolidayNameProperty = DependencyProperty.Register(
            nameof(HolidayName),
            typeof(string),
            typeof(OfficialHolidayAbsenceControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsArrange |
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public string HolidayName
        {
            get => (string)GetValue(HolidayNameProperty);
            set => SetValue(HolidayNameProperty, value);
        }

        public static readonly DependencyProperty HolidayCountryProperty = DependencyProperty.Register(
            nameof(HolidayCountry),
            typeof(string),
            typeof(OfficialHolidayAbsenceControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsArrange |
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public string HolidayCountry
        {
            get => (string)GetValue(HolidayCountryProperty);
            set => SetValue(HolidayCountryProperty, value);
        }

        static OfficialHolidayAbsenceControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OfficialHolidayAbsenceControl), new FrameworkPropertyMetadata(typeof(OfficialHolidayAbsenceControl)));
        }
    }
}