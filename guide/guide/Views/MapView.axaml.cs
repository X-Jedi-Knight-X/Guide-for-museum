using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using System;
using System.Linq;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Threading;
using guide.ViewModels;

namespace guide.Views;

public partial class MapView : UserControl
{
    private List<exhibit> exhibits_all;
    private List<exhibit> exhibits_selected;
    private int pagenumber;
    private int PageSize;
    private int page;
    public MapView()
    {
        InitializeComponent();
        PageSize = LoadingView.PageSize;
        exhibits_all = LoadingView.exhibits_all;
        this.Loaded += Main;
    }

    public MapView(string hall)
    {
        InitializeComponent();
        PageSize = LoadingView.PageSize;
        exhibits_all = LoadingView.exhibits_all;
        this.Loaded += Main;
        this.Loaded += (sender, e) => SearchExhibitsAndView(hall);
 
    }
    
    
    private void SetPosition(Button button, double x, double y)
    {
        double w = button.Bounds.Width;
        double h = button.Bounds.Height;
        ScaleTransform scale = new ScaleTransform(NavigationService.coef+0.04, NavigationService.coef+0.04);
        button.RenderTransform = scale;
        Canvas.SetLeft(button, plan.Bounds.Width * x - (w - w * NavigationService.coef) / 2);
        Canvas.SetTop(button, plan.Bounds.Height * y - (h - h * NavigationService.coef) / 2);
    }

    private void PlacePartExhibits(List<exhibit> _exhibits)
    {
        if (page != 0)
        {
            Button previous = new Button
            {
                Content = "Предыдущая страница",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                CornerRadius = new CornerRadius(10),
            };
            previous.Click += (sender, e) => PlaceExhibits(exhibits_selected,page-1);
            ExhibitsList.Children.Add(new Border
            {
                Classes = { "all" },
                Child = previous,
                Margin = new Thickness(70,0,70,0),
            });
        }
        for (int i = 0; i < _exhibits.Count; i++)
        {
            Border border = CreateBorder(_exhibits[i]);
            ExhibitsList.Children.Add(border);
        }

        if (page + 1 < exhibits_selected.Count / PageSize)
        {
            Button next = new Button
            {
                Content = "Следующая страница",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                CornerRadius = new CornerRadius(10),
            };
            next.Click += (sender, e) => PlaceExhibits(exhibits_selected,page+1);
            ExhibitsList.Children.Add(new Border
            {
                Classes = { "all" },
                Child = next,
                Margin = new Thickness(70,10,70,20),
                
            });
        }
    }

    private void PlaceExhibits(List<exhibit> _exhibits, int _page)
    {
        ExhibitsList.Children.Clear();
        scroll.ScrollToHome();
        page = _page;
        PlacePartExhibits(getPartExhibits(_exhibits, page));
    }
    private List<exhibit> getPartExhibits(List<exhibit> exhibitsToShow, int page)
    {
        int stay = exhibits_selected.Count - PageSize*page;
        if (stay>=PageSize)
            return exhibitsToShow.GetRange(page*PageSize, PageSize);
        else
            return exhibits_selected.GetRange(page*PageSize, stay);
    }

