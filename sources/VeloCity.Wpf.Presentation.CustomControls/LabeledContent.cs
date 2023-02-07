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

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls
{
    public class LabeledContent : ContentControl
    {
        #region Label

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            nameof(Label),
            typeof(object),
            typeof(LabeledContent),
            new PropertyMetadata(null)
        );

        public object Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        #endregion

        #region InfoContent

        public static readonly DependencyProperty InfoContentProperty = DependencyProperty.Register(
            nameof(InfoContent),
            typeof(object),
            typeof(LabeledContent),
            new PropertyMetadata(null)
        );

        public object InfoContent
        {
            get => GetValue(InfoContentProperty);
            set => SetValue(InfoContentProperty, value);
        }

        #endregion

        #region BetweenSpace

        public static readonly DependencyProperty BetweenSpaceProperty = DependencyProperty.Register(
            nameof(BetweenSpace),
            typeof(GridLength),
            typeof(LabeledContent),
            new PropertyMetadata(new GridLength(20))
        );

        public GridLength BetweenSpace
        {
            get => (GridLength)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        #endregion

        static LabeledContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabeledContent), new FrameworkPropertyMetadata(typeof(LabeledContent)));
        }
    }
}