using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GuestController : ControllerBase
	{

		[HttpGet]
		public async Task<IActionResult> GetVacancies(VacancyRequestParameters requestParameters)
		{
			try
			{
				return Ok();
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}
	}
}
