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
using System.Windows.Data;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

internal static class FrameworkElementExtensions
{
    internal static void UpdateSource(this FrameworkElement element, DependencyProperty dependencyProperty)
    {
        BindingExpression bindingExpression = element.GetBindingExpression(dp: dependencyProperty);
        bindingExpression?.UpdateSource();
    }

    internal static void UpdateSource(this DependencyProperty dependencyProperty, FrameworkElement element)
    {
        BindingExpression bindingExpression = element.GetBindingExpression(dp: dependencyProperty);
        bindingExpression?.UpdateSource();
    }

    internal static void UpdateSources(this FrameworkElement element, List<DependencyProperty> dependencyProperties)
    {
        IEnumerable<BindingExpression> bindingExpressions = dependencyProperties.Select(element.GetBindingExpression);

        foreach (BindingExpression bindingExpression in bindingExpressions) 
            bindingExpression?.UpdateSource();
    }

    internal static void UpdateSources(this FrameworkElement element, params DependencyProperty[] dependencyProperties)
    {
        IEnumerable<BindingExpression> bindingExpressions = dependencyProperties.Select(element.GetBindingExpression);

        foreach (BindingExpression bindingExpression in bindingExpressions)
            bindingExpression?.UpdateSource();
    }
}