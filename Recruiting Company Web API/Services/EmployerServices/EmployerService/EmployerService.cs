using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.EmployerModels;
using System.Text;

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

		public async Task<(bool, IEnumerable<dynamic>?)> GetVacanciesAsync(string name)
		{
			bool findUserResult;
			IEnumerable<dynamic>? vacancies = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				vacancies = await GetVacancies(employer!.Id)
					.Select(v => new
					{
						v.Id,
						v.CategoryID,
						v.CreateDate,
						v.Title,
						v.Location,
						v.Salary,
						v.PhoneNumber,
						v.EMail,
						v.Description
					}).ToListAsync();
			}
			return (findUserResult, vacancies);
		}

		public async Task<(bool, dynamic?)> AddVacancyAsync(VacancyModel model, string name)
		{
			bool findUserResult;
			dynamic? vacancy = null;
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

		public async Task<(bool, dynamic?)> EditVacansyAsync(VacancyModel model, string name)
		{
			ulong vacancyId = model.Id ?? throw new Exception("Id is null");
			bool findUserResult;
			dynamic? vacancy = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				var category = await _context.Categories.FindAsync(model.CategoryID)
					?? throw new Exception("Category is null");
				var vacancyEntity = await GetVacancies(employer!.Id, vacancyId).FirstOrDefaultAsync();
				if (vacancyEntity != null)
				{
					vacancyEntity.Title = model.Title;
					vacancyEntity.Location = model.Location;
					vacancyEntity.Category = category;
					vacancyEntity.CategoryID = category.Id;
					vacancyEntity.Salary = model.Salary;
					vacancyEntity.PhoneNumber = model.PhoneNumber;
					vacancyEntity.EMail = model.EMail;
					vacancyEntity.Description = model.Description;
					await _context.SaveChangesAsync();
					vacancy = new
					{
						vacancyEntity.Id,
						vacancyEntity.CategoryID,
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
				var vacancy = await GetVacancies(employer!.Id, id).FirstOrDefaultAsync();
				if (findVacancyResult = vacancy != null)
				{
					_context.Vacancies.Remove(vacancy!);
					await _context.SaveChangesAsync();
				}
			}
			return (findUserResult, findVacancyResult);
		}

		public async Task<(bool, bool, IEnumerable<dynamic>?)> GetVacancyResponsesAsync(ulong id, string name)
		{
			bool findUserResult;
			bool findVacancyResult = false;
			IEnumerable<dynamic>? responses = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				var vacancy = await GetVacancies(employer!.Id, id).FirstOrDefaultAsync();
				if (findVacancyResult = vacancy != null)
				{
					responses = await GetResponses(employer!.Id, vacancyId: vacancy!.Id)
						.Select(r => new
						{
							r.Id,
							r.ResponseTime,
							IsLink = r.CV.Link != null,
							IsFile = r.CV.File != null,
							r.CV.Link
						}).ToListAsync();
				}
			}
			return (findUserResult, findVacancyResult, responses);
		}

		public async Task<(bool, dynamic?)> GetVacancyResponseCVFileAsync(ulong id, string name)
		{
			bool findUserResult;
			dynamic? responseCVFile = null;
			var employer = await _userManager.FindByNameAsync(name);
			if (findUserResult = employer != null)
			{
				responseCVFile = await GetResponses(employer!.Id, id: id)
					.Select(r => new
					{
						FileName = string.Format("CV_to_Vacansy_{0}",
							r.Vacancy.Title.Length > 7 ? $"{r.Vacancy.Title.Substring(0, 7)}..._" : r.Vacancy.Title),
						r.CV.FileFormat,
						File = r.CV.File != null ? Convert.ToBase64String(r.CV.File) : null
					}).FirstOrDefaultAsync();
			}
			return (findUserResult, responseCVFile);
		}

		private IQueryable<Vacancy> GetVacancies(string userID, ulong? id = null)
		{
			return _context.Vacancies
				.Where(v => v.EmployerID == userID)
				.Where(v => id == null || v.Id == id);
		}

		private IQueryable<Response> GetResponses(string userID, ulong? id = null, ulong? vacancyId = null)
		{
			return _context.Responses
				.Where(r => r.Vacancy.EmployerID == userID)
				.Where(r => id == null || r.Id == id)
				.Where(r => vacancyId == null || r.Vacancy.Id == vacancyId);
		}
	}
}
