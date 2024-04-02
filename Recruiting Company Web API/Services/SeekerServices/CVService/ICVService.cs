using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.CVService
{
	public interface ICVService
	{
		Task<(bool findUserResult, IEnumerable<dynamic>? cvs)> GetCVsAsync(string name);
		Task<(bool modelValidResult, bool findUserResult, dynamic? cv)> UploadCVAsync(CVModel model, string name);
		Task<(bool findUserResult, bool findCVResult)> DeleteCVAsync(ulong id, string name);
	}
}
