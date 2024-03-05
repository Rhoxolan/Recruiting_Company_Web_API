using Microsoft.AspNetCore.Identity;

namespace Recruiting_Company_Web_API.Entities
{
	public class Employer : IdentityUser
	{
		public required string CompanyName { get; set; }
	}
}
