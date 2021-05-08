using BlazorCore.Enums;
using BlazorCore.Interfaces;
using BlazorCore.Models;
using BlazorCore.Models.Requests;
using BlazorCore.Models.Users;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlazorCore.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
	{
		private readonly IRequestSenderService _requestSenderService;
		private readonly AuthStateProvider _authStateProvider;
		private readonly ILocalStorageService _localStorage; 

		public AuthenticationService(IRequestSenderService requestSenderService, AuthenticationStateProvider authStateProvider,
			ILocalStorageService localStorage)
		{
			_requestSenderService = requestSenderService;
			_authStateProvider = (AuthStateProvider)authStateProvider;
			_localStorage = localStorage;
		}

		public async Task<ApiResponse<T>> RegisterUserAsync<T>(RegistrationRequestModel requestModel) where T : IAccessModel
		{
			var result = await _requestSenderService
				.SendAsync<ApiResponse<T>>(HttpMethod.Post, "https://dev-auth.op.today/v1/identity/registration", requestModel);

			if (result.Success)
			{
				await SetAuthDataAsync(result.Data, requestModel.DeviceId);
			}

			return result;
		}

		public async Task<ApiResponse<T>> LoginAsync<T>(string login, string code) where T : IAccessModel
		{
			LoginRequestModel requestModel = new LoginRequestModel()
			{
				ClientType = ClientType.Web,
				DeviceId = Guid.NewGuid().ToString(),
				ConfirmationCode = code,
				Email = login
			};
			var result = await _requestSenderService
				.SendAsync<ApiResponse<T>>(HttpMethod.Post, "https://dev-auth.op.today/v1/identity/login", requestModel);

			if (result.Success)
			{
				await SetAuthDataAsync(result.Data, requestModel.DeviceId);
			}

			return result;
		}

		public async Task<ApiResponse<object>> SendConfirmationCodeAsync(ConfirmationCodeRequestModel requestModel)
		{
			var result = await _requestSenderService
				.SendAsync<ApiResponse<object>>(HttpMethod.Post, "https://dev-auth.op.today/v1/identity/send-code", requestModel);
			return result;
		}

		public async Task<ApiResponse<object>> LogoutAsync()
		{
			await _localStorage.RemoveItemAsync(LocalStorageKey.AuthToken.ToString());
			await _localStorage.RemoveItemAsync(LocalStorageKey.RefreshToken.ToString());
			await _localStorage.RemoveItemAsync(LocalStorageKey.User.ToString());
			_authStateProvider.NotifyUserLogout();
			return new ApiResponse<object>(System.Net.HttpStatusCode.OK);
		}

		private async Task SetAuthDataAsync(IAccessModel data, string deviceId)
        {
			var authToken = data?.GetAuthToken();
            if (!string.IsNullOrWhiteSpace(authToken))
            {
				await _localStorage.SetItemAsync(LocalStorageKey.AuthToken.ToString(), authToken);
				_requestSenderService.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);
				_authStateProvider.NotifyUserAuthentication(authToken);
			}
			
			var refreshToken = data?.GetRefreshToken();
			if (!string.IsNullOrWhiteSpace(refreshToken))
			{
				await _localStorage.SetItemAsync(LocalStorageKey.RefreshToken.ToString(), refreshToken);
				await _localStorage.SetItemAsync(LocalStorageKey.DeviceId.ToString(), deviceId);
			}

			if(data is LoginModel)
            {
				UserModel userModel = ((LoginModel)data).User;
				await _localStorage.SetItemAsync(LocalStorageKey.User.ToString(), userModel);
			}
		}
	}
}
