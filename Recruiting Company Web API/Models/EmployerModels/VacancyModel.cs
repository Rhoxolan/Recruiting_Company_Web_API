﻿using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Models.EmployerModels
{
	public class VacancyModel
	{
		[Required]
		public short CategoryID { get; set; }

		[Required]
		[MaxLength(100)]
		public string Title { get; set; } = default!;

		[Range(0, int.MaxValue)]
		public int? Salary { get; set; }

		[MaxLength(100)]
		public string? PhoneNumber { get; set; }

		[MaxLength(100)]
		public string? EMail { get; set; }

		[Required]
		[MinLength(100)]
		[MaxLength(1000)]
		public string Description { get; set; } = default!;
	}
}
