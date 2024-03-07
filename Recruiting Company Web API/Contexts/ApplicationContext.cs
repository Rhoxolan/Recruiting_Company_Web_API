using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recruiting_Company_Web_API.Entities;

namespace Recruiting_Company_Web_API.Contexts
{
	public class ApplicationContext : IdentityDbContext
	{
		public DbSet<IdentityUser> IdentityUsers { get; set; }
		public DbSet<Employer> Employers { get; set; }
		public DbSet<Seeker> Seekers { get; set; }
		public DbSet<Vacancy> Vacancies { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CV> CVs { get; set; }
		public DbSet<Response> Responses { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}
