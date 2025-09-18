using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvaloniaEdit;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.TextMate;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JavaTransformer.UI.MainDesktop.Models.Exceptions;
using JavaTransformer.UI.MainDesktop.Models.Files;
using JavaTransformer.UI.MainDesktop.Models.Services;
using JavaTransformer.UI.MainDesktop.Views.UtilWindow.MessageBoxs;
using JavaTransformer.UI.MainDesktop.Views.UtilWindow.RenameFileWindows;
using JavaTransformer.UI.MainDesktop.Views.UtilWindow.RenameWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using TextMateSharp.Grammars;

namespace JavaTransformer.UI.MainDesktop.ViewModels
{
    public partial class ProjectPageViewModel : ViewModelProgram, OnLoadedService
    {
        [ObservableProperty]
        private string _pathProject;

        [ObservableProperty]
        private FileSystemNode _selectedNode;

        [ObservableProperty]
        private ObservableCollection<FileSystemNode> _roots;

        private ILoggerService _loggerService;

        public ProjectPageViewModel(ILoggerService loggerService)
        {
            Roots = new ObservableCollection<FileSystemNode>();
            _loggerService = loggerService;
        }

        public void Loaded(object sender, RoutedEventArgs args) { }

        public void LoadDirectories(string path)
        {
            if (!Directory.Exists(path))
                return;

            LoadDirectoryRecursive(Roots, path);
        }

        public async Task OpenFile(TextEditor _textEditor, string path)
        {
            if (!FileTask.IsFile(path)) return;

            var task = FileTask.ReadAllTextAsync(path);

            await task;

            var result = task.Result;
            _textEditor.Document = new AvaloniaEdit.Document.TextDocument(result);
        }

        public void SetLanguageSyntaxByExt(TextEditor text, RegistryOptions _registryOptions, string ext)
        {
            var option = _registryOptions.GetLanguageByExtension(ext);
            if (option == null) return;

            var _textMateInstallation = text.InstallTextMate(_registryOptions);
            _textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(option.Id));
        }

        public void SaveFile(TextEditor current, string currentFile)
        {
            if(!File.Exists(currentFile))
            {
                RenameDialog dialog = new RenameDialog("");
                dialog.Show(MainWindowViewModel.Instance.MainWindow);
                dialog.OnNameConfirmed += (a, e) => 
                {
                    var state = FileTask.RequestCreateFile(e, current.Document.Text);
                    _loggerService.Warn($"state save file: {e}, == {state}");
                };
            }
        }

        private void LoadDirectoryRecursive(ObservableCollection<FileSystemNode> nodes, string directoryPath)
        {
            ExceptionInvoker.TryInvoke<Exception>(() => { 
                foreach (var subDirectory in Directory.GetDirectories(directoryPath))
                {
                    var directoryNode = new FileSystemNode(subDirectory)
                    {
                        IsExpanded = false
                    };

                    nodes.Add(directoryNode);

                    LoadDirectoryRecursive(directoryNode.Children, subDirectory);
                }

                foreach (var file in Directory.GetFiles(directoryPath))
                {
                    var fileNode = new FileSystemNode(file);
                    nodes.Add(fileNode);
                }
            });

        }

        [RelayCommand]
        public async Task RenameSelectedFileAsync()
        {
            if (SelectedNode == null) return;

            var dialog = new RenameFileDialog(SelectedNode.Path);

            dialog.Show(MainWindowViewModel.Instance.MainWindow);
            dialog.OnFileNameConfirmed += async (a, e) =>
            {
                await HandleFileNameConfirmed(e);
            };
        }

        [RelayCommand]
        public async Task DeleteSelectedFileAsync()
        {
            if (SelectedNode == null) return;

            var result = await FileTask.RequestDeletePath(SelectedNode.Path);
            if (result) DeleteNode(SelectedNode);
        }

        private async Task HandleFileNameConfirmed(RenameInfo e)
        {
            if (SelectedNode == null) return;

            var oldPath = SelectedNode.Path;
            var result = await FileTask.RequestRenamePath(oldPath, e.Path);

            if (result) RenameNode(SelectedNode, e.Name);
        }

        public void LoadChildren(FileSystemNode node)
        {
            if (!node.IsDirectory) return;

            try
            {
                node.Children.Clear();

                foreach (var directory in Directory.GetDirectories(node.Path))
                {
                    var childNode = new FileSystemNode(directory);
                    childNode.Children.Add(new FileSystemNode("dummy"));
                    node.Children.Add(childNode);
                }

                foreach (var file in Directory.GetFiles(node.Path))
                {
                    node.Children.Add(new FileSystemNode(file));
                }
            }
            catch (UnauthorizedAccessException) { }
        }

        public void DeleteNode(FileSystemNode node)
        {
            var parent = FindParent(Roots, node);
            parent?.Children.Remove(node);
            if (parent == null) Roots.Remove(node);
        }

        public void RenameNode(FileSystemNode node, string newName)
        {
            if (node == null || string.IsNullOrWhiteSpace(newName)) return;

            var oldPath = node.Path;
            var directory = Path.GetDirectoryName(oldPath);
            var newPath = Path.Combine(directory, newName);

            if (oldPath.Equals(newPath, StringComparison.OrdinalIgnoreCase)) return;

            node.Path = newPath;
            node.Name = newName;
        }

        public void OnNodeSelected(FileSystemNode node)
        {
            if (node != null && node.Children.Count == 1)
            {
                LoadChildren(node);
            }
        }

        private FileSystemNode FindParent(ObservableCollection<FileSystemNode> nodes, FileSystemNode target)
        {
            foreach (var node in nodes)
            {
                if (node.Children.Contains(target)) return node;

                var parent = FindParent(node.Children, target);
                if (parent != null) return parent;
            }
            return null;
        }
    }
}