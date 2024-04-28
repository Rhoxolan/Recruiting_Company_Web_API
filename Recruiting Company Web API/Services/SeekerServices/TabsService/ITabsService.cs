using Recruiting_Company_Web_API.DTOs.VacancyDTOs;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.TabsService
{
	public interface ITabsService
	{
		Task<ServiceResult> AddVacansyToTabAsync(TabModel model, string name);
		Task<ServiceResult> DeleteTabAsync(ulong id, string name);
		Task<ServiceResult<List<CommonVacancyDTO>>> GetTabsAsync(string name);
		Task<ServiceResult<bool>> CheckIsNotedAsync(ulong vacancyId, string name);
	}
}
