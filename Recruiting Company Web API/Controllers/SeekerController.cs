using Microsoft.AspNetCore.Mvc;
using Recruiting_Company_Web_API.Services.SeekerServices.SeekerService;

namespace Recruiting_Company_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SeekerController : ControllerBase
	{
		private readonly ISeekerService _seekerService;

		public SeekerController(ISeekerService seekerService)
		{
			_seekerService = seekerService;
		}
	}
}
