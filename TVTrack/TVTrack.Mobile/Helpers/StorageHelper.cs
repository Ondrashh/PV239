using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Helpers;
public static class StorageHelper
{
    public async static Task StoreUsername(string username)
    {
        await SecureStorage.Default.SetAsync("username", username);
    }

    public async static Task<string> GetUsername()
    {
        return "test";
        // return await SecureStorage.Default.GetAsync("username");
    }
}
