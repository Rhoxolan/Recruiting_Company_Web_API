using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Recruiting_Company_Web_API.ExceptionHandlers
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			var problemDetails = new ProblemDetails
			{
				Detail = "Error. Please contact to developer",
				Status = StatusCodes.Status500InternalServerError,
				Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
				Title = "An error occurred while processing your request."
			};
			httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
			return await ValueTask.FromResult(true);
		}
	}
}
