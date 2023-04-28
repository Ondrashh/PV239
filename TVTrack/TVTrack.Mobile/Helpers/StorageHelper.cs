namespace TVTrack.Mobile.Helpers;
public static class StorageHelper
{
    public async static Task StoreUsername(string username)
    {
        await SecureStorage.Default.SetAsync("username", username);
    }

    public async static Task LogOut()
    {
        SecureStorage.Default.Remove("username");
    }

    public async static Task<string> GetUsername()
    {
        return await SecureStorage.Default.GetAsync("username");
    }
}
