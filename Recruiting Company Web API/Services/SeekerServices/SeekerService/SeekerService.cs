using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.SeekerService
{
	public class SeekerService : ISeekerService
	{
		private readonly UserManager<Seeker> _userManager;
		private readonly ApplicationContext _context;

		public SeekerService(UserManager<Seeker> userManager, ApplicationContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		#region CV

		public async Task<(bool, IEnumerable<dynamic>?)> GetCVsAsync(string name)
		{
			bool findUserResult;
			IEnumerable<dynamic>? cvs = null;
			var seeker = await _userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				cvs = await _context.CVs
					.Where(c => c.SeekerID == seeker!.Id)
					.Select(c => new
					{
						c.Id,
						c.UploadDate,
						File = c.File != null ? Convert.ToBase64String(c.File) : null,
						Format = c.File != null ? c.FileFormat : null,
						IsFile = c.File != null,
						IsLink = c.Link != null,
						Link = c.Link ?? null
					}).ToListAsync();
			}
			return (findUserResult, cvs);
		}

		#region UploadCV

		public async Task<(bool, bool, dynamic?)> UploadCVAsync(CVModel model, string name)
		{
			bool modelValidResult;
			bool findUserResult = false;
			dynamic? cv = null;
			if (modelValidResult = model.File != null && model.Link == null)
			{
				(findUserResult, cv) = await UploadCVAsync_Internal(model, name, UploadCVAsFileAsync);
			}
			else if (modelValidResult = model.File == null && model.Link != null)
			{
				(findUserResult, cv) = await UploadCVAsync_Internal(model, name, UploadCVAsLinkAsync);
			}
			return (modelValidResult, findUserResult, cv);
		}

		private async Task<(bool, dynamic?)> UploadCVAsync_Internal(CVModel model, string name,
			Func<CVModel, Seeker, Task<dynamic>> upload)
		{
			bool findUserResult;
			dynamic? cv = null;
			var seeker = await _userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				cv = await upload(model, seeker!);
			}
			return (findUserResult, cv);
		}

		private async Task<dynamic> UploadCVAsFileAsync(CVModel model, Seeker seeker)
		{
			var cvEntity = new CV
			{
				File = Convert.FromBase64String(model.File!),
				FileFormat = model.FileFormat,
				UploadDate = DateTime.Now,
				Seeker = seeker
			};
			await _context.CVs.AddAsync(cvEntity);
			await _context.SaveChangesAsync();
			return new
			{
				cvEntity.Id,
				model.File,
				cvEntity.FileFormat,
				cvEntity.UploadDate,
				IsFile = true,
				IsLink = false
			};
		}

		private async Task<dynamic> UploadCVAsLinkAsync(CVModel model, Seeker seeker)
		{
			var cvEntity = new CV
			{
				Link = model.Link,
				UploadDate = DateTime.Now,
				Seeker = seeker
			};
			await _context.CVs.AddAsync(cvEntity);
			await _context.SaveChangesAsync();
			return new
			{
				cvEntity.Id,
				model.Link,
				cvEntity.UploadDate,
				IsFile = false,
				IsLink = true
			};
		}

		#endregion

		public async Task<(bool, bool)> DeleteCVAsync(ulong id, string name)
		{
			bool findUserResult;
			bool findCVResult = false;
			var seeker = await _userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				var cv = await _context.CVs
					.Where(c => c.SeekerID == seeker!.Id)
					.Where(c => c.Id == id).FirstOrDefaultAsync();
				if (findCVResult = cv != null)
				{
					_context.CVs.Remove(cv!);
					await _context.SaveChangesAsync();
				}
			}
			return (findUserResult, findCVResult);
		}

		#endregion

		#region Response

		public async Task<(bool, bool, bool, dynamic?)> RespondToVacancyAsync(ResponseModel model, string name)
		{
			bool findUserResult;
			bool findVacancyResult = false;
			bool findCVResult = false;
			dynamic? response = null;
			var seeker = await _userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				var vacancy = await _context.Vacancies.FindAsync(model.VacancyId);
				if (findVacancyResult = vacancy != null)
				{
					var cv = await _context.CVs
						.Where(c => c.SeekerID == seeker!.Id)
						.Where(c => c.Id == model.CVId)
						.FirstOrDefaultAsync();
					if (findCVResult = cv != null)
					{
						var responseEntity = new Response
						{
							ResponseTime = DateTime.Now,
							Vacancy = vacancy!,
							CV = cv!
						};
						await _context.Responses.AddAsync(responseEntity);
						await _context.SaveChangesAsync();
						response = new
						{
							responseEntity.Id,
							responseEntity.ResponseTime,
							responseEntity.VacancyID,
							CVId = cv!.Id,
							CVUploadDate = cv.UploadDate,
							File = cv.File != null ? Convert.ToBase64String(cv.File) : null,
							Format = cv.File != null ? cv.FileFormat : null,
							IsFile = cv.File != null,
							IsLink = cv.Link != null,
							Link = cv.Link ?? null
						};
					}
				}
			}
			return (findUserResult, findVacancyResult, findCVResult, response);
		}

		#endregion

		#region Tabs

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

		#endregion
	}
}
