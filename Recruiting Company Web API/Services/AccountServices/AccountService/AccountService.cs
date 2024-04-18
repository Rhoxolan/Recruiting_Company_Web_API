using Microsoft.AspNetCore.Identity;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.AccountModels;

namespace Recruiting_Company_Web_API.Services.AccountServices.AccountService
{
	public class AccountService(UserManager<Employer> employerManager, UserManager<Seeker> seekerManager) : IAccountService
	{
		public async Task<ServiceResult<IdentityUser>> CreateUserAsync(RegisterModel model)
		{
			if (model.AccountType == 1)
			{
				if (model.Age == null || model.Name == null)
				{
					return ServiceResult<IdentityUser>.Failure(ServiceErrorType.BadModel, model.Age == null ? "Age is null" : "Name is null");
				}
				return await CreateSeekerAsync(model.Login, model.Age.Value, model.Name, model.Password);
			}
			else if (model.AccountType == 2)
			{
				if (model.CompanyName == null)
				{
					return ServiceResult<IdentityUser>.Failure(ServiceErrorType.BadModel, "Company name is null");
				}
				return await CreateEmployerAsync(model.Login, model.CompanyName, model.Password);
			}
			return ServiceResult<IdentityUser>.Failure(ServiceErrorType.BadModel, "The account type must be 1 or 2");
		}

		private async Task<ServiceResult<IdentityUser>> CreateSeekerAsync(string login, short age, string name, string password)
		{
			Seeker seeker = new Seeker
			{
				UserName = login,
				Age = age,
				Name = name
			};
			var result = await seekerManager.CreateAsync(seeker, password);
			if (!result.Succeeded)
			{
				return ServiceResult<IdentityUser>.Failure(ServiceErrorType.Fault, result.Errors.First().Description);
			}
			return ServiceResult<IdentityUser>.Success(seeker);
		}

		private async Task<ServiceResult<IdentityUser>> CreateEmployerAsync(string login, string companyName, string password)
		{
			Employer employer = new Employer
			{
				UserName = login,
				CompanyName = companyName,
				PublicId = Guid.NewGuid()
			};
			var result = await employerManager.CreateAsync(employer, password);
			if (!result.Succeeded)
			{
				return ServiceResult<IdentityUser>.Failure(ServiceErrorType.Fault, result.Errors.First().Description);
			}
			return ServiceResult<IdentityUser>.Success(employer);
		}

		public async Task<ServiceResult<IdentityUser>> SignInUserAsync(LoginModel model)
		{
			if (model.AccountType == 1)
			{
				return await SignInSeekerAsync(model.Login, model.Password);
			}
			else if (model.AccountType == 2)
			{
				return await SignInEmployerAsync(model.Login, model.Password);
			}
			return ServiceResult<IdentityUser>.Failure(ServiceErrorType.BadModel, "The account type must be 1 or 2");
		}

		private async Task<ServiceResult<IdentityUser>> SignInSeekerAsync(string login, string password)
		{
			var seeker = await seekerManager.FindByNameAsync(login);
			if (seeker == null)
			{
				return ServiceResult<IdentityUser>.Failure(ServiceErrorType.UserNotFound, "User not found");
			}
			if (!await seekerManager.CheckPasswordAsync(seeker, password))
			{
				return ServiceResult<IdentityUser>.Failure(ServiceErrorType.Unauthorized, "Incorrect password");
			}
			return ServiceResult<IdentityUser>.Success(seeker);
		}

		private async Task<ServiceResult<IdentityUser>> SignInEmployerAsync(string login, string password)
		{
			var employer = await employerManager.FindByNameAsync(login);
			if (employer == null)
			{
				return ServiceResult<IdentityUser>.Failure(ServiceErrorType.UserNotFound, "User not found");
			}
			if (!await employerManager.CheckPasswordAsync(employer, password))
			{
				return ServiceResult<IdentityUser>.Failure(ServiceErrorType.Unauthorized, "Incorrect password");
			}
			return ServiceResult<IdentityUser>.Success(employer);
		}
	}
}
