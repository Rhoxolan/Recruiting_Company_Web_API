using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.DTOs.CVDTOs;
using Recruiting_Company_Web_API.DTOs.FileDTOs;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.CVService
{
	public class CVService(UserManager<Seeker> userManager, ApplicationContext context) : ICVService
	{
		public async Task<ServiceResult<List<CVDTO>>> GetCVsAsync(string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<List<CVDTO>>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var cvs = await context.CVs
				.Where(c => c.SeekerID == seeker.Id)
				.Select(c => new CVDTO
				{
					Id = c.Id,
					UploadDate = c.UploadDate,
					IsFile = c.File != null,
					IsLink = c.Link != null,
					Link = c.Link ?? null
				}).ToListAsync();
			return ServiceResult<List<CVDTO>>.Success(cvs);
		}

		public async Task<ServiceResult<FileDTO>> GetCVsFileAsync(ulong id, string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<FileDTO>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			var file = await context.CVs
				.Where(c => c.SeekerID == seeker.Id)
				.Where(c => c.File != null)
				.Where(c => c.Id == id)
				.Select(c => new FileDTO
				{
					File = c.File != null ? Convert.ToBase64String(c.File) : null,
					FileName = $"CV's file {c.Id}",
					FileFormat = c.FileFormat
				}).FirstOrDefaultAsync();
			if (file == null)
			{
				return ServiceResult<FileDTO>.Failure(ServiceErrorType.EntityNotFound, "The CV's file not found!");
			}
			return ServiceResult<FileDTO>.Success(file);
		}

		public async Task<ServiceResult<CVDTO>> UploadCVAsync(CVModel model, string name)
		{
			var seeker = await userManager.FindByNameAsync(name);
			if (seeker == null)
			{
				return ServiceResult<CVDTO>.Failure(ServiceErrorType.UserNotFound, "User not found!");
			}
			if (model.File != null && model.Link == null)
			{
				return ServiceResult<CVDTO>.Success(await UploadCVAsFileAsync(model, seeker));
			}
			else if (model.File == null && model.Link != null)
			{
				return ServiceResult<CVDTO>.Success(await UploadCVAsLinkAsync(model, seeker));
			}
			return ServiceResult<CVDTO>.Failure(ServiceErrorType.BadModel, "Bad Model!");
		}

		private async Task<CVDTO> UploadCVAsFileAsync(CVModel model, Seeker seeker)
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
			return new CVDTO
			{
				Id = cvEntity.Id,
				UploadDate = cvEntity.UploadDate,
				IsFile = true,
				IsLink = false,
				Link = cvEntity.Link
			};
		}

		private async Task<CVDTO> UploadCVAsLinkAsync(CVModel model, Seeker seeker)
		{
			var cvEntity = new CV
			{
				Link = model.Link,
				UploadDate = DateTime.Now,
				Seeker = seeker
			};
			await context.CVs.AddAsync(cvEntity);
			await context.SaveChangesAsync();
			return new CVDTO
			{
				Id = cvEntity.Id,
				Link = model.Link,
				UploadDate = cvEntity.UploadDate,
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
				.Where(c => c.SeekerID == seeker.Id)
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
