using Microsoft.AspNetCore.Identity;

namespace Recruiting_Company_Web_API.Entities
{
	public class Seeker : IdentityUser
	{
		public required short Age { get; set; }

		public required string Name { get; set; }
	}
}
