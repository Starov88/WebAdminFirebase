using BlazorCore.Models;
using BlazorCore.Models.Requests;
using System.Threading.Tasks;

namespace BlazorCore.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<T>> RegisterUserAsync<T>(RegistrationRequestModel userForRegistration) where T : IAccessModel;
        Task<ApiResponse<T>> LoginAsync<T>(string login, string code) where T : IAccessModel;
        Task<ApiResponse<object>> SendConfirmationCodeAsync(ConfirmationCodeRequestModel requestModel);
        Task<ApiResponse<object>> LogoutAsync();
    }
}
