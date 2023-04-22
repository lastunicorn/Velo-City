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

using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles.Converters;

[Localizability(LocalizationCategory.NeverLocalize)]
public sealed class FloatToStringRoundDownConverter : IValueConverter
{
    public ushort Decimals { get; set; } = 2;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not float floatValue)
            return value?.ToString();

        int a = (int)Math.Pow(10, Decimals);

        decimal fractionalPart = (decimal)floatValue % 1;
        decimal fractionalPartAsFullNumber = decimal.Truncate(fractionalPart * a);
        decimal fractionalPartAgain = fractionalPartAsFullNumber / a;
        return fractionalPartAgain.ToString(".00");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}