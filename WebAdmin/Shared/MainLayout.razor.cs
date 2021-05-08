using BlazorCore.Enums;
using BlazorCore.Services.Auth;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace WebAdmin.Shared
{
    public partial class MainLayout : IDisposable
    {
        [Inject]
        private ILocalStorageService _localStorage { get; set; }
        [Inject]
        private InterceptorService _interceptor { get; set; }
        [Inject]
        private RefreshTokenService _refreshTokenService { get; set; }
        [Inject]
        private NavigationManager _navigationManager { get; set; }
        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [CascadingParameter]
        public Error Error { get; set; }

        public bool isLeftMenuOpened = false;
        public bool isAuthorizedUser = false;

        protected override async Task OnInitializedAsync()
        {
            _interceptor.RegisterEvent();
            _authenticationStateProvider.AuthenticationStateChanged += AuthenticationStateChangedHandler;
            if (await _localStorage.ContainKeyAsync(LocalStorageKey.IsLeftMenuOpened.ToString()))
            {
                isLeftMenuOpened = await _localStorage.GetItemAsync<bool>(LocalStorageKey.IsLeftMenuOpened.ToString());
            }
            try
            {
                var token = await _refreshTokenService.TryRefreshTokenAsync<BlazorCore.Models.LoginModel>();
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new ApplicationException("Unauthorized");
                }
                isAuthorizedUser = true;
            }
            catch (Exception ex)
            {
                isAuthorizedUser = false;
                Error.ProcessError(ex);
            }
        }

        private async void AuthenticationStateChangedHandler(Task<AuthenticationState> task)
        {
            var state = await task;
            isAuthorizedUser = ((AuthStateProvider)_authenticationStateProvider).IsAuthorized(state);
            _navigationManager.NavigateTo("/");
        }

        public async void TuggleMenu()
        {
            isLeftMenuOpened = !isLeftMenuOpened;
            await _localStorage.SetItemAsync<bool>(LocalStorageKey.IsLeftMenuOpened.ToString(), isLeftMenuOpened);
        }

        private async Task OnProfileClickAsync()
        {
            _navigationManager.NavigateTo("/profile");
        }

        private async Task OnMenuItemClick(MouseEventArgs e)
        {
            //IsLeftMenuOpened = false;
            //await _localStorage.SetItemAsync<bool>(LocalStorageKey.IsLeftMenuOpened.ToString(), IsLeftMenuOpened);
        }

        public void Dispose() => _interceptor.DisposeEvent();
    }
}
