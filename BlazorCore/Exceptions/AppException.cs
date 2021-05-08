using BlazorCore.Models;
using System;
using System.Net;

namespace BlazorCore.Exceptions
{
    public class AppException : Exception
	{
		/// <summary>
		/// Response http status code
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }

		public AppException(ApiResponse<object> apiResponse) : this(apiResponse?.Message, (HttpStatusCode)apiResponse.StatusCode)
		{
		}

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="errorMessage">Error message</param>
		/// <param name="httpStatusCode">Response http status code</param>
		public AppException(string errorMessage, HttpStatusCode httpStatusCode)
			: base(errorMessage)
		{
			StatusCode = httpStatusCode;
		}

		/// <summary>
		///     ctor
		/// </summary>
		/// <param name="errorMessage">Error message</param>
		/// <param name="httpStatusCode">Response http status code</param>
		/// <param name="exception">Main exception</param>
		public AppException(string errorMessage, HttpStatusCode httpStatusCode, Exception exception)
			: base(errorMessage, exception)
		{
			StatusCode = httpStatusCode;
		}
	}
}
