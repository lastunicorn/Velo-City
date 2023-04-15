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

public class OkCancelPopup : FloatingBox
{
    private readonly ManualResetEventSlim manualResetEventSlim = new(false);

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(OkCancelPopup)
    );

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly DependencyProperty TitleIconProperty = DependencyProperty.Register(
        nameof(TitleIcon),
        typeof(object),
        typeof(OkCancelPopup)
    );

    public object TitleIcon
    {
        get => GetValue(TitleIconProperty);
        set => SetValue(TitleIconProperty, value);
    }

    public bool? Result { get; private set; }

    public event EventHandler Closed;

    protected override void OnIsOpenChanged()
    {
        base.OnIsOpenChanged();

        if (IsOpen)
            Result = null;
        else
            OnClosed();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        KeyDown += HandlePreviewKeyDown;

        if (GetTemplateChild("PART_AcceptButton") is Button acceptButton)
            acceptButton.Click += HandleAcceptButtonClick;

        if (GetTemplateChild("PART_CancelButton") is Button cancelButton)
            cancelButton.Click += HandleCancelButtonClick;
    }

    private void HandlePreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (!IsOpen)
            return;
        switch (e.Key)
        {
            case Key.Enter:

                CloseWithAccept();
                break;

            case Key.Escape:

                CloseWithCancel();
                break;
        }
    }

    private void HandleAcceptButtonClick(object sender, RoutedEventArgs e)
    {
        CloseWithAccept();
    }

    private void HandleCancelButtonClick(object sender, RoutedEventArgs e)
    {
        CloseWithCancel();
    }

    private void CloseWithAccept()
    {
        Result = true;
        IsOpen = false;
        manualResetEventSlim.Set();
    }

    private void CloseWithCancel()
    {
        Result = false;
        IsOpen = false;
        manualResetEventSlim.Set();
    }

    public async Task<bool?> Show()
    {
        manualResetEventSlim.Reset();
        IsOpen = true;

        await manualResetEventSlim.WaitHandle.ToTask();

        return Result;
    }

    public bool? ShowDialog()
    {
        manualResetEventSlim.Reset();
        IsOpen = true;
        manualResetEventSlim.WaitHandle.WaitOne();

        return Result;
    }

    static OkCancelPopup()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(OkCancelPopup), new FrameworkPropertyMetadata(typeof(OkCancelPopup)));
    }

    protected virtual void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}