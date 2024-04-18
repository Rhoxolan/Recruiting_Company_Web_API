using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;

namespace Recruiting_Company_Web_API.Services.GuestServices.GuestService
{
	public interface IGuestService
	{
		Task<ServiceResult<IEnumerable<dynamic>>> GetVacanciesAsync(VacancyRequestParameters requestParameters);
		Task<ServiceResult<int>> GetVacanciesCountAsync(VacancyRequestParameters requestParameters);
		Task<ServiceResult<dynamic>> GetVacancyAsync(ulong id);
	}
}
