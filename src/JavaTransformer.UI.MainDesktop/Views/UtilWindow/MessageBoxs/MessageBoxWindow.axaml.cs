using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using JavaTransformer.UI.MainDesktop.Models.Core;

namespace JavaTransformer.UI.MainDesktop;

public partial class MessageBoxWindow : Window
{
    private WindowManagedCore core;

    public MessageBoxWindow()
    {
        InitializeComponent();
        Loaded += MessageBox_Loaded;
    }

    private void MessageBox_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        core = new WindowManagedCore(this);
        core.EnableTopMost();
        core.EnableDragMoveForWindow();
        core.ApplyModernNoBorderStyle();

    }

    public void SetMessage(string message)
    {
        _output.Text = message;
    }
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    public void ShowMessage(string message, string title, Window? parent)
    {
        if (parent != null)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowDialog(parent);
        }
        else Show();

        _header.Text = title;
        SetMessage(message);

    }
}