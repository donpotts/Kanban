using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using KanbanTasks.Shared.Models;
using KanbanTasks.Shared.Blazor.Models;

namespace KanbanTasks.Shared.Blazor.Authorization;

public class IdentityAuthenticationStateProvider(HttpClient httpClient) : AuthenticationStateProvider
{
    private AccessTokenResponse? accessTokenResponse;
    private DateTimeOffset? expiresAt;

    private static readonly ClaimsPrincipal anonymousUser = new(new ClaimsIdentity());
    private ClaimsPrincipal currentUser = anonymousUser;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(currentUser));

    public async Task LoginAsync(LoginModel loginModel)
    {
        accessTokenResponse = null;
        expiresAt = null;
        currentUser = anonymousUser;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));

        var response = await httpClient.PostAsJsonAsync("/identity/login?useCookies=false", loginModel);

        var date = response.Headers.Date ?? DateTimeOffset.UtcNow;

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("The login attempt failed.");
            }
            else if (response.StatusCode != HttpStatusCode.NotFound)
            {
                string? message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }

            response.EnsureSuccessStatusCode();
        }

        accessTokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>()
            ?? throw new Exception("The login attempt failed.");

        expiresAt = date.AddSeconds(accessTokenResponse.ExpiresIn);

        HttpRequestMessage meRequest = new(HttpMethod.Get, "/api/user/@me");
        meRequest.Headers.Authorization = new("Bearer", accessTokenResponse.AccessToken);

        response = await httpClient.SendAsync(meRequest);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("The login attempt failed.");
            }
            else if (response.StatusCode != HttpStatusCode.NotFound)
            {
                string? message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }

            response.EnsureSuccessStatusCode();
        }

        var me = await response.Content.ReadFromJsonAsync<ApplicationUserWithRolesDto>()
            ?? throw new Exception("The login attempt failed.");

        List<Claim> claims = [];

        string name = string.Empty;

        if (!string.IsNullOrWhiteSpace(me.FirstName))
        {
            claims.Add(new(ClaimTypes.GivenName, me.FirstName));

            name = me.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(me.LastName))
        {
            claims.Add(new(ClaimTypes.Surname, me.LastName));

            if (!string.IsNullOrEmpty(name))
            {
                name += " ";
            }

            name += me.LastName;
        }

        if (string.IsNullOrEmpty(name))
        {
            name = "User";
        }

        claims.Add(new(ClaimTypes.Name, name));

        if (!string.IsNullOrEmpty(me.Email))
        {
            claims.Add(new(ClaimTypes.Email, me.Email));
        }

        foreach (var role in me.Roles ?? Enumerable.Empty<string>())
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        ClaimsIdentity loggedInUserIdentity = new(claims, "Identity");
        currentUser = new ClaimsPrincipal(loggedInUserIdentity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
    }

    public async Task<string?> GetBearerTokenAsync()
    {
        if (accessTokenResponse == null || expiresAt == null)
        {
            return null;
        }

        if (expiresAt <= DateTimeOffset.UtcNow)
        {
            var response = await httpClient.PostAsJsonAsync(
                "/identity/refresh",
                new { accessTokenResponse.RefreshToken });

            var date = response.Headers.Date ?? DateTimeOffset.UtcNow;

            if (!response.IsSuccessStatusCode)
            {
                accessTokenResponse = null;
                expiresAt = null;
                currentUser = anonymousUser;
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("The token could not be refreshed.");
                }
                else if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    string? message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

                response.EnsureSuccessStatusCode();
            }

            accessTokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();

            if (accessTokenResponse == null)
            {
                expiresAt = null;
                currentUser = anonymousUser;
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));

                throw new Exception("The login attempt failed.");
            }

            expiresAt = date.AddSeconds(accessTokenResponse.ExpiresIn);
        }

        return accessTokenResponse.AccessToken;
    }

    public void Logout()
    {
        accessTokenResponse = null;
        expiresAt = null;
        currentUser = anonymousUser;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
    }
}
