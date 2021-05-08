using BlazorCore.Exceptions;
using BlazorCore.Interfaces;
using BlazorCore.Models;
using IdentityModel.Client;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorCore.Services
{
    public class RequestSenderService : IRequestSenderService
    {
        private readonly JsonSerializerOptions _options;

        public HttpClient Client { get; private set; }

        public RequestSenderService(HttpClient client)
        {
            Client = client;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _options.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string url, object data, string token = null)
        {
            var requestMessage = GetRequestMessage(method, url, data);
            return await SendRequestAsync<TResponse>(requestMessage, token);
        }

        private async Task<TResponse> SendRequestAsync<TResponse>(HttpRequestMessage httpRequest, string token = null)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                Client.SetBearerToken(token);
            }

            TResponse responseData = default;
            using (var response = await Client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    if (response.Content != null)
                    {
                        var stream = await response.Content.ReadAsStreamAsync();
                        responseData = await JsonSerializer.DeserializeAsync<TResponse>(stream, _options);
                    }
                }
                else
                {
                    await RequestErrorHandlerAsync(response);
                }
            }
            return responseData;
        }

        private HttpRequestMessage GetRequestMessage(HttpMethod method, string url, object data, string contentType = "application/json")
        {
            var requestMessage = new HttpRequestMessage(method, url);
            if (data != null)
            {
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, contentType);
            }
            return requestMessage;
        }

        private async Task RequestErrorHandlerAsync(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode)
            {
                string errorMessage = httpResponse.ReasonPhrase;
                var statusCode = httpResponse.StatusCode;
                try
                {
                    var stream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseData = await JsonSerializer.DeserializeAsync<ApiResponse<object>>(stream, _options);
                    statusCode = (HttpStatusCode)responseData.StatusCode;
                    errorMessage = responseData.Message;
                }
                catch { }
                throw new AppException(errorMessage, statusCode);
            }
        }
    }
}
