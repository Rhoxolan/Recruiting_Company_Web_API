using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Entities
{
	public class Seeker : IdentityUser
	{
		[Range(16, double.MaxValue)]
		public required short Age { get; set; }

		[MaxLength(50)]
		public required string Name { get; set; }

		public ICollection<CV> CVs { get; set; } = default!;
	}
}
