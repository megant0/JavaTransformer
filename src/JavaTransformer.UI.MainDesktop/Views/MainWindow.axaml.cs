using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using JavaTransformer.UI.MainDesktop.Core;
using System;

namespace JavaTransformer.UI.MainDesktop.Views
{
    public partial class MainWindow : Window
    {
        WindowManagedCore core;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += WindowLoaded;
        }

        private void WindowLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            core = new WindowManagedCore(this);
          
            core.ApplyModernWindowStyle();
            core.EnableDragMoveFor(headerGrid);
        }
    }
}