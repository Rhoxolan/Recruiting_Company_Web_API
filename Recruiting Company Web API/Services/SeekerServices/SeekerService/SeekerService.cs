using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.SeekerModels;

namespace Recruiting_Company_Web_API.Services.SeekerServices.SeekerService
{
	public class SeekerService : ISeekerService
	{
		private readonly UserManager<Seeker> _userManager;
		private readonly ApplicationContext _context;

		public SeekerService(UserManager<Seeker> userManager, ApplicationContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public async Task<(bool, IEnumerable<dynamic>?)> GetCVsAsync(string name)
		{
			bool findUserResult;
			IEnumerable<dynamic>? cvs = null;
			var seeker = await _userManager.FindByNameAsync(name);
			if (findUserResult = seeker != null)
			{
				cvs = await _context.CVs
					.Where(c => c.SeekerID == seeker!.Id)
					.Select(c => new
					{
						c.Id,
						c.UploadDate,
						//...
					}).ToListAsync();
			}
			return (findUserResult, cvs);
		}

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
			var seeker = await _userManager.FindByNameAsync(name);
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
			await _context.CVs.AddAsync(cvEntity);
			await _context.SaveChangesAsync();
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
			await _context.CVs.AddAsync(cvEntity);
			await _context.SaveChangesAsync();
			return new
			{
				cvEntity.Id,
				model.Link,
				cvEntity.UploadDate,
				IsFile = false,
				IsLink = true
			};
		}
	}
}
