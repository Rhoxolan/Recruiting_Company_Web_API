using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;

namespace Recruiting_Company_Web_API.Services.GuestServices.GuestService
{
	public interface IGuestService
	{
		Task<IEnumerable<dynamic>> GetVacanciesAsync(VacancyRequestParameters requestParameters);
	}
}
