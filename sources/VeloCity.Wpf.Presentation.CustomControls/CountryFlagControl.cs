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

using System;
using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls
{
    public class CountryFlagControl : Control
    {
        public static readonly DependencyProperty CountryCodeProperty = DependencyProperty.Register(
            nameof(CountryCode),
            typeof(string),
            typeof(CountryFlagControl),
            new PropertyMetadata(null, CountryCodeChangedCallback)
        );

        private static void CountryCodeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CountryFlagControl countryFlagControl)
            {
                if (e.NewValue is string)
                {
                    string resourceName = "CountryFlag_" + e.NewValue;

                    ResourceDictionary resourceDictionary = new()
                    {
                        Source = new Uri("Pack://application:,,,/DustInTheWind.VeloCity.Wpf.Presentation.Styles;component/Themes/CustomControls/CountryFlagControlStyles.xaml", UriKind.RelativeOrAbsolute)
                    };

                    object resource = resourceDictionary[resourceName];
                    countryFlagControl.FlagTemplate = resource as ControlTemplate;
                }
                else
                {
                    countryFlagControl.Template = null;
                }
            }
        }

        public string CountryCode
        {
            get => (string)GetValue(CountryCodeProperty);
            set => SetValue(CountryCodeProperty, value);
        }

        private static readonly DependencyPropertyKey FlagTemplatePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(FlagTemplate),
            typeof(ControlTemplate),
            typeof(CountryFlagControl),
            new FrameworkPropertyMetadata(null, flags: FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public static readonly DependencyProperty FlagTemplateProperty = FlagTemplatePropertyKey.DependencyProperty;

        public ControlTemplate FlagTemplate
        {
            get => (ControlTemplate)GetValue(FlagTemplateProperty);
            private set => SetValue(FlagTemplatePropertyKey, value);
        }

        static CountryFlagControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CountryFlagControl), new FrameworkPropertyMetadata(typeof(CountryFlagControl)));
        }
    }
}