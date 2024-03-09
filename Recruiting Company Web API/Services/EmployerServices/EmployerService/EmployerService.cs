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

		public async Task<Employer?> GetEmployerAsync(string name)
		{
			return await _userManager.FindByNameAsync(name);
		}

		public async Task<List<Vacancy>> GetVacanciesAsync(Employer employer)
		{
			return await _context.Vacancies.Include(v => v.Employer).Where(v => v.Employer.Id == employer.Id).ToListAsync();
		}

		public async Task<Vacancy> AddVacancyAsync(VacancyModel model, Employer employer)
		{
			var category = await _context.Categories.FindAsync(model.CategoryID)
				?? throw new Exception("Category is null");
			var vacancy = new Vacancy
			{
				CreateDate = DateTime.Now,
				Category = category,
				Title = model.Title,
				Salary = model.Salary,
				PhoneNumber = model.PhoneNumber,
				EMail = model.EMail,
				Description = model.Description,
				Employer = employer
			};
			await _context.Vacancies.AddAsync(vacancy);
			await _context.SaveChangesAsync();
			return vacancy;
		}
	}
}
