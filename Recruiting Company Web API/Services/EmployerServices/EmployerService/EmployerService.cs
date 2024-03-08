using Recruiting_Company_Web_API.Contexts;

namespace Recruiting_Company_Web_API.Services.EmployerServices.EmployerService
{
	public class EmployerService : IEmployerService
	{
		private readonly ApplicationContext _context;

		public EmployerService(ApplicationContext context)
		{
			_context = context;
		}
	}
}
