using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.TextMate;
using JavaTransformer.UI.MainDesktop.Core;
using JavaTransformer.UI.MainDesktop.ViewModels;
using Microsoft.Win32;
using System;
using System.Linq;
using TextMateSharp.Grammars;

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
            core.EnableDragMoveForWindow();

            var _registryOptions = new TextMateSharp.Grammars.RegistryOptions(ThemeName.DarkPlus);

            var _textMateInstallation = _textEditor.InstallTextMate(_registryOptions);

            _textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension(".cpp").Id));
        }


    }
}