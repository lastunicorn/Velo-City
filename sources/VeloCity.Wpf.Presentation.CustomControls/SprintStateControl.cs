﻿// Velo City
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

using System;
using System.Windows;
using System.Windows.Controls;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls
{
    public enum SprintState
    {
        Unknown = 0,
        New,
        InProgress,
        Closed
    }

    public class SprintStateControl : Control
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(SprintState),
            typeof(SprintStateControl),
            new PropertyMetadata(SprintState.Unknown)
        );

        public SprintState Value
        {
            get => (SprintState)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty IsIconVisibleProperty = DependencyProperty.Register(
            nameof(IsIconVisible),
            typeof(bool),
            typeof(SprintStateControl),
            new PropertyMetadata(true)
        );

        public bool IsIconVisible
        {
            get => (bool)GetValue(IsIconVisibleProperty);
            set => SetValue(IsIconVisibleProperty, value);
        }

        public static readonly DependencyProperty IsLabelVisibleProperty = DependencyProperty.Register(
            nameof(IsLabelVisible),
            typeof(bool),
            typeof(SprintStateControl),
            new PropertyMetadata(true)
        );

        public bool IsLabelVisible
        {
            get => (bool)GetValue(IsLabelVisibleProperty);
            set => SetValue(IsLabelVisibleProperty, value);
        }

        public static readonly DependencyProperty IsIconShadowVisibleProperty = DependencyProperty.Register(
            nameof(IsIconShadowVisible),
            typeof(bool),
            typeof(SprintStateControl),
            new PropertyMetadata(false)
        );

        public bool IsIconShadowVisible
        {
            get => (bool)GetValue(IsIconShadowVisibleProperty);
            set => SetValue(IsIconShadowVisibleProperty, value);
        }

        static SprintStateControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SprintStateControl), new FrameworkPropertyMetadata(typeof(SprintStateControl)));
        }
    }
}