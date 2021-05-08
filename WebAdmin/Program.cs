using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using BlazorCore.Interfaces;
using BlazorCore.Services;
using BlazorCore.Services.Auth;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace WebAdmin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            }
            .EnableIntercept(sp));

            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<IRequestSenderService, RequestSenderService>();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<RefreshTokenService>();
            builder.Services.AddScoped<InterceptorService>();
            
            builder.Services.AddSingleton<NotifyService>();

            await builder.Build().RunAsync();
        }
    }
}
//dotnet watch -p C:\My\Projects\WebAdmin\WebAdmin run