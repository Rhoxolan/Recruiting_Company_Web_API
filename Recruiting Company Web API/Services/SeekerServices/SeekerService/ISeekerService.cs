using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.SeekerService
{
	public interface ISeekerService
	{
		Task<(bool findUserResult, bool findVacancyResult)> AddVacansyToTabAsync(ulong id, string name);
		Task<(bool findUserResult, IEnumerable<dynamic>? cvs)> GetCVsAsync(string name);
		Task<(bool modelValidResult, bool findUserResult, dynamic? cv)> UploadCVAsync(CVModel model, string name);
	}
}
