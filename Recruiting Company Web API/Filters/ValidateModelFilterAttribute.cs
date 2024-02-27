using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Recruiting_Company_Web_API.Filters
{
	public class ValidateModelFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				context.Result = new BadRequestObjectResult($"{context.ModelState.First().Key}: " +
					$"{context.ModelState.First().Value}");
			}
		}
	}
}