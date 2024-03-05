﻿using System.ComponentModel.DataAnnotations;

namespace Recruiting_Company_Web_API.Entities
{
	public class Category
	{
		public short Id { get; set; }

		[MaxLength(15)]
		public required string Name { get; set; }
	}
}
