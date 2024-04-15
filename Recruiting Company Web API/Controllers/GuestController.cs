using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Infrastructure;
using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;
using Recruiting_Company_Web_API.Services.GuestServices.GuestService;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GuestController(IGuestService guestService) : RecruitingCompanyController
	{
		[HttpGet("Vacancy/{id}")]
		public async Task<IActionResult> GetVacancy(ulong id)
		{
			var result = await guestService.GetVacancyAsync(id);
			return ProcessResult(result, () => Ok(new { vacancy = result.Value }));
		}

		[HttpGet("VacanciesCount")]
		public async Task<IActionResult> GetVacanciesCount([FromQuery] VacancyRequestParameters requestParameters)
		{
			var result = await guestService.GetVacanciesCountAsync(requestParameters);
			return ProcessResult(result, () => Ok(new { count = result.Value }));
		}

		[HttpGet("Vacancies")]
		public async Task<IActionResult> GetVacancies([FromQuery] VacancyRequestParameters requestParameters)
		{
			var result = await guestService.GetVacanciesAsync(requestParameters);
			return ProcessResult(result, () => Ok(new { vacancies = result.Value }));
		}
	}
}
