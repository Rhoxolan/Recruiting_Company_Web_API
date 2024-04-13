using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.CVService
{
	public interface ICVService
	{
		Task<ServiceResult<IEnumerable<dynamic>>> GetCVsAsync(string name);
		Task<ServiceResult<dynamic>> UploadCVAsync(CVModel model, string name);
		Task<ServiceResult> DeleteCVAsync(ulong id, string name);
	}
}
