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
using System.Windows.Input;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public class EditableTextBlock : Control
{
    #region Text

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(EditableTextBlock),
        new PropertyMetadata(HandleTextChanged));

    private static void HandleTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is EditableTextBlock editableTextBlock) 
            editableTextBlock.IsInEditMode = false;
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    #endregion

    #region IsInEditMode

    public static readonly DependencyProperty IsInEditModeProperty = DependencyProperty.Register(
        nameof(IsInEditMode),
        typeof(bool),
        typeof(EditableTextBlock),
        new PropertyMetadata(false));

    public bool IsInEditMode
    {
        get => (bool)GetValue(IsInEditModeProperty);
        set => SetValue(IsInEditModeProperty, value);
    }

    #endregion

    public override void OnApplyTemplate()
    {
        if (GetTemplateChild("PART_EditIcon") is FrameworkElement editIcon)
        {
            editIcon.MouseDown += (sender, args) =>
            {
                StartEditMode();
            };
        }

        if (GetTemplateChild("PART_EditBox") is TextBox textBox)
        {
            textBox.LostFocus += (sender, args) =>
            {
                //CommitChanges();
            };

            textBox.KeyDown += (sender, args) =>
            {
                switch (args.Key)
                {
                    case Key.Enter:
                        CommitChanges();
                        args.Handled = true;
                        break;

                    case Key.Escape:
                        RevertChanges();
                        args.Handled = true;
                        break;
                }
            };

            textBox.IsVisibleChanged += (sender, args) =>
            {
                if (args.NewValue is true)
                    textBox.Focus();
            };
        }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.MiddleButton == MouseButtonState.Pressed || e.ClickCount == 2)
            StartEditMode();

        base.OnMouseDown(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key is Key.Space or Key.Enter)
        {
            StartEditMode();
            e.Handled = true;
        }

        base.OnKeyDown(e);
    }

    private void StartEditMode()
    {
        if (!IsInEditMode)
            IsInEditMode = true;
    }

    private void CommitChanges()
    {
        if (GetTemplateChild("PART_EditBox") is TextBox textBox)
            textBox.UpdateSource(TextBox.TextProperty);

        IsInEditMode = false;
    }

    private void RevertChanges()
    {
        IsInEditMode = false;
    }

    static EditableTextBlock()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableTextBlock), new FrameworkPropertyMetadata(typeof(EditableTextBlock)));
    }
}