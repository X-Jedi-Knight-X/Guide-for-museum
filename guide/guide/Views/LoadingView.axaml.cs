using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Avalonia.Platform;
using guide.ViewModels;

namespace guide.Views;

public partial class LoadingView : UserControl
{
    
    public static List<exhibit> exhibits_all;
    public static List<int> ids;
    public static int PageSize;
    public LoadingView()
    {
        InitializeComponent();
        exhibits_all = new List<exhibit>();
        ids = new List<int>();
        PageSize = 20;
        this.Loaded += Main;
    }

    private async void Main(object sender, RoutedEventArgs e)
    {
        
        await Task.Run(() =>
        {
            NavigationService.Width = VisualRoot.ClientSize.Width;
            NavigationService.Height = VisualRoot.ClientSize.Height;
            NavigationService.coef = double.Min(VisualRoot.ClientSize.Width/540, VisualRoot.ClientSize.Height/940);
            Getids();
            deserialization_exhibits();
        });
        LoadProgress.IsVisible = false;
        NavigationService.Navigate(new MenuView());
        
    }

    
    private void Getids()
    {
        using (var stream = AssetLoader.Open(new Uri("avares://guide/Resources/exponat/ids.txt")))
        using (var sr = new StreamReader(stream))
        {
            ids = sr.ReadToEnd()
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }
    }
    private void deserialization_exhibits()
        { 
            using (var stream = AssetLoader.Open(new Uri("avares://guide/Resources/exponat/exhibits_top.json")))
            using (var sr = new StreamReader(stream))
            {
                string data = sr.ReadToEnd();
                var exponats = JsonSerializer.Deserialize<Dictionary<int, Dictionary<string, object>>>(data);
                foreach (int id in ids)
                {
                    try
                    {
                        exhibit _exhibit = new exhibit(exponats[id], id);
                        exhibits_all.Add(_exhibit);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
}