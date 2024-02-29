using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Models.AccountModels
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Login is required")]
		[StringLength(50, ErrorMessage = "Login length must be lower than 50")]
		public string Login { get; set; } = default!;

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; } = default!;

		[Required(ErrorMessage = "Account Type is required")]
		[Range(1, 2, ErrorMessage = "Invalid Account Type value")]
		public short AccountType { get; set; }

		[Range(16, 150, ErrorMessage = "Age must be 16 or older")]
		public short? Age { get; set; }

		[StringLength(50, ErrorMessage = "Name length must be lower than 50")]
		public string? Name { get; set; }

		[StringLength(50, ErrorMessage = "Company Name length must be lower than 50")]
		public string? CompanyName { get; set; }
	}
}
