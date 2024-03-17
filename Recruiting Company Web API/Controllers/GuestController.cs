using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;
using Recruiting_Company_Web_API.Services.GuestServices.GuestService;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GuestController : ControllerBase
	{
		private readonly IGuestService _guestService;

		public GuestController(IGuestService guestService)
		{
			_guestService = guestService;
		}

		[HttpGet("VacanciesCount")]
		public async Task<IActionResult> GetVacanciesCount([FromQuery]VacancyRequestParameters requestParameters)
		{
			try
			{
				int count = await _guestService.GetVacanciesCountAsync(requestParameters);
				return Ok(new { count });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpGet("Vacancies")]
		public async Task<IActionResult> GetVacancies([FromQuery]VacancyRequestParameters requestParameters)
		{
			try
			{
				var vacancies = await _guestService.GetVacanciesAsync(requestParameters);
				return Ok(new { vacancies });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}
	}
}
