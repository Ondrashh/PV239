using TVTrack.Mobile.Helpers;
using TVTrack.Mobile.Services.Interfaces;

namespace TVTrack.Mobile.Services;
public class GlobalExceptionService : IGlobalExceptionService
{
    public void HandleException(Exception exception, string? source = null)
    {
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await AlertHelper.ShowErrorSnackbar("An error has occured");
        });
    }
}