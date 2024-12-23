using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace guide.Views;

public partial class MenuView : UserControl
{
    public MenuView()
    {
        InitializeComponent();
        
    }
    

    private void ExhibitsSearch_Click(object? sender, RoutedEventArgs e)
    {
        NavigationService.Width = this.VisualRoot.ClientSize.Width;
        NavigationService.Height = this.VisualRoot.ClientSize.Height;
        NavigationService.Navigate(new ExhibitsSearchView());
    }

    private void Map_Click(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MapView());
    }
}