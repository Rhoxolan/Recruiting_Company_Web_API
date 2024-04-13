using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.CVService
{
	public class CVService(UserManager<Seeker> userManager, ApplicationContext context) : ICVService
	{
		public async Task<ServiceResult<IEnumerable<dynamic>>> GetCVsAsync(string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<IEnumerable<dynamic>>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var cvs = await context.CVs
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
			return ServiceResult<IEnumerable<dynamic>>.Success(cvs);
		}

		public async Task<ServiceResult<dynamic>> UploadCVAsync(CVModel model, string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<dynamic>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			if (model.File != null && model.Link == null)
			{
				return ServiceResult<dynamic>.Success(await UploadCVAsFileAsync(model, seeker));
			}
			else if (model.File == null && model.Link != null)
			{
				return ServiceResult<dynamic>.Success(await UploadCVAsLinkAsync(model, seeker));
			}
			return ServiceResult<dynamic>.Failure(ServiceErrorType.BadModel, "Bad Model!");
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

		public async Task<ServiceResult> DeleteCVAsync(ulong id, string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var cv = await context.CVs
				.Where(c => c.SeekerID == seeker!.Id)
				.Where(c => c.Id == id).FirstOrDefaultAsync();
			if (cv == null)
			{
				return ServiceResult.Failure(ServiceErrorType.EntityNotFound, "CV not found!");
			}
			context.CVs.Remove(cv!);
			await context.SaveChangesAsync();
			return ServiceResult.Success();
		}
	}
}
