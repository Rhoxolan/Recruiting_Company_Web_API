using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.EmployerModels;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public class EmployerService : IEmployerService
	{
		private readonly UserManager<Employer> _userManager;
		private readonly ApplicationContext _context;

		public EmployerService(ApplicationContext context, UserManager<Employer> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<(bool, List<Vacancy>?)> GetVacanciesAsync(string name)
		{
			bool findUserResult;
			List<Vacancy>? vacancies = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				vacancies = await _context.Vacancies
					.Include(v => v.Category)
					.Where(v => v.Employer.Id == employer!.Id)
					.ToListAsync();
			}
			return (findUserResult, vacancies);
		}

		public async Task<(bool, Vacancy?)> AddVacancyAsync(VacancyModel model, string name)
		{
			bool findUserResult;
			Vacancy? vacancy = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				var category = await _context.Categories.FindAsync(model.CategoryID)
					?? throw new Exception("Category is null");
				vacancy = new Vacancy
				{
					CreateDate = DateTime.Now,
					Category = category,
					Title = model.Title,
					Location = model.Location,
					Salary = model.Salary,
					PhoneNumber = model.PhoneNumber,
					EMail = model.EMail,
					Description = model.Description,
					Employer = employer!
				};
				await _context.Vacancies.AddAsync(vacancy);
				await _context.SaveChangesAsync();
			}
			return (findUserResult, vacancy);
		}
	}
}
