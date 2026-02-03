using Blazored.LocalStorage;

namespace MindNose.Front.Services;

public class LocalStorageTheme
{
    private readonly ILocalStorageService _localStorage;

    public LocalStorageTheme(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string> GetTheme() => await _localStorage.GetItemAsync<string>("theme");

    public async Task SetTheme(string theme)
    {
        if (string.IsNullOrWhiteSpace(theme))
            throw new ArgumentNullException(nameof(theme));

        await _localStorage.SetItemAsync("theme", theme);
    }
}
