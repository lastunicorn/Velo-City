﻿// VeloCity
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
// using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class TeamMemberAbsenceControl : ContentControl
{
    public static readonly DependencyProperty IsPartialVacationProperty = DependencyProperty.Register(
        nameof(IsPartialVacation),
        typeof(bool),
        typeof(TeamMemberAbsenceControl),
        new FrameworkPropertyMetadata(false,
            FrameworkPropertyMetadataOptions.AffectsArrange |
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsParentMeasure));

    public bool IsPartialVacation
    {
        get => (bool)GetValue(IsPartialVacationProperty);
        set => SetValue(IsPartialVacationProperty, value);
    }

    public static readonly DependencyProperty IsMissingByContractProperty = DependencyProperty.Register(
        nameof(IsMissingByContract),
        typeof(bool),
        typeof(TeamMemberAbsenceControl),
        new FrameworkPropertyMetadata(false,
            FrameworkPropertyMetadataOptions.AffectsArrange |
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsParentMeasure));

    public bool IsMissingByContract
    {
        get => (bool)GetValue(IsMissingByContractProperty);
        set => SetValue(IsMissingByContractProperty, value);
    }

    public static readonly DependencyProperty AbsenceHoursProperty = DependencyProperty.Register(
        nameof(AbsenceHours),
        typeof(int),
        typeof(TeamMemberAbsenceControl),
        new FrameworkPropertyMetadata(0,
            FrameworkPropertyMetadataOptions.AffectsArrange |
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsParentMeasure));

    public int AbsenceHours
    {
        get => (int)GetValue(AbsenceHoursProperty);
        set => SetValue(AbsenceHoursProperty, value);
    }

    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(
        nameof(IconForeground),
        typeof(Brush),
        typeof(TeamMemberAbsenceControl)
    );

    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    static TeamMemberAbsenceControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TeamMemberAbsenceControl), new FrameworkPropertyMetadata(typeof(TeamMemberAbsenceControl)));
    }
}