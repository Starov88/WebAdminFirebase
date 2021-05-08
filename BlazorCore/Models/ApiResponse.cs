using System.Net;

namespace BlazorCore.Models
{
	public class ApiResponse<T>
	{
		public int StatusCode { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }

		public ApiResponse()
		{

		}

		public ApiResponse(HttpStatusCode statusCode, string message = null, T data = default)
		{
			Success = statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.NoContent ? true : false;
			StatusCode = (int)statusCode;
			Data = Success ? data : default;
			Message = message;
		}
	}
}
