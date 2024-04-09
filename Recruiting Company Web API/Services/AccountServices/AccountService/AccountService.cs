using Microsoft.AspNetCore.Identity;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Models.AccountModels;

namespace Recruiting_Company_Web_API.Services.AccountServices.AccountService
{
	public class AccountService(UserManager<Employer> employerManager, UserManager<Seeker> seekerManager) : IAccountService
	{
		public async Task<(IdentityResult, IdentityUser)> CreateUserAsync(RegisterModel model)
		{
			if (model.AccountType == 1)
			{
				return await CreateSeekerAsync(model.Login, model.Age!.Value, model.Name!, model.Password);
			}
			else if (model.AccountType == 2)
			{
				return await CreateEmployerAsync(model.Login, model.CompanyName!, model.Password);
			}
			return default;
		}

		public async Task<(bool, IdentityUser?)> SignInUserAsync(LoginModel model)
		{
			if (model.AccountType == 1)
			{
				return await SignInSeekerAsync(model.Login, model.Password);
			}
			else if (model.AccountType == 2)
			{
				return await SignInEmployerAsync(model.Login, model.Password);
			}
			return default;
		}

		private async Task<(IdentityResult, IdentityUser)> CreateSeekerAsync(string login, short age, string name, string password)
		{
			Seeker user = new Seeker
			{
				UserName = login,
				Age = age,
				Name = name
			};
			return (await seekerManager.CreateAsync(user, password), user);
		}

		private async Task<(IdentityResult, IdentityUser)> CreateEmployerAsync(string login, string companyName, string password)
		{
			Employer user = new Employer
			{
				UserName = login,
				CompanyName = companyName,
				PublicId = Guid.NewGuid()
			};
			return (await employerManager.CreateAsync(user, password), user);
		}

		private async Task<(bool, IdentityUser?)> SignInSeekerAsync(string login, string password)
		{
			var checkPassword = false;
			var user = await seekerManager.FindByNameAsync(login);
			if (user != null)
			{
				checkPassword = await seekerManager.CheckPasswordAsync(user, password);
			}
			return (checkPassword, user);
		}

		private async Task<(bool, IdentityUser?)> SignInEmployerAsync(string login, string password)
		{
			var checkPassword = false;
			var user = await employerManager.FindByNameAsync(login);
			if (user != null)
			{
				checkPassword = await employerManager.CheckPasswordAsync(user, password);
			}
			return (checkPassword, user);
		}
	}
}
