using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using JavaTransformer.UI.MainDesktop.Models.Core;
using JavaTransformer.UI.MainDesktop.Views.UtilWindow.MessageBoxs;
using System;
using System.Xml.Linq;

namespace JavaTransformer.UI.MainDesktop;

public partial class RenameFileWindow : Window
{
    public event EventHandler<string> OnFileNameConfirmed;
    public event EventHandler OnCanceled;

    private WindowManagedCore core;

    public RenameFileWindow()
    {
        InitializeComponent();
        Loaded += LoadedWindow;
    }

    private void LoadedWindow(object? sender, RoutedEventArgs e)
    {
        core = new WindowManagedCore(this);
        core.ApplyModernNoBorderStyle();
        core.EnableDragMoveForWindow();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        string fileName = FileNameTextBox.Text?.Trim();
        OnFileNameConfirmed?.Invoke(this, fileName);
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        OnCanceled?.Invoke(this, EventArgs.Empty);
        Close();
    }

    public void ShowDialog(Window parent, string currentName)
    {
        if (parent != null)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        if (!string.IsNullOrEmpty(currentName))
        {
            FileNameTextBox.Text = currentName;
            FileNameTextBox.SelectAll();
        }
        ShowDialog(parent);
    }
}