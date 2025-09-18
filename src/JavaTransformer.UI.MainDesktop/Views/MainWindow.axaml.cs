#pragma warning disable CS8622 
using Avalonia.Controls;
using JavaTransformer.UI.MainDesktop.ViewModels;
using System;

namespace JavaTransformer.UI.MainDesktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is not MainWindowViewModel model) 
                throw new InvalidOperationException("DataContext must be MainWindowViewModel");

            model.Setup(_mainGrid, _headerGrid, _headerMenu);
            model.Loaded(this, e);
        }
    }
}
#pragma warning restore CS8622 
