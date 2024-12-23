using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace guide.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        

        
        NavigationService.contentControl = this.FindControl<ContentControl>("Page_Container");
        NavigationService.Navigate(new LoadingView());
        
        
    }

    
}
