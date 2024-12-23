using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;

namespace guide.ViewModels;

public class exhibit
{
    public string name;
    public string description;
    public Bitmap preview;
    public string OriginalImage;
    public string location;

    public exhibit(Dictionary<string, object> data, int id)
    {
        name = data["name"].ToString();
        try { description = data["description"].ToString(); }
        catch { description = "";}
        try
        {
            preview = new Bitmap(AssetLoader.Open(new Uri($"avares://guide/Resources/exponat/preview/part{id%10}/{id}.jpg")));
        }
        catch
        {
            preview = new Bitmap("");
        }
        string idImage= data["images"].ToString();
        location = data["location"].ToString();
        OriginalImage = $"https://www.goskatalog.ru/muzfo-imaginator/rest/images/public/750/{idImage}/{idImage}.jpg";
    }
    
}