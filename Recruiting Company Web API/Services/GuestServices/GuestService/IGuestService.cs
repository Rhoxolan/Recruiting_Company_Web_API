using Recruiting_Company_Web_API.DTOs.VacancyDTOs;
using Recruiting_Company_Web_API.RequestParameters.GuestRequestParameters;

namespace Recruiting_Company_Web_API.Services.GuestServices.GuestService
{
	public interface IGuestService
	{
		Task<ServiceResult<List<CommonVacancyDTO>>> GetVacanciesAsync(VacancyRequestParameters requestParameters);
		Task<ServiceResult<int>> GetVacanciesCountAsync(VacancyRequestParameters requestParameters);
		Task<ServiceResult<CommonVacancyDTO>> GetVacancyAsync(ulong id);
	}
}
