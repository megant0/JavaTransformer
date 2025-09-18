using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JavaTransformer.UI.MainDesktop.Models;
using JavaTransformer.UI.MainDesktop.Models.Common.DataTransferObject.Settings;
using JavaTransformer.UI.MainDesktop.Models.Core;
using JavaTransformer.UI.MainDesktop.Models.Exceptions;
using JavaTransformer.UI.MainDesktop.Models.Extension;
using JavaTransformer.UI.MainDesktop.Models.Files;
using JavaTransformer.UI.MainDesktop.Models.Services;
using JavaTransformer.UI.MainDesktop.Views;
using JavaTransformer.UI.MainDesktop.Views.UtilWindow.MessageBoxs;
using JavaTransformer.UI.MainDesktop.Views.UtilWindow.RenameFileWindows;
using System;

using Sys = System;

namespace JavaTransformer.UI.MainDesktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelProgram, OnLoadedService
    {
        private static MainWindowViewModel _instance;
        public static MainWindowViewModel Instance { get { return _instance; } }

        private WindowManagedCore core;

        private Grid _mainGrid;
        private Grid _headerGrid;
        private Menu _headerMenu;
        private Window _mainWindow;
        private bool _isLoaded = false;
        private IConfigurationService _configurationService;
        private ILoggerService _loggerService;

        [ObservableProperty]
        private Control _currentPage;

        public Window MainWindow { get { return _mainWindow; } }

        public MainWindowViewModel(IConfigurationService configurationService, ILoggerService loggerService)
        {
            _configurationService = configurationService;
            _loggerService = loggerService;

            ExceptionInvoker.ExceptionOccurred += ExceptionInvoker_ExceptionOccurred;
        }
        private void ExceptionInvoker_ExceptionOccurred(object? sender, Exception e)
        {
            _loggerService.Error($"[Exception Invoker] {e.ToString()}");
        }

        private void ThrowIsNotLoaded()
        {
            if (!_isLoaded) throw new Exception("Not Setup");
        }

        public void Setup(Grid mainGrid, Grid headerGrid, Menu headerMenu)
        {
            _isLoaded = true;
            
            _mainGrid   = mainGrid;
            _headerGrid = headerGrid;
            _headerMenu = headerMenu;
        }

        public void Loaded(object sender, RoutedEventArgs args)
        {
            ThrowIsNotLoaded();
            
            _instance = this;
            _mainWindow = sender as Window ?? throw new Exception("unknown sender");

            core = new WindowManagedCore(_mainWindow);
            core.ApplyModernWindowStyle();
            core.EnableDragMoveFor(_headerGrid);

            var service = ServiceLocator.GetService<IConfigurationService>();
            var page = new ProjectPage(service.GetSection<EditorSettings>(nameof(EditorSettings)), _loggerService, "JavaTransformer");
            CurrentPage = page;

            _loggerService.Info("Initialize Program");
            _loggerService.SaveAsync();
        }
    }
}
