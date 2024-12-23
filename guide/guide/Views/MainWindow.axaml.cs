using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System;

namespace guide.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape || e.Key == Key.Back)
        {
            if (NavigationService.CanGoBack())
            {
                NavigationService.GoBack();
                e.Handled = true; // Устанавливаем флаг, что событие обработано
            }
        }
    }
}