    private Border CreateBorder(exhibit _exhibit)
    {
        Button button = new Button
        {
            Content = "Подробнее",
            CornerRadius = new CornerRadius(10),
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Left,
            Padding = new Thickness(5, 2),
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
                    new Image{Source = _exhibit.preview},
                    new StackPanel
                    {
                        Children =
                        {
                            new TextBlock { Text = _exhibit.name, Width = NavigationService.Width*0.7, FontSize = 16, FontWeight = FontWeight.Bold, TextWrapping = TextWrapping.Wrap},
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
        NavigationService.Navigate(new ExhibitDescriptionView(exhibit, exhibits_selected));
    }

    private void Main(object sender, RoutedEventArgs e)
    {
        MapCanvas.Height=plan.Bounds.Height;
        scroll.Height=NavigationService.Height-plan.Bounds.Height;
        Canvas.SetLeft(names,plan.Bounds.Width*0.35);
        Canvas.SetTop(names,plan.Bounds.Height*0.49);
        SetPosition(nature_hall,0.057,0.057);
        SetPosition(gallery, 0.06, 0.42);
        SetPosition(antique_hall, 0.313, 0.06);
        SetPosition(city_history_hall, 0.5, 0.3);
        SetPosition(lifi_hall, 0.69537, 0.3);
        SetPosition(scales_hall, 0.884, 0.3);
        SetPosition(war_hall, 0.688, 0.052);
        SetPosition(history_technology_hall, 0.88, 0.052);
        SetPosition(soviet_hall, 0.756, 0.052);
        SetPosition(modern_hall, 0.502, 0.052);
        if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
        {
            back.Text = "Чтобы вернуться на предыдущую страницу нажмите esc";
        }
        else
        {
            back.Text = "Вернуться на предыдущую страницу можно системной кнопкой назад";
        }

        nature_hall.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush=new SolidColorBrush(Colors.White);
            gallery.BorderBrush = null;
            antique_hall.BorderBrush = null;
            city_history_hall.BorderBrush = null;
            lifi_hall.BorderBrush = null;
            scales_hall.BorderBrush = null;
            war_hall.BorderBrush = null;
            history_technology_hall.BorderBrush = null;
            soviet_hall.BorderBrush = null;
            modern_hall.BorderBrush = null;
            SearchExhibitsAndView("природ");
            

        }, handledEventsToo: true);
        gallery.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush = null;
            gallery.BorderBrush=new SolidColorBrush(Colors.White);
            antique_hall.BorderBrush = null;
            city_history_hall.BorderBrush = null;
            lifi_hall.BorderBrush = null;
            scales_hall.BorderBrush = null;
            war_hall.BorderBrush = null;
            history_technology_hall.BorderBrush = null;
            soviet_hall.BorderBrush = null;
            modern_hall.BorderBrush = null;
            SearchExhibitsAndView("галерея");
            
        }, handledEventsToo: true);
        antique_hall.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush = null;
            gallery.BorderBrush = null;
            antique_hall.BorderBrush = new SolidColorBrush(Colors.White);
            city_history_hall.BorderBrush = null;
            lifi_hall.BorderBrush = null;
            scales_hall.BorderBrush = null;
            war_hall.BorderBrush = null;
            history_technology_hall.BorderBrush = null;
            soviet_hall.BorderBrush = null;
            modern_hall.BorderBrush = null;
            SearchExhibitsAndView("старинной");
            
        }, handledEventsToo: true);
        city_history_hall.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush = null;
            gallery.BorderBrush = null;
            antique_hall.BorderBrush = null;
            city_history_hall.BorderBrush = new SolidColorBrush(Colors.White);
            lifi_hall.BorderBrush = null;
            scales_hall.BorderBrush = null;
            war_hall.BorderBrush = null;
            history_technology_hall.BorderBrush = null;
            soviet_hall.BorderBrush = null;
            modern_hall.BorderBrush = null;
            SearchExhibitsAndView("истории города");
            
        }, handledEventsToo: true);
        lifi_hall.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush = null;
            gallery.BorderBrush = null;
            antique_hall.BorderBrush = null;
            city_history_hall.BorderBrush = null;
            lifi_hall.BorderBrush = new SolidColorBrush(Colors.White);
            scales_hall.BorderBrush = null;
            war_hall.BorderBrush = null;
            history_technology_hall.BorderBrush = null;
            soviet_hall.BorderBrush = null;
            modern_hall.BorderBrush = null;
            SearchExhibitsAndView("быта");
            
        }, handledEventsToo: true);
        scales_hall.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush = null;
            gallery.BorderBrush = null;
            antique_hall.BorderBrush = null;
            city_history_hall.BorderBrush = null;
            lifi_hall.BorderBrush = null;
            scales_hall.BorderBrush = new SolidColorBrush(Colors.White);
            war_hall.BorderBrush = null;
            history_technology_hall.BorderBrush = null;
            soviet_hall.BorderBrush = null;
            modern_hall.BorderBrush = null;
            SearchExhibitsAndView("весов");
            
        }, handledEventsToo: true);
        war_hall.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush = null;
            gallery.BorderBrush = null;
            antique_hall.BorderBrush = null;
            city_history_hall.BorderBrush = null;
            lifi_hall.BorderBrush = null;
            scales_hall.BorderBrush = null;
            war_hall.BorderBrush = new SolidColorBrush(Colors.White);
            history_technology_hall.BorderBrush = null;
            soviet_hall.BorderBrush = null;
            modern_hall.BorderBrush = null;
            SearchExhibitsAndView("войны");
            
        }, handledEventsToo: true);
        history_technology_hall.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush = null;
            gallery.BorderBrush = null;
            antique_hall.BorderBrush = null;
            city_history_hall.BorderBrush = null;
            lifi_hall.BorderBrush = null;
            scales_hall.BorderBrush = null;
            war_hall.BorderBrush = null;
            history_technology_hall.BorderBrush = new SolidColorBrush(Colors.White);
            soviet_hall.BorderBrush = null;
            modern_hall.BorderBrush = null;
            SearchExhibitsAndView("техники");
            
        }, handledEventsToo: true);
        soviet_hall.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush = null;
            gallery.BorderBrush = null;
            antique_hall.BorderBrush = null;
            city_history_hall.BorderBrush = null;
            lifi_hall.BorderBrush = null;
            scales_hall.BorderBrush = null;
            war_hall.BorderBrush = null;
            history_technology_hall.BorderBrush = null;
            soviet_hall.BorderBrush = new SolidColorBrush(Colors.White);
            modern_hall.BorderBrush = null;
            SearchExhibitsAndView("периода");
            
        }, handledEventsToo: true);
        modern_hall.AddHandler(Button.PointerPressedEvent, (sender, e) =>
        {
            nature_hall.BorderBrush = null;
            gallery.BorderBrush = null;
            antique_hall.BorderBrush = null;
            city_history_hall.BorderBrush = null;
            lifi_hall.BorderBrush = null;
            scales_hall.BorderBrush = null;
            war_hall.BorderBrush = null;
            history_technology_hall.BorderBrush = null;
            soviet_hall.BorderBrush = null;
            modern_hall.BorderBrush = new SolidColorBrush(Colors.White);
            SearchExhibitsAndView("совреме");
            
        }, handledEventsToo: true);
        
    }

    public async void SearchExhibitsAndView(String locationName)
    {
        back.IsVisible = false;
        ExhibitsList.Children.Clear();
        LoadProgress.IsVisible = true;
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);
        await Task.Run(() =>
        {
            exhibits_selected = new List<exhibit>();
                    foreach (exhibit exhibit in exhibits_all)
                    {
                        if (exhibit.location.Contains(locationName))
                        {
                            exhibits_selected.Add(exhibit);
                        }
                    }
        });
        if (exhibits_selected.Count != 0)
            PlaceExhibits(exhibits_selected, 0);
        else
        {
            ExhibitsList.Children.Clear();
            ExhibitsList.Children.Add(new TextBlock
            {
                Text = "В этот зал экспонаты пока не добавлены.",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            });
        }
        LoadProgress.IsVisible = false;
        
    } 
}