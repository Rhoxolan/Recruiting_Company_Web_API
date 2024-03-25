using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.SeekerService
{
	public interface ISeekerService
	{
		Task<(bool modelValidResult, bool findUserResult, dynamic? cv)> UploadCVAsync(CVModel model, string name);
	}
}
