using BlazorCore.Enums;
using BlazorCore.Helpers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorCore.Services.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(LocalStorageKey.AuthToken.ToString());
            if (string.IsNullOrWhiteSpace(token))
                return _anonymous;

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
        }

        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }

        public bool IsAuthorized(AuthenticationState authenticationState)
        {
            if (authenticationState != null && authenticationState.User != null)
            {
                var exp = authenticationState.User.FindFirst(c => c.Type.Equals("exp"))?.Value;
                if (long.TryParse(exp, out var seconds))
                {
                    var expTime = DateTimeOffset.FromUnixTimeSeconds(seconds);
                    var dateNow = DateTimeOffset.UtcNow;
                    var diff = expTime - dateNow;
                    if (diff.TotalMinutes > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
