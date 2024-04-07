using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;
using Recruiting_Company_Web_API.Services.GuestServices.GuestService;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GuestController(IGuestService guestService) : ControllerBase
	{
		[HttpGet("Vacancy/{id}")]
		public async Task<IActionResult> GetVacancy(ulong id)
		{
			var vacancy = await guestService.GetVacancyAsync(id);
			if (vacancy == null)
			{
				return NotFound();
			}
			return Ok(new { vacancy });
		}

		[HttpGet("VacanciesCount")]
		public async Task<IActionResult> GetVacanciesCount([FromQuery] VacancyRequestParameters requestParameters)
		{
			int count = await guestService.GetVacanciesCountAsync(requestParameters);
			return Ok(new { count });
		}

		[HttpGet("Vacancies")]
		public async Task<IActionResult> GetVacancies([FromQuery] VacancyRequestParameters requestParameters)
		{
			var vacancies = await guestService.GetVacanciesAsync(requestParameters);
			return Ok(new { vacancies });
		}
	}
}
