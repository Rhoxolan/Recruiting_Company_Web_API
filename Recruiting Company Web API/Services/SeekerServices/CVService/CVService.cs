using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.CVService
{
	public class CVService(UserManager<Seeker> userManager, ApplicationContext context) : ICVService
	{
		public async Task<(bool, IEnumerable<dynamic>?)> GetCVsAsync(string name)
		{
			bool findUserResult;
			IEnumerable<dynamic>? cvs = null;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				cvs = await context.CVs
					.Where(c => c.SeekerID == seeker!.Id)
					.Select(c => new
					{
						c.Id,
						c.UploadDate,
						File = c.File != null ? Convert.ToBase64String(c.File) : null,
						Format = c.File != null ? c.FileFormat : null,
						IsFile = c.File != null,
						IsLink = c.Link != null,
						Link = c.Link ?? null
					}).ToListAsync();
			}
			return (findUserResult, cvs);
		}

		#region UploadCV

		public async Task<(bool, bool, dynamic?)> UploadCVAsync(CVModel model, string name)
		{
			bool modelValidResult;
			bool findUserResult = false;
			dynamic? cv = null;
			if (modelValidResult = model.File != null && model.Link == null)
			{
				(findUserResult, cv) = await UploadCVAsync_Internal(model, name, UploadCVAsFileAsync);
			}
			else if (modelValidResult = model.File == null && model.Link != null)
			{
				(findUserResult, cv) = await UploadCVAsync_Internal(model, name, UploadCVAsLinkAsync);
			}
			return (modelValidResult, findUserResult, cv);
		}

		private async Task<(bool, dynamic?)> UploadCVAsync_Internal(CVModel model, string name,
			Func<CVModel, Seeker, Task<dynamic>> upload)
		{
			bool findUserResult;
			dynamic? cv = null;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				cv = await upload(model, seeker!);
			}
			return (findUserResult, cv);
		}

		private async Task<dynamic> UploadCVAsFileAsync(CVModel model, Seeker seeker)
		{
			var cvEntity = new CV
			{
				File = Convert.FromBase64String(model.File!),
				FileFormat = model.FileFormat,
				UploadDate = DateTime.Now,
				Seeker = seeker
			};
			await context.CVs.AddAsync(cvEntity);
			await context.SaveChangesAsync();
			return new
			{
				cvEntity.Id,
				model.File,
				cvEntity.FileFormat,
				cvEntity.UploadDate,
				IsFile = true,
				IsLink = false
			};
		}

		private async Task<dynamic> UploadCVAsLinkAsync(CVModel model, Seeker seeker)
		{
			var cvEntity = new CV
			{
				Link = model.Link,
				UploadDate = DateTime.Now,
				Seeker = seeker
			};
			await context.CVs.AddAsync(cvEntity);
			await context.SaveChangesAsync();
			return new
			{
				cvEntity.Id,
				model.Link,
				cvEntity.UploadDate,
				IsFile = false,
				IsLink = true
			};
		}

		#endregion

		public async Task<(bool, bool)> DeleteCVAsync(ulong id, string name)
		{
			bool findUserResult;
			bool findCVResult = false;
			var seeker = await userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				var cv = await context.CVs
					.Where(c => c.SeekerID == seeker!.Id)
					.Where(c => c.Id == id).FirstOrDefaultAsync();
				if (findCVResult = cv != null)
				{
					context.CVs.Remove(cv!);
					await context.SaveChangesAsync();
				}
			}
			return (findUserResult, findCVResult);
		}
	}
}
