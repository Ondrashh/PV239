using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Gms.Common;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace TVTrack.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static readonly string ChannelID = "NewEpisode";
    public static readonly int NotificationID = 101;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.PostNotifications }, 0);
        }

        CreateNotificationChannel();
    }


    public void CreateNotificationChannel()
    {
        if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var channel = new NotificationChannel(ChannelID, "New Episode Notification", NotificationImportance.Default);

            var notificaitonManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificaitonManager.CreateNotificationChannel(channel);
        }
    }
}
