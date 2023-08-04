using Android.App;
using Android.OS;
using Android.Content.PM;

namespace GpsSync;


[Activity(
    Theme = "@style/Maui.SplashTheme", 
    MainLauncher = true, 
    ConfigurationChanges = 
        ConfigChanges.ScreenSize | 
        ConfigChanges.Orientation | 
        ConfigChanges.UiMode | 
        ConfigChanges.ScreenLayout | 
        ConfigChanges.SmallestScreenSize | 
        ConfigChanges.Density
)]
[IntentFilter(
    new[] { ShinyNotificationIntents.NotificationClickAction }
)]
public class MainActivity : MauiAppCompatActivity
{
}

