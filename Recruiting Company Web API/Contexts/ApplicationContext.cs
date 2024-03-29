﻿using Microsoft.AspNetCore.Identity;
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
		public DbSet<SeekerTab> SeekersTabs { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Category>().HasData(InitCategories());
			builder.Entity<SeekerTab>().HasIndex("SeekerID", "VacancyID").IsUnique();
			base.OnModelCreating(builder);
		}
		
		private IEnumerable<Category> InitCategories()
		{
			string[] categoryNames = { "IT", "Освіта", "Наука", "Медицина", "Будівництво", "Важка промисловість",
				"Легка промисловість", "Авто", "Фінанси", "Торгівля", "Охорона", "Менеджмент", "Бізнес", "Краса",
				"Спорт", "Маркетинг", "Нерухомість", "Культура", "Сервіс", "Зоо", "Юриспруденція", "Вантаж",
				"Робота за кордоном", "Технічне обслуговування", "Адміністрація" };

            for (int i = 0; i < categoryNames.Length; i++)
            {
				yield return new Category { Id = (short)(i + 1), Name = categoryNames[i] };
            }
        }
	}
}
