using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using System.IO;
using Avalonia.Labs.Gif;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using guide.ViewModels;

namespace guide.Views;

public partial class ExhibitDescriptionView : UserControl
{
    public exhibit Exhibit;
    public List<exhibit> Exhibits;
    public ExhibitDescriptionView(exhibit _exhibit, List<exhibit> exhibits)
    {
        Exhibit = _exhibit;
        Exhibits = exhibits;
        InitializeComponent();
        ShowView();
    }
    private async void ShowView()
    {
        _= DownloadAndDisplayImage(MyImage, Exhibit.OriginalImage);
        ShowDescription();
    }

    public void ShowDescription()
    {
        Name.Text = Exhibit.name;
        Description.Text = Exhibit.description.TrimEnd('\n', '\r', ' ', '\t');
        Location_in_map.Text = Exhibit.location.TrimEnd('\n', '\r', ' ', '\t');
    }
    
    private void CloseView_Click(object? sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }
    public async Task DownloadAndDisplayImage(Image imageControl, string imageUrl)
    {
        
            Loading.IsVisible = true;
            using (HttpClient client = new HttpClient())
            {
                bool success = false;
                while (!success)
                {
                    HttpResponseMessage response = await client.GetAsync(imageUrl);
                    success=response.IsSuccessStatusCode;
                }

                if (success)
                {
                    // Загружаем изображение как массив байт
                                    var imageBytes = await client.GetByteArrayAsync(imageUrl);
                    
                                    // Используем MemoryStream для создания Bitmap из массива байт
                                    using (var stream = new MemoryStream(imageBytes))
                                    {
                                        // Создаем объект Bitmap из потока
                                        var bitmap = new Bitmap(stream);
                    
                                        // Устанавливаем Bitmap как источник для элемента Image
                                        imageControl.Source = bitmap;
                                    }
                                    
                    Loading.IsVisible = false;
                }
                
            }
        // Создаем HttpClient для загрузки изображения

    }

    private void Location_in_map_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Exhibit.location.ToLower().Contains("природ"))
            NavigationService.Navigate(new MapView("природ"));
        if (Exhibit.location.ToLower().Contains("галерея"))
            NavigationService.Navigate(new MapView("галерея"));
        if (Exhibit.location.ToLower().Contains("старинной"))
            NavigationService.Navigate(new MapView("старинной"));
        if (Exhibit.location.ToLower().Contains("истории города"))
            NavigationService.Navigate(new MapView("истории города"));
        if (Exhibit.location.ToLower().Contains("быта"))
            NavigationService.Navigate(new MapView("быта"));
        if (Exhibit.location.ToLower().Contains("весов"))
            NavigationService.Navigate(new MapView("весов"));
        if (Exhibit.location.ToLower().Contains("войны"))
            NavigationService.Navigate(new MapView("войны"));
        if (Exhibit.location.ToLower().Contains("техники"))
            NavigationService.Navigate(new MapView("техники"));
        if (Exhibit.location.ToLower().Contains("периода"))
            NavigationService.Navigate(new MapView("периода"));
        if (Exhibit.location.ToLower().Contains("совреме"))
            NavigationService.Navigate(new MapView("совреме"));
    }

    private void PreviousPage_Click(object? sender, RoutedEventArgs e)
    {
        int i = Exhibits.IndexOf(Exhibit);
        NavigationService.ChangeView(new ExhibitDescriptionView(Exhibits[((i-1)%Exhibits.Count+Exhibits.Count)%Exhibits.Count], Exhibits));
        
    }

    private void NextPage_Click(object? sender, RoutedEventArgs e)
    {
        int i = Exhibits.IndexOf(Exhibit);
        NavigationService.ChangeView(new ExhibitDescriptionView(Exhibits[((i+1)%Exhibits.Count+Exhibits.Count)%Exhibits.Count], Exhibits));
    }
}
