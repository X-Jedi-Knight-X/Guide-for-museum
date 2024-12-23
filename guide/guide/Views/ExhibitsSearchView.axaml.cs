using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Input;
using Avalonia.Platform;
using Avalonia;
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using DynamicData;
using guide.ViewModels;

namespace guide.Views;

public partial class ExhibitsSearchView : UserControl
{
    private List<exhibit> exhibits;
    private List<exhibit> filteredExhibits; 
    private List<int> ids;
    private int _page;
    private CancellationTokenSource _cts;
    private int PageSize;
    public ExhibitsSearchView()
    {
        InitializeComponent();
        
        PageSize = LoadingView.PageSize;
        ids = LoadingView.ids;
        exhibits = LoadingView.exhibits_all;
        filteredExhibits = exhibits;
        CreateSearch();
        display_exhibits(exhibits,0);
    }
    
    private void CreateSearch()
    {
        var textBoxSearch = new TextBox
                {
                    FontSize = Math.Round(25*NavigationService.coef),
                    Height = 40,
                    CornerRadius = new CornerRadius(15),
                    Watermark = "Введите название экспоната",
                };
        textBoxSearch.TextChanged += OnSearchTextChanged;
        _border.VerticalAlignment = VerticalAlignment.Top;
        _border.Padding = new Thickness(5);
        var panel = new StackPanel();
        panel.Children.Add(textBoxSearch);
        _border.Child = panel;
    }
    private async void display_exhibits(List<exhibit> exhibitsToShow, int page)
    {
        LoadProgress.IsVisible = true;
        _page = page;
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);

        if (exhibitsToShow.Count != 0)
        {
            
            var partExhibits = await Task.Run(() => getPartExhibits(exhibitsToShow, page));
            display_part_exhibits(partExhibits);
        }
        else
        {
            PanelExhibits.Children.Clear();
            PanelExhibits.Children.Add(new TextBlock
            {
                Text = "По вашему запросу ничего не найдено.",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            });
        }


        PageNumber.Text = (page + 1).ToString();

        LoadProgress.IsVisible = false; 
    }

    private List<exhibit> getPartExhibits(List<exhibit> exhibitsToShow, int page)
    {
        int stay = exhibitsToShow.Count - PageSize*page;
        if (stay>=PageSize)
            return exhibitsToShow.GetRange(page*PageSize, PageSize);
        else
            return exhibitsToShow.GetRange(page*PageSize, stay);
    }
    
    private void display_part_exhibits(List<exhibit> Partexhibits)
    {
        if (_page != 0)
            previous.IsVisible = true;
        else
            previous.IsVisible = false;

        if (_page + 1 < filteredExhibits.Count / PageSize)
            next.IsVisible = true;
        else
            next.IsVisible = false;
        
        if (next.IsVisible==false && previous.IsVisible==false)
            prevnextBorder.IsVisible = false;
        
        PanelExhibits.Children.Clear();
        foreach (exhibit exhibit in Partexhibits)
        {
            Border exhibitBorder = CreateBorder(exhibit); 
            PanelExhibits.Children.Add(exhibitBorder);
        }
        ScrollViewer.ScrollToHome();
    }

    private Border CreateBorder(exhibit _exhibit)
    {
        Button button = new Button
        {
            Content = "Подробнее",
            HorizontalAlignment = HorizontalAlignment.Left,
            Padding = new Thickness(5, 2),
            FontSize = 20,
            CornerRadius = new CornerRadius(10),
        };
        button.Click+= (sender, e) => OpenExhibitDescription_Click(sender, e, _exhibit);
                                        
                                    
        Border border = new Border
        {
            Classes = { "all" },
            Child = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 10,
                Children =
                {
                    new Image { Source = _exhibit.preview },
                    new StackPanel
                    {
                        Children =
                        {
                            new TextBlock
                            {
                                Text = _exhibit.name, Width = NavigationService.Width * 0.7, FontSize = 16,
                                FontWeight = FontWeight.Bold, TextWrapping = TextWrapping.Wrap
                            },
                            button
                        }
                    }
                }
            }
        };
        return border;
    }

    private void OpenExhibitDescription_Click(object? sender, RoutedEventArgs e, exhibit exhibit)
    {
        NavigationService.Navigate(new ExhibitDescriptionView(exhibit, exhibits));
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = (sender as TextBox).Text.ToLower();

        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        try
        {
            await Task.Delay(1000, token);

            filteredExhibits = exhibits
                .Where(exhibit => exhibit.name.ToLower().Contains(searchText))
                .ToList();

            PanelExhibits.Children.Clear();
            display_exhibits(filteredExhibits, 0);
        }
        catch (TaskCanceledException)
        {
            // Задача была отменена, ничего не делаем
        }
    }

    private void NextPage_Click(object? sender, RoutedEventArgs e)
    {
        PanelExhibits.Children.Clear();
        display_exhibits(filteredExhibits, int.Parse(PageNumber.Text));
    }

    private void PreviousPage_Click(object? sender, RoutedEventArgs e)
    {
        PanelExhibits.Children.Clear();
        display_exhibits(filteredExhibits, int.Parse(PageNumber.Text)-2);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
}
