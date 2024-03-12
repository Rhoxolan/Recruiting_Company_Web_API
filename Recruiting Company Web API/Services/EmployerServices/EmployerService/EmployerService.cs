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

		public async Task<(bool, IEnumerable<object>?)> GetVacanciesAsync(string name)
		{
			bool findUserResult;
			IEnumerable<object>? vacancies = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				vacancies = await _context.Vacancies
					.Include(v => v.Category)
					.Where(v => v.Employer.Id == employer!.Id)
					.Select(v => new
					{
						v.Id,
						CategoryID = v.Category.Id,
						v.CreateDate,
						v.Title,
						v.Salary,
						v.PhoneNumber,
						v.EMail,
						v.Description
					})
					.ToListAsync();
			}
			return (findUserResult, vacancies);
		}

		public async Task<(bool, object?)> AddVacancyAsync(VacancyModel model, string name)
		{
			bool findUserResult;
			object? vacancy = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				var category = await _context.Categories.FindAsync(model.CategoryID)
					?? throw new Exception("Category is null");
				var vacancyEntity = new Vacancy
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
				await _context.Vacancies.AddAsync(vacancyEntity);
				await _context.SaveChangesAsync();
				vacancy = new
				{
					vacancyEntity.Id,
					CategoryID = vacancyEntity.Category.Id,
					vacancyEntity.CreateDate,
					vacancyEntity.Title,
					vacancyEntity.Salary,
					vacancyEntity.PhoneNumber,
					vacancyEntity.EMail,
					vacancyEntity.Description
				};
			}
			return (findUserResult, vacancy);
		}

		public async Task<(bool, object?)> EditVacansyAsync(VacancyModel model, string name)
		{
			_ = model.Id ?? throw new Exception("Id is null");
			bool findUserResult;
			object? vacancy = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				var category = await _context.Categories.FindAsync(model.CategoryID)
					?? throw new Exception("Category is null");
				var vacancyEntity = await _context.Vacancies
					.Include(v => v.Category)
					.Include(v => v.Employer)
					.Where(v => v.Employer.Id == employer!.Id)
					.Where(v => v.Id == model.Id)
					.FirstOrDefaultAsync();
				if (vacancyEntity != null)
				{
					vacancyEntity.Title = model.Title;
					vacancyEntity.Location = model.Location;
					vacancyEntity.Category = category;
					vacancyEntity.Salary = model.Salary;
					vacancyEntity.PhoneNumber = model.PhoneNumber;
					vacancyEntity.EMail = model.EMail;
					vacancyEntity.Description = model.Description;
					await _context.SaveChangesAsync();
					vacancy = new
					{
						vacancyEntity.Id,
						CategoryID = vacancyEntity.Category.Id,
						vacancyEntity.CreateDate,
						vacancyEntity.Title,
						vacancyEntity.Salary,
						vacancyEntity.PhoneNumber,
						vacancyEntity.EMail,
						vacancyEntity.Description
					};
				}
			}
			return (findUserResult, vacancy);
		}

		public async Task<(bool, bool)> DeleteVacancyAsync(ulong id, string name)
		{
			bool findUserResult;
			bool findVacancyResult = false;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				var vacancy = await _context.Vacancies.FindAsync(id);
				if (findVacancyResult = vacancy != null)
				{
					_context.Vacancies.Remove(vacancy!);
					await _context.SaveChangesAsync();
				}
			}
			return (findUserResult, findVacancyResult);
		}

		public async Task<(bool, bool, IEnumerable<object>?)> GetVacancyResponses(ulong id, string name)
		{
			bool findUserResult;
			bool findVacancyResult = false;
			IEnumerable<object>? responses = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				var vacancy = await _context.Vacancies.FindAsync(id);
				if (findVacancyResult = vacancy != null)
				{
					responses = await _context.Responses
						.Include(r => r.Vacancy)
						.Include(r => r.CV)
						.Where(r => r.Vacancy.Id == id)
						.Select(r => new
						{
							r.Id,
							r.ResponseTime
							//...
						})
						.ToListAsync();
				}
			}
			return (findUserResult, findVacancyResult, responses);
		}
	}
}
