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

		public async Task<IEnumerable<dynamic>> GetVacanciesAsync(VacancyRequestParameters requestParameters)
		{
			var query = _context.Vacancies
				.Include(v => v.Category)
				.Include(v => v.Employer)
				.Skip((requestParameters.PageNumber - 1) * requestParameters.Pagesize)
				.Take(requestParameters.Pagesize);

			AddFilters(ref query, requestParameters);

			return await query
				.Select(v => new
				{
					v.Id,
					CategoryID = v.Category.Id,
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

		private void AddFilters(ref IQueryable<Vacancy> query, VacancyRequestParameters requestParameters)
		{
			IQueryable<Vacancy> newQuery = query;

			if (requestParameters.CategoryID != null)
			{
				newQuery = newQuery.Where(v => v.Category.Id == requestParameters.CategoryID);
			}

			query = newQuery;
		}

	}
}
