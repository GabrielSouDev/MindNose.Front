using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MindNose.Front.Services;

public class LocalStorageAuthProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly ClaimsIdentity _anonymous = new ClaimsIdentity();

    public LocalStorageAuthProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        var identity = await GetClaimsIdentityFromTokenAsync(token);

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task MarkUserAsAuthenticatedAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentNullException(nameof(token));

        await _localStorage.SetItemAsync("token", token);

        var identity = await GetClaimsIdentityFromTokenAsync(token);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    public async Task MarkUserAsLoggedOutAsync()
    {
        await _localStorage.RemoveItemAsync("token");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(_anonymous))));
    }

    private async Task<ClaimsIdentity> GetClaimsIdentityFromTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return _anonymous;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (expClaim != null && long.TryParse(expClaim, out long expSeconds))
            {
                var expDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                if (expDate < DateTime.UtcNow)
                {
                    await _localStorage.RemoveItemAsync("token");
                    return _anonymous;
                }
            }

            var claims = new List<Claim>();
            claims.AddRange(jwtToken.Claims.Where(c =>
                c.Type == ClaimTypes.Name ||
                c.Type == ClaimTypes.Email ||
                c.Type == ClaimTypes.Role ||
                c.Type == JwtRegisteredClaimNames.Sub
            ));

            return new ClaimsIdentity(claims, "localstorage_auth");
        }
        catch
        {
            return _anonymous;
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("token");
    }
}
