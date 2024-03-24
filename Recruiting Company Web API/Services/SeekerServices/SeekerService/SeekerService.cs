using Microsoft.AspNetCore.Identity;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;

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
	}
}
