using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Entities
{
	public class Employer : IdentityUser
	{
		[MaxLength(50)]
		public required string CompanyName { get; set; }

		public required Guid PublicId { get; set; }

		public ICollection<Vacancy> Vacancies { get; set; } = default!;
	}
}
