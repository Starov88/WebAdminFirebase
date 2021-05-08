using BlazorCore.Enums;
using BlazorCore.Interfaces;
using BlazorCore.Models;
using BlazorCore.Models.Requests;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlazorCore.Services.Auth
{
    public class RefreshTokenService
    {
		private readonly IRequestSenderService _requestSenderService;
		private readonly AuthStateProvider _authStateProvider;
		private readonly ILocalStorageService _localStorage;

		public RefreshTokenService(IRequestSenderService requestSenderService, AuthenticationStateProvider authStateProvider,
			ILocalStorageService localStorage)
		{
			_requestSenderService = requestSenderService;
			_authStateProvider = (AuthStateProvider)authStateProvider;
			_localStorage = localStorage;
		}

		public async Task<string> TryRefreshTokenAsync<T>() where T : IAccessModel
		{
			string authToken = null;
			var authState = await _authStateProvider.GetAuthenticationStateAsync();
			if(authState != null && authState.User != null)
            {
				var exp = authState.User.FindFirst(c => c.Type.Equals("exp"))?.Value;
				if(long.TryParse(exp, out var seconds))
                {
					var expTime = DateTimeOffset.FromUnixTimeSeconds(seconds);
					var dateNow = DateTimeOffset.UtcNow;
					var diff = expTime - dateNow;
					if (diff.TotalMinutes <= 2)
					{
						authToken = await RefreshTokenAsync<T>();
					}
                    else
                    {
						authToken = await _localStorage.GetItemAsync<string>(LocalStorageKey.AuthToken.ToString());
					}
				}
			}
			
			return authToken;
		}

		private async Task<string> RefreshTokenAsync<T>() where T : IAccessModel
		{
			var refreshToken = await _localStorage.GetItemAsync<string>(LocalStorageKey.RefreshToken.ToString());
			if (string.IsNullOrWhiteSpace(refreshToken))
			{
				throw new ApplicationException("Something went wrong during the refresh token action");
			}
			var deviceId = await _localStorage.GetItemAsync<string>(LocalStorageKey.DeviceId.ToString());
			if (string.IsNullOrWhiteSpace(deviceId))
			{
				throw new ApplicationException("Something went wrong during the refresh token action");
			}

			var requestModel = new RefreshTokenRequestModel()
			{
				Token = refreshToken,
				ClientType = ClientType.Web,
				DeviceId = deviceId
			};

			var result = await _requestSenderService
				.SendAsync<ApiResponse<T>>(HttpMethod.Post, "https://dev-auth.op.today/v1/identity/login/refresh", requestModel);

			if (!result.Success)
				throw new ApplicationException("Something went wrong during the refresh token action");

			var authToken = result.Data.GetAuthToken();

			await _localStorage.SetItemAsync(LocalStorageKey.AuthToken.ToString(), authToken);
			await _localStorage.SetItemAsync(LocalStorageKey.RefreshToken.ToString(), result.Data.GetRefreshToken());

			_requestSenderService.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);
			_authStateProvider.NotifyUserAuthentication(result.Data.GetAuthToken());

			return authToken;
		}
	}
}
