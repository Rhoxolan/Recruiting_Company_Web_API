using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;

namespace Recruiting_Company_Web_API.Services.GuestServices.GuestService
{
	public class GuestService : IGuestService
	{
		private readonly ApplicationContext _context;

		public GuestService(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<int> GetVacanciesCountAsync(VacancyRequestParameters requestParameters)
		{
			return await GetVacancies(requestParameters).CountAsync();
		}

		public async Task<IEnumerable<dynamic>> GetVacanciesAsync(VacancyRequestParameters requestParameters)
		{
			return await GetVacancies(requestParameters)
						.Skip((requestParameters.PageNumber - 1) * requestParameters.Pagesize)
						.Take(requestParameters.Pagesize)
						.OrderBy(v => v.CreateDate)
						.Select(v => new
						{
							v.Id,
							CategoryID = v.Category.Id,
							EmployerID = v.Employer.PublicId,
							Employer = v.Employer.CompanyName,
							v.CreateDate,
							v.Title,
							v.Location,
							v.Salary,
							v.PhoneNumber,
							v.EMail,
							v.Description
						})
						.ToListAsync();
		}

		private IQueryable<Vacancy> GetVacancies(VacancyRequestParameters requestParameters)
		{
			return _context.Vacancies
						.Include(v => v.Category)
						.Include(v => v.Employer)
						.Where(v => requestParameters.CategoryID == null || v.Category.Id == requestParameters.CategoryID)
						.Where(v => requestParameters.EmployerID == null || v.Employer.PublicId.ToString() == requestParameters.EmployerID);
		}
	}
}
