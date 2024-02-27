using Microsoft.AspNetCore.Identity;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.AccountModels;
using Recruiting_Company_Web_API.Types.Exceptions;

namespace Recruiting_Company_Web_API.Services.AccountServices.AccountService
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<Employer> _employerManager;
		private readonly UserManager<Seeker> _seekerManager;

		public AccountService(UserManager<Employer> employerManager, UserManager<Seeker> seekerManager)
		{
			_employerManager = employerManager;
			_seekerManager = seekerManager;
		}

		public async Task<IdentityUser> CreateUserAsync(RegisterModel model)
		{
			(IdentityResult result, IdentityUser user) createResult = new();
			if (model.AccountType == 1)
			{
				createResult = await CreateSeekerAsync(model.Login, model.Age!.Value, model.Name!, model.Password);
			}
			else if (model.AccountType == 2)
			{
				createResult = await CreateEmployerAsync(model.Login, model.CompanyName!, model.Password);
			}
			if (!createResult.result!.Succeeded)
			{
				throw new RecruitingCompanyAuthenticationException(createResult.result!.Errors.First().Description);
			}
			return createResult.user!;
		}

		public async Task<IdentityUser> SignInUserAsync(LoginModel model)
		{
			(bool result, IdentityUser? user) signInResult = new();
			if (model.AccountType == 1)
			{
				signInResult = await SignInSeekerAsync(model.Login, model.Password);
			}
			else if (model.AccountType == 2)
			{
				signInResult = await SignInEmployerAsync(model.Login, model.Password);
			}
			if (signInResult.user == null || !signInResult.result)
			{
				throw new RecruitingCompanyAuthenticationException("Authentication failed!");
			}
			return signInResult.user;
		}

		private async Task<(IdentityResult, IdentityUser)> CreateSeekerAsync(string login, short age, string name, string password)
		{
			Seeker user = new Seeker
			{
				UserName = login,
				Age = age,
				Name = name
			};
			return (await _seekerManager.CreateAsync(user, password), user);
		}

		private async Task<(IdentityResult, IdentityUser)> CreateEmployerAsync(string login, string companyName, string password)
		{
			Employer user = new Employer
			{
				UserName = login,
				CompanyName = companyName
			};
			return (await _employerManager.CreateAsync(user, password), user);
		}

		private async Task<(bool, IdentityUser?)> SignInSeekerAsync(string login, string password)
		{
			var checkPassword = false;
			var user = await _seekerManager.FindByNameAsync(login);
			if (user != null)
			{
				checkPassword = await _seekerManager.CheckPasswordAsync(user, password);
			}
			return (checkPassword, user);
		}

		private async Task<(bool, IdentityUser?)> SignInEmployerAsync(string login, string password)
		{
			var checkPassword = false;
			var user = await _employerManager.FindByNameAsync(login);
			if (user != null)
			{
				checkPassword = await _employerManager.CheckPasswordAsync(user, password);
			}
			return (checkPassword, user);
		}
	}
}
