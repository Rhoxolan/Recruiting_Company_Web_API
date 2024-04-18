using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Infrastructure;
using Recruiting_Company_Web_API.Models.EmployerModels;
using Recruiting_Company_Web_API.Services.EmployerServices.EmployerService;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class EmployerController(IEmployerService employerService) : RecruitingCompanyController
	{
		[HttpGet("Vacancies")]
		public async Task<IActionResult> GetVacancies()
		{
			var result = await employerService.GetVacanciesAsync(UserName);
			return ProcessResult(result, () => Ok(new { vacancies = result.Value }));
		}

		[HttpPost("AddVacancy")]
		public async Task<IActionResult> AddVacancy(VacancyModel model)
		{
			var result = await employerService.AddVacancyAsync(model, UserName);
			return ProcessResult(result, () => Ok(new { vacancy = result.Value }));
		}

		[HttpPatch("EditVacancy")]
		public async Task<IActionResult> EditVacancy(VacancyModel model)
		{
			var result = await employerService.EditVacansyAsync(model, UserName);
			return ProcessResult(result, () => Ok(new { vacancy = result.Value }));
		}

		[HttpDelete("DeleteVacancy/{id}")]
		public async Task<IActionResult> DeleteVacancy(ulong id)
		{
			var result = await employerService.DeleteVacancyAsync(id, UserName);
			return ProcessResult(result, Ok);
		}

		[HttpGet("Responses/{id}")]
		public async Task<IActionResult> GetResponses(ulong id)
		{
			var result = await employerService.GetVacancyResponsesAsync(id, UserName);
			return ProcessResult(result, () => Ok(new { responses = result.Value }));
		}

		[HttpGet("ResponseCVFile/{id}")]
		public async Task<IActionResult> GetResponseCVFile(ulong id)
		{
			var result = await employerService.GetVacancyResponseCVFileAsync(id, UserName);
			return ProcessResult(result, () => Ok(new { responseCVFile = result.Value }));
		}
	}
}
