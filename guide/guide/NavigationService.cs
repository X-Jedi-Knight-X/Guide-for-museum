using System.Collections.Generic;
using Avalonia.Controls;

namespace guide;

public static class NavigationService
{
    public static double Width;
    public static double Height;
    public static double coef;
    public static ContentControl contentControl;
    
    private static readonly Stack<object> history = new();
    public static void Navigate(UserControl newView)
    {
        history.Push(newView);
        contentControl.Content = newView;
    }

    public static void ChangeView(UserControl newView)
    {
        contentControl.Content = newView;
    }
    public static void GoBack()
    {
        history.Pop();
        contentControl.Content = history.Peek();
    }
    
    public static bool CanGoBack()
    {
        return history.Count > 2;
    }
    
}