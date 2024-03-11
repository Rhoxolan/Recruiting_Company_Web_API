using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Models.EmployerModels;
using Recruiting_Company_Web_API.Services.EmployerServices.EmployerService;
using System.Security.Claims;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class EmployerController : ControllerBase
	{
		private readonly IEmployerService _employerService;

		public EmployerController(IEmployerService employerService)
		{
			_employerService = employerService;
		}

		[HttpGet("Vacancies")]
		public async Task<IActionResult> GetVacancies()
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, vacancies) = await _employerService.GetVacanciesAsync(userNameClaim!.Value);
				if(!findUserResult || vacancies == null)
				{
					return BadRequest();
				}
				return Ok(new { vacancies = vacancies.Select(v => new { v.Id, CategoryID = v.Category.Id, v.CreateDate,
					v.Title, v.Salary, v.PhoneNumber, v.EMail, v.Description })});
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpPost("AddVacancy")]
		public async Task<IActionResult> AddVacancy(VacancyModel model)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, vacancy) = await _employerService.AddVacancyAsync(model, userNameClaim!.Value);
				if (!findUserResult || vacancy == null)
				{
					return BadRequest();
				}
				return Ok(new { vacancy = new { vacancy.Id, CategoryID = vacancy.Category.Id, vacancy.CreateDate,
					vacancy.Title, vacancy.Salary, vacancy.PhoneNumber, vacancy.EMail, vacancy.Description } });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpPatch("EditVacansy")]
		public async Task<IActionResult> EditVacancy(VacancyModel model)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, vacancy) = await _employerService.EditVacansyAsync(model, userNameClaim!.Value);
				if (!findUserResult)
				{
					return BadRequest();
				}
				if (vacancy == null)
				{
					return NotFound();
				}
				return Ok(new { vacancy = new { vacancy.Id, CategoryID = vacancy.Category.Id, vacancy.CreateDate,
					vacancy.Title, vacancy.Salary, vacancy.PhoneNumber, vacancy.EMail, vacancy.Description } });
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}

		[HttpDelete("DeleteVacancy")]
		public async Task<IActionResult> DeleteVacancy(ulong id)
		{
			try
			{
				var userNameClaim = User.FindFirst(ClaimTypes.Name);
				var (findUserResult, findVacancyResult) = await _employerService.DeleteVacansyAsync(id, userNameClaim!.Value);
				if (!findUserResult)
				{
					return BadRequest();
				}
				if (!findVacancyResult)
				{
					return NotFound();
				}
				return Ok();
			}
			catch
			{
				return Problem("Error. Please contact to developer");
			}
		}
	}
}
