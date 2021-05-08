using BlazorCore.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace BlazorCore.Services.Auth
{
    public class InterceptorService
	{
		private readonly HttpClientInterceptor _interceptor;
		private readonly RefreshTokenService _tokenService;

		public InterceptorService(HttpClientInterceptor interceptor, RefreshTokenService tokenService)
		{
			_interceptor = interceptor;
			_tokenService = tokenService;
		}

		public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
		public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;

		public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
		{
			var absolutePath = e.Request.RequestUri.AbsolutePath;
			e.Request.Headers.TryAddWithoutValidation("Access-Control-Allow-Origin", "*");

			if (!absolutePath.Contains("identity/") ||
				absolutePath.Contains("identity/logout") || absolutePath.Contains("identity/registration"))
			{
				var token = await _tokenService.TryRefreshTokenAsync<LoginModel>();

				if (!string.IsNullOrWhiteSpace(token))
				{
					e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
				}
			}
		}
	}
}
