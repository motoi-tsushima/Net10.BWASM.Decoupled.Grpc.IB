using Microsoft.JSInterop;

namespace Net10.BWASM.Decoupled.Grpc.IB.Client.Services;

public class LocalStorageService(IJSRuntime jsRuntime)
{
    public async Task<string?> GetItemAsync(string key)
    {
        try
        {
            return await jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        }
        catch
        {
            return null;
        }
    }

    public async Task SetItemAsync(string key, string value)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }
        catch { }
    }
}
