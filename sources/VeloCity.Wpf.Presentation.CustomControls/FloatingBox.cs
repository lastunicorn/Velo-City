// Velo City
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
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls
{
    public class FloatingBox : ContentControl
    {
        private FrameworkElement elementToMove;
        private FrameworkElement sizableContent;
        private Cursor oldCursor;
        private Point currentMousePosition;

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen),
            typeof(bool),
            typeof(FloatingBox),
            new PropertyMetadata(false, HandleIsOpenChanged)
        );

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        private static void HandleIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FloatingBox floatingBox && e.NewValue is bool newValue)
            {
                if (newValue)
                {
                    if (floatingBox.sizableContent != null)
                    {
                        floatingBox.sizableContent.Width = floatingBox.initialWidth;
                        floatingBox.sizableContent.Height = floatingBox.initialHeight;
                    }

                    if (floatingBox.elementToMove != null)
                    {
                        Canvas.SetLeft(floatingBox.elementToMove, floatingBox.initialLeft);
                        Canvas.SetTop(floatingBox.elementToMove, floatingBox.initialTop);

                        //Canvas canvas = floatingBox.elementToMove.FindParent<Canvas>();
                        //double initialLeft = Canvas.GetLeft(floatingBox.elementToMove);

                        //if (double.IsNaN(initialLeft))
                        //{
                        //    initialLeft = (canvas.ActualWidth - floatingBox.ActualWidth - floatingBox.Margin.Left - floatingBox.Margin.Right) / 2;
                        //    Canvas.SetLeft(floatingBox.elementToMove, initialLeft);
                        //}

                        //double initialTop = Canvas.GetTop(floatingBox.elementToMove);

                        //if (double.IsNaN(initialTop))
                        //{
                        //    initialTop = (canvas.ActualHeight - floatingBox.ActualHeight - floatingBox.Margin.Top - floatingBox.Margin.Bottom) / 2;
                        //    Canvas.SetTop(floatingBox.elementToMove, initialTop);
                        //}
                    }

                    floatingBox.Focus();

                    RoutedEventArgs args = new(OpenedEvent, floatingBox);
                    floatingBox.RaiseEvent(args);
                }
                else
                {
                    floatingBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                floatingBox.OnIsOpenChanged();
            }
        }


        public static RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(
            "Opened",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(FloatingBox));

        public event RoutedEventHandler Opened
        {
            add => AddHandler(OpenedEvent, value);
            remove => RemoveHandler(OpenedEvent, value);
        }
        
        public event EventHandler IsOpenChanged;

        protected virtual void OnIsOpenChanged()
        {
            IsOpenChanged?.Invoke(this, EventArgs.Empty);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_HeaderContainer") is Border headerContainer)
            {
                elementToMove = headerContainer.FindParentBefore<Canvas>() as FrameworkElement;
                sizableContent = elementToMove ?? headerContainer.FindParent<FloatingBox>();

                if (elementToMove != null)
                {
                    headerContainer.MouseLeftButtonDown += HandleMouseLeftButtonDown;
                    headerContainer.MouseLeftButtonUp += HandleMouseLeftButtonUp;
                    headerContainer.MouseMove += HandleMouseMove;
                }
            }

            if (GetTemplateChild("PART_ResizeGrip") is Thumb thumb)
            {
                thumb.DragStarted += HandleThumbDragStarted;
                thumb.DragDelta += HandleThumbDragDelta;
                thumb.DragCompleted += HandleThumbDragCompleted;
            }
        }

        private void HandleThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            oldCursor = Cursor;
            Cursor = Cursors.SizeNWSE;
        }

        private void HandleThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            double newHeight = sizableContent.ActualHeight + e.VerticalChange;
            double newWidth = sizableContent.ActualWidth + e.HorizontalChange;

            double minWidth = Math.Max(320, sizableContent.MinWidth);
            double minHeight = Math.Max(240, sizableContent.MinHeight);

            if (newWidth < minWidth)
                newWidth = minWidth;

            if (newHeight < minHeight)
                newHeight = minHeight;

            sizableContent.Width = newWidth;
            sizableContent.Height = newHeight;
        }

        private void HandleThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            Cursor = oldCursor;
        }

        private void HandleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement uiElement && elementToMove != null)
            {
                Canvas canvas = elementToMove.FindParent<Canvas>();
                currentMousePosition = e.GetPosition(canvas);

                uiElement.CaptureMouse();
            }
        }

        private void HandleMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement uiElement && elementToMove != null)
            {
                if (uiElement.IsMouseCaptured)
                {
                    Canvas canvas = elementToMove.FindParent<Canvas>();
                    Point mousePosition = e.GetPosition(canvas);

                    double left = Canvas.GetLeft(elementToMove);
                    Canvas.SetLeft(elementToMove, left + mousePosition.X - currentMousePosition.X);

                    double top = Canvas.GetTop(elementToMove);
                    Canvas.SetTop(elementToMove, top + mousePosition.Y - currentMousePosition.Y);

                    uiElement.ReleaseMouseCapture();
                }
            }
        }

        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is UIElement uiElement && elementToMove != null)
            {
                if (uiElement.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
                {
                    Canvas canvas = elementToMove.FindParent<Canvas>();

                    Point mousePosition = e.GetPosition(canvas);

                    double oldLeft = Canvas.GetLeft(elementToMove);
                    double newLeft = oldLeft + mousePosition.X - currentMousePosition.X;
                    double minLeft = 0;
                    double maxLeft = canvas.ActualWidth - elementToMove.ActualWidth - elementToMove.Margin.Left - elementToMove.Margin.Right;

                    if ((newLeft >= minLeft || newLeft > oldLeft) && (newLeft <= maxLeft || newLeft < oldLeft))
                        Canvas.SetLeft(elementToMove, newLeft);

                    double oldTop = Canvas.GetTop(elementToMove);
                    double newTop = oldTop + mousePosition.Y - currentMousePosition.Y;
                    double minTop = 0;
                    double maxTop = canvas.ActualHeight - elementToMove.ActualHeight - elementToMove.Margin.Top - elementToMove.Margin.Bottom;

                    if ((newTop >= minTop || newTop > oldTop) && (newTop <= maxTop || newTop < oldTop))
                        Canvas.SetTop(elementToMove, newTop);

                    currentMousePosition = mousePosition;
                }
            }
        }

        public FloatingBox()
        {
            Loaded += HandleLoaded;
        }

        private double initialLeft;
        private double initialTop;
        private double initialWidth;
        private double initialHeight;

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            if (elementToMove != null)
            {
                Canvas canvas = elementToMove.FindParent<Canvas>();
                double initialLeft = Canvas.GetLeft(elementToMove);

                if (double.IsNaN(initialLeft))
                {
                    initialLeft = (canvas.ActualWidth - ActualWidth - Margin.Left - Margin.Right) / 2;
                    this.initialLeft = initialLeft;
                }

                double initialTop = Canvas.GetTop(elementToMove);

                if (double.IsNaN(initialTop))
                {
                    initialTop = (canvas.ActualHeight - ActualHeight - Margin.Top - Margin.Bottom) / 2;
                    this.initialTop = initialTop;
                }

                initialWidth = sizableContent.Width;
                initialHeight = sizableContent.Height;
            }
        }
    }
}