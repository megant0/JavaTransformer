using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Core
{
    public class WindowManagedCore : IDisposable
    {
        private readonly Window _window;
        private readonly List<Control> _dragElements = new();
        private bool _isDisposed;

        public event EventHandler<PointerPressedEventArgs>? PointerPressed;

        public WindowManagedCore(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            Initialize();
        }

        private void Initialize()
        {
            _window.PointerPressed += OnWindowPointerPressed;
        }

        private void OnWindowPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            PointerPressed?.Invoke(sender, e);
        }

        public void ApplyModernWindowStyle()
        {
            ThrowIfDisposed();

            _window.ExtendClientAreaToDecorationsHint = true;
            _window.ExtendClientAreaTitleBarHeightHint = -1;
            _window.WindowState = WindowState.Normal;
        }

        public void ApplyModernNoBorderStyle()
        {
            ThrowIfDisposed();

            _window.SystemDecorations = SystemDecorations.BorderOnly;

            _window.ExtendClientAreaToDecorationsHint = true;
            _window.ExtendClientAreaTitleBarHeightHint = -1;
            _window.ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;

            _window.WindowState = WindowState.Normal;
        }

        public void EnableDragMoveFor(Control dragElement)
        {
            ThrowIfDisposed();

            if (dragElement == null)
                throw new ArgumentNullException(nameof(dragElement));

            if (!_dragElements.Contains(dragElement))
            {
                dragElement.PointerPressed += OnDragElementPointerPressed;
                _dragElements.Add(dragElement);
            }
        }

        public void DisableDragMoveFor(Control dragElement)
        {
            ThrowIfDisposed();

            if (dragElement == null)
                throw new ArgumentNullException(nameof(dragElement));

            if (_dragElements.Contains(dragElement))
            {
                dragElement.PointerPressed -= OnDragElementPointerPressed;
                _dragElements.Remove(dragElement);

            }
        }
        public void EnableDragMoveForWindow()
        {
            ThrowIfDisposed();
            PointerPressed += OnDragMoveRequested;
        }

        public void EnableTopMost()
        {
            _window.Topmost = true;
        }

        public void DisabledTopMost()
        {
            _window.Topmost = false;
        }

        public void DisableDragMoveForWindow()
        {
            ThrowIfDisposed();
            PointerPressed -= OnDragMoveRequested;
        }

        private void OnDragElementPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(_window).Properties.IsLeftButtonPressed)
            {
                StartDragMove(e);
            }
        }

        private void OnDragMoveRequested(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(_window).Properties.IsLeftButtonPressed)
            {
                StartDragMove(e);
            }
        }

        private void StartDragMove(PointerPressedEventArgs e)
        {
            _window.BeginMoveDrag(e);
        }


        public void CenterOnScreen()
        {
            ThrowIfDisposed();
            _window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public void SetMinSize(double width, double height)
        {
            ThrowIfDisposed();
            _window.MinWidth = width;
            _window.MinHeight = height;
        }

        public void SetMaxSize(double width, double height)
        {
            ThrowIfDisposed();
            _window.MaxWidth = width;
            _window.MaxHeight = height;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(WindowManagedCore));
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _window.PointerPressed -= OnWindowPointerPressed;
            PointerPressed -= OnDragMoveRequested;

            foreach (var element in _dragElements)
            {
                element.PointerPressed -= OnDragElementPointerPressed;
                element.Cursor = Cursor.Default;
            }
            _dragElements.Clear();

            PointerPressed = null;

            _isDisposed = true;
            GC.SuppressFinalize(this);
        }

    }
}
