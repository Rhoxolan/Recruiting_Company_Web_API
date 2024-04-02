using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.TabsService
{
	public class TabsService : ITabsService
	{
		private readonly UserManager<Seeker> _userManager;
		private readonly ApplicationContext _context;

		public TabsService(UserManager<Seeker> userManager, ApplicationContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public async Task<(bool, bool)> AddVacansyToTabAsync(TabModel model, string name)
		{
			bool findUserResult;
			bool findVacancyResult = false;
			var seeker = await _userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				var vacancy = await _context.Vacancies.FindAsync(model.VacancyId);
				if (findVacancyResult = vacancy != null)
				{
					var tab = new SeekerTab
					{
						Seeker = seeker!,
						SeekerID = seeker!.Id,
						Vacancy = vacancy!,
						VacancyID = vacancy!.Id
					};
					await _context.SeekersTabs.AddAsync(tab);
					await _context.SaveChangesAsync();
				}
			}
			return (findUserResult, findVacancyResult);
		}

		public async Task<(bool, IEnumerable<dynamic>?)> GetTabsAsync(string name)
		{
			bool findUserResult;
			IEnumerable<dynamic>? tabs = null;
			var seeker = await _userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				tabs = await _context.SeekersTabs
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
			var seeker = await _userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				var vacancy = await _context.Vacancies.FindAsync(id);
				if (findVacancyResult = vacancy != null)
				{
					var tab = await _context.SeekersTabs
						.Where(t => t.SeekerID == seeker!.Id)
						.Where(t => t.VacancyID == vacancy!.Id).FirstOrDefaultAsync();
					if (findTabResult = tab != null)
					{
						_context.SeekersTabs.Remove(tab!);
						await _context.SaveChangesAsync();
					}
				}
			}
			return (findUserResult, findVacancyResult, findTabResult);
		}
	}
}
