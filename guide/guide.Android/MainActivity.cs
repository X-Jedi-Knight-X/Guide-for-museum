using Android.App;
using Android.Content.PM;
using Android.Icu.Text;
using Android.Net;
using Android.OS;
using Android.Views;
using Avalonia;
using Avalonia.Android;
using Avalonia.Controls;

namespace guide.Android;

[Activity(
    Label = "Любимский музей, экспонаты",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon_guide",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    //protected override void OnCreate(Bundle savedInstanceState)
    //{
    //    base.OnCreate(savedInstanceState);
    //
    //    // This was the first attempt
    //    Window.DecorView.SystemUiFlags = (SystemUiFlags)(
    //        SystemUiFlags.LayoutStable
    //        | SystemUiFlags.LayoutFullscreen
    //        | SystemUiFlags.Fullscreen);
    //
    //    // This also didn't work
    //    Window.AddFlags(WindowManagerFlags.Fullscreen);
    //}
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
    public override void OnBackPressed()
    {
        if (NavigationService.CanGoBack())
        {
            NavigationService.GoBack();
        }
        else
        {
            base.OnBackPressed();
        }
    }

}
