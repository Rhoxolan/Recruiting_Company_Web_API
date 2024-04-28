using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.DTOs.VacancyDTOs;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.TabsService
{
	public class TabsService(UserManager<Seeker> userManager, ApplicationContext context) : ITabsService
	{
		public async Task<ServiceResult> AddVacansyToTabAsync(TabModel model, string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancy = await context.Vacancies.FindAsync(model.VacancyId);
			if (vacancy == null)
			{
				return ServiceResult.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
			}
			var tab = new SeekerTab
			{
				Seeker = seeker,
				SeekerID = seeker.Id,
				Vacancy = vacancy,
				VacancyID = vacancy.Id
			};
			await context.SeekersTabs.AddAsync(tab);
			await context.SaveChangesAsync();
			return ServiceResult.Success();
		}

		public async Task<ServiceResult<List<CommonVacancyDTO>>> GetTabsAsync(string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<List<CommonVacancyDTO>>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var tabs = await context.SeekersTabs
				.Where(t => t.SeekerID == seeker.Id)
				.OrderBy(t => t.Vacancy.CreateDate)
				.Select(t => new CommonVacancyDTO
				{
					Id = t.Vacancy.Id,
					CategoryID = t.Vacancy.CategoryID,
					EmployerID = t.Vacancy.Employer.PublicId,
					Employer = t.Vacancy.Employer.CompanyName,
					CreateDate = t.Vacancy.CreateDate,
					Title = t.Vacancy.Title,
					Location = t.Vacancy.Location,
					Salary = t.Vacancy.Salary,
					PhoneNumber = t.Vacancy.PhoneNumber,
					EMail = t.Vacancy.EMail,
					Description = t.Vacancy.Description
				}).ToListAsync();
			return ServiceResult<List<CommonVacancyDTO>>.Success(tabs);
		}

		public async Task<ServiceResult> DeleteTabAsync(ulong id, string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancy = await context.Vacancies.FindAsync(id);
			if (vacancy == null)
			{
				return ServiceResult.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
			}
			var tab = await context.SeekersTabs
				.Where(t => t.SeekerID == seeker.Id)
				.Where(t => t.VacancyID == vacancy.Id).FirstOrDefaultAsync();
			if (tab == null)
			{
				return ServiceResult.Failure(ServiceErrorType.EntityNotFound, "Tab not found!");
			}
			context.SeekersTabs.Remove(tab);
			await context.SaveChangesAsync();
			return ServiceResult.Success();
		}

		public async Task<ServiceResult<bool>> CheckIsNotedAsync(ulong vacancyId, string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<bool>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var vacancy = await context.Vacancies.FindAsync(vacancyId);
			if (vacancy == null)
			{
				return ServiceResult<bool>.Failure(ServiceErrorType.EntityNotFound, "Vacancy not found!");
			}
			var isNoted = await context.SeekersTabs
				.Where(t => t.SeekerID == seeker.Id)
				.Where(t => t.VacancyID == vacancy.Id)
				.AnyAsync();
			return ServiceResult<bool>.Success(isNoted);
		}
	}
}
