using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.TabsService
{
	public interface ITabsService
	{
		Task<(bool findUserResult, bool findVacancyResult)> AddVacansyToTabAsync(TabModel model, string name);
		Task<(bool findUserResult, bool findVacancyResult, bool findTabResult)> DeleteTabAsync(ulong id, string name);
		Task<(bool findUserResult, IEnumerable<dynamic>? tabs)> GetTabsAsync(string name);
	}
}
