using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recruiting_Company_Web_API.Contexts;
using Recruiting_Company_Web_API.Entities;
using Recruiting_Company_Web_API.Filters;
using Recruiting_Company_Web_API.Services.AccountServices.AccountService;
using Recruiting_Company_Web_API.Services.AuthenticationServices.JWTService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(opt
	=> opt.UseSqlServer(builder.Configuration.GetConnectionString("RecruitingCompanyDB")));

builder.Services.AddControllers();

builder.Services.AddCors(opt =>
	opt.AddDefaultPolicy(builder =>
	builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())); //Провести более тонкую настройку

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddIdentityCore<Employer>()
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddIdentityCore<Seeker>()
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddAuthentication(opt =>
{
	opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
	opt.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
	};
});

builder.Services.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddTransient<IJWTService, JWTService>()
	.AddTransient<IAccountService, AccountService>()
	.AddScoped<ValidateModelFilterAttribute>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
