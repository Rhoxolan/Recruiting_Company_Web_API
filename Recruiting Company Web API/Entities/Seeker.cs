﻿using Microsoft.AspNetCore.Identity;

namespace Recruiting_Company_Web_API.Entities
{
	public class Seeker : IdentityUser
	{
		public short Age { get; set; }

		public string Name { get; set; } = default!;
	}
}
