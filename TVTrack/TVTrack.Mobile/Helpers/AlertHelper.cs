using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TVTrack.Mobile.Helpers
{
    public static class AlertHelper
    {
        public static async Task ShowToast(string message, ToastDuration duration = ToastDuration.Long, double fontSize = 14)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var toast = Toast.Make(message, duration, fontSize);
            await toast.Show(cts.Token);
        }

        public static async Task ShowErrorSnackbar(string message, double duration = 10, double fontSize = 14)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.OrangeRed,
                CornerRadius = new CornerRadius(10),
                Font = Microsoft.Maui.Font.SystemFontOfSize(fontSize),
                ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(fontSize)
            };
            TimeSpan durationTimeSpan = TimeSpan.FromSeconds(duration);
            var snackbar = Snackbar.Make(message, duration: durationTimeSpan, visualOptions: snackbarOptions);
            await snackbar.Show(cts.Token);
        }
    }
}
