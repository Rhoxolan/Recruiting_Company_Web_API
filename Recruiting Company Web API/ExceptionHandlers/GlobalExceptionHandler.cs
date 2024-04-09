using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Recruiting_Company_Web_API.ExceptionHandlers
{
	public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			var problemDetails = new ProblemDetails
			{
				Detail = "Error. Please contact to developer"
			};
			var problemDetailsContext = new ProblemDetailsContext
			{
				HttpContext = httpContext,
				ProblemDetails = problemDetails
			};
			if (!await problemDetailsService.TryWriteAsync(problemDetailsContext))
			{
				problemDetails.Status = StatusCodes.Status500InternalServerError;
				problemDetails.Title = "An error occurred while processing your request.";
				httpContext.Response.StatusCode = problemDetails.Status.Value;
				await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
			}
			return await ValueTask.FromResult(true);
		}
	}
}
