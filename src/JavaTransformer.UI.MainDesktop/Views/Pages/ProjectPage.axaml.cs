#pragma warning disable CS8601 

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.TextMate;
using JavaTransformer.UI.MainDesktop.Models;
using JavaTransformer.UI.MainDesktop.Models.Common.DataTransferObject.Settings;
using JavaTransformer.UI.MainDesktop.Models.Extension;
using JavaTransformer.UI.MainDesktop.Models.Files;
using JavaTransformer.UI.MainDesktop.Models.Services;
using JavaTransformer.UI.MainDesktop.ViewModels;
using JavaTransformer.UI.MainDesktop.Views.UtilWindow.MessageBoxs;
using System.Collections.Generic;
using System.Threading.Tasks;
using TextMateSharp.Grammars;
using static System.Net.Mime.MediaTypeNames;

namespace JavaTransformer.UI.MainDesktop;

public partial class ProjectPage : UserControl
{
    private ProjectPageViewModel ViewModel => DataContext as ProjectPageViewModel;
    private string _path;
    private string _currentOpenFile { get; set; } = "empty";

    private EditorSettings _editorSettings;

    private ILoggerService _loggerService;

    public ProjectPage(EditorSettings configuration, ILoggerService loggerService) : this(configuration, loggerService, "") 
    { }

    public ProjectPage(EditorSettings configuration, ILoggerService loggerService, string path)
    {
        _path = path;
        _editorSettings = configuration;

        InitializeComponent();
        DataContext = new ProjectPageViewModel(loggerService);

        Loaded += ProjectPage_Loaded;
        AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.S && e.KeyModifiers == KeyModifiers.Control)
        {
            e.Handled = true; 
            ViewModel.SaveFile(_textEditor, _currentOpenFile);
        }
    }

    private void ProjectPage_Loaded(object? sender, RoutedEventArgs e)
    {
        ViewModel.Loaded(this, e);
        ViewModel.PathProject = _path;
        ViewModel.LoadDirectories(_path);

        SetTextEditorOptions();
    }

    private void SetTextEditorOptions()
    {
        _textEditor.LoadSettings(_editorSettings);

        _textEditor.HorizontalScrollBarVisibility =   Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        _textEditor.Background = Brushes.Transparent;
        _textEditor.Options.ColumnRulerPositions = new List<int>() { 80, 100 };
        _textEditor.TextArea.IndentationStrategy = new AvaloniaEdit.Indentation.CSharp.CSharpIndentationStrategy(_textEditor.Options);
        _textEditor.Options.CompletionAcceptAction = CompletionAcceptAction.DoubleTapped;    
    }

    private async void MenuItem_Click(object? sender, RoutedEventArgs e)
    {
        await ViewModel.RenameSelectedFileAsync();
    }

    private async void MenuItem_Click_1(object? sender, RoutedEventArgs e)
    {
        await ViewModel.DeleteSelectedFileAsync();
    }

    private async void TreeView_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        FileSystemNode? node = _fileTreeView.SelectedItem as FileSystemNode;
        await ViewModel.OpenFile(_textEditor, node?.Path);

        if (FileTask.IsFile(node.Path))
        {
            _currentOpenFile = node.Path;
            _stateText.Text = node.Name;
        }
    }
}
#pragma warning restore CS8601