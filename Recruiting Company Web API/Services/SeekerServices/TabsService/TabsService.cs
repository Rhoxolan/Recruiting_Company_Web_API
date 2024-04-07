using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.TabsService
{
	public class TabsService(UserManager<Seeker> userManager, ApplicationContext context) : ITabsService
	{
		public async Task<(bool, bool)> AddVacansyToTabAsync(TabModel model, string name)
		{
			bool findUserResult;
			bool findVacancyResult = false;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				var vacancy = await context.Vacancies.FindAsync(model.VacancyId);
				if (findVacancyResult = vacancy != null)
				{
					var tab = new SeekerTab
					{
						Seeker = seeker!,
						SeekerID = seeker!.Id,
						Vacancy = vacancy!,
						VacancyID = vacancy!.Id
					};
					await context.SeekersTabs.AddAsync(tab);
					await context.SaveChangesAsync();
				}
			}
			return (findUserResult, findVacancyResult);
		}

		public async Task<(bool, IEnumerable<dynamic>?)> GetTabsAsync(string name)
		{
			bool findUserResult;
			IEnumerable<dynamic>? tabs = null;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				tabs = await context.SeekersTabs
					.Where(t => t.SeekerID == seeker!.Id)
					.OrderBy(t => t.Vacancy.CreateDate)
					.Select(t => new
					{
						t.Vacancy.Id,
						t.Vacancy.CategoryID,
						EmployerID = t.Vacancy.Employer.PublicId,
						Employer = t.Vacancy.Employer.CompanyName,
						t.Vacancy.CreateDate,
						t.Vacancy.Title,
						t.Vacancy.Location,
						t.Vacancy.Salary,
						t.Vacancy.PhoneNumber,
						t.Vacancy.EMail,
						t.Vacancy.Description
					}).ToListAsync();
			}
			return (findUserResult, tabs);
		}

		public async Task<(bool, bool, bool)> DeleteTabAsync(ulong id, string name)
		{
			bool findUserResult;
			bool findVacancyResult = false;
			bool findTabResult = false;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				var vacancy = await context.Vacancies.FindAsync(id);
				if (findVacancyResult = vacancy != null)
				{
					var tab = await context.SeekersTabs
						.Where(t => t.SeekerID == seeker!.Id)
						.Where(t => t.VacancyID == vacancy!.Id).FirstOrDefaultAsync();
					if (findTabResult = tab != null)
					{
						context.SeekersTabs.Remove(tab!);
						await context.SaveChangesAsync();
					}
				}
			}
			return (findUserResult, findVacancyResult, findTabResult);
		}

		public async Task<(bool, bool)> CheckIsNotedAsync(ulong vacancyId, string name)
		{
			bool findUserResult;
			bool isNoted = false;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				isNoted = await context.SeekersTabs
					.Where(t => t.SeekerID == seeker!.Id)
					.Where(t => t.VacancyID == vacancyId)
					.AnyAsync();
			}
			return (findUserResult, isNoted);
		}
	}
}
