using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorCore.Interfaces
{
    public interface IRequestSenderService
    {
        HttpClient Client { get; }
        Task<TResponse> SendAsync<TResponse>(HttpMethod method, string url, object data, string token = null);
    }
}
