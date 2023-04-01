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

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class StoryPointsBox : Control
{
    #region Value

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(float?),
        typeof(StoryPointsBox),
        new PropertyMetadata(null, HandleValueChanged)
    );

    private static void HandleValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is StoryPointsBox storyPointsBox)
        {
            if (e.NewValue is float?)
            {
                float? newValue = (float?)e.NewValue;
                storyPointsBox.IsNull = newValue == null;
                storyPointsBox.WholePart = (int)Math.Truncate(newValue.Value);
                storyPointsBox.FractionalPart = newValue.Value % 1;
            }
            else if (e.NewValue is float newValue)
            {
                storyPointsBox.IsNull = false;
                storyPointsBox.WholePart = (int)Math.Truncate(newValue);
                storyPointsBox.FractionalPart = newValue % 1;
            }
            else
            {
                storyPointsBox.IsNull = true;
                storyPointsBox.WholePart = 0;
                storyPointsBox.FractionalPart = 0;
            }
        }
    }

    public float? Value
    {
        get => (float?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    #endregion

    #region IsNull

    private static readonly DependencyPropertyKey IsNullPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(IsNull),
        typeof(bool),
        typeof(StoryPointsBox),
        new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
    );

    public static readonly DependencyProperty IsNullProperty = IsNullPropertyKey.DependencyProperty;

    public bool IsNull
    {
        get => (bool)GetValue(IsNullProperty);
        private set => SetValue(IsNullPropertyKey, value);
    }

    #endregion

    #region WholePart

    private static readonly DependencyPropertyKey WholePartPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(WholePart),
        typeof(int),
        typeof(StoryPointsBox),
        new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
    );

    public static readonly DependencyProperty WholePartProperty = WholePartPropertyKey.DependencyProperty;

    public int WholePart
    {
        get => (int)GetValue(WholePartProperty);
        private set => SetValue(WholePartPropertyKey, value);
    }

    #endregion

    #region FractionalPart

    private static readonly DependencyPropertyKey FractionalPartPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(FractionalPart),
        typeof(float),
        typeof(StoryPointsBox),
        new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender)
    );

    public static readonly DependencyProperty FractionalPartProperty = FractionalPartPropertyKey.DependencyProperty;

    public float FractionalPart
    {
        get => (float)GetValue(FractionalPartProperty);
        private set => SetValue(FractionalPartPropertyKey, value);
    }

    #endregion

    static StoryPointsBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StoryPointsBox), new FrameworkPropertyMetadata(typeof(StoryPointsBox)));
    }
